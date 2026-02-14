/* Тамекло, Коку Эдмон */
/* Часть 3: Система управления мероприятиями (Вариант 10) */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EventManagementSystem
{
    // Интерфейс для бронирования
    public interface IBookable
    {
        bool Book(int participants);
        bool CancelBooking();
        int AvailableSpaces { get; }
    }

    // Интерфейс для событий с оплатой
    public interface IPayable
    {
        decimal CalculatePrice(int participants);
        decimal PricePerPerson { get; }
    }

    // Абстрактный класс Event
    public abstract class Event
    {
        public string EventId { get; protected set; }
        public string Name { get; protected set; }
        public DateTime Date { get; protected set; }
        public string Location { get; protected set; }
        public string Organizer { get; protected set; }
        public string Description { get; protected set; }

        public static int TotalEvents { get; private set; }

        protected Event(string id, string name, DateTime date, string location, string organizer)
        {
            EventId = id;
            Name = name;
            Date = date;
            Location = location;
            Organizer = organizer;
            Description = "";
            TotalEvents++;
        }

        public abstract string GetEventType();
        public abstract void DisplayFullInfo();

        public virtual void UpdateDescription(string newDescription)
        {
            Description = newDescription;
        }

        public bool IsPastEvent()
        {
            return Date < DateTime.Now;
        }

        ~Event()
        {
            TotalEvents--;
        }
    }

    // Конкретный класс: Конференция
    public class Conference : Event, IBookable, IPayable
    {
        public string Speaker { get; private set; }
        public string Topic { get; private set; }
        public int MaxParticipants { get; private set; }
        public int BookedParticipants { get; private set; }
        public decimal PricePerPerson { get; private set; }
        public bool HasWorkshops { get; private set; }

        public int AvailableSpaces => MaxParticipants - BookedParticipants;

        public Conference(string id, string name, DateTime date, string location,
                         string organizer, string speaker, string topic,
                         int maxParticipants, decimal pricePerPerson, bool hasWorkshops)
            : base(id, name, date, location, organizer)
        {
            Speaker = speaker;
            Topic = topic;
            MaxParticipants = maxParticipants;
            PricePerPerson = pricePerPerson;
            HasWorkshops = hasWorkshops;
            BookedParticipants = 0;
        }

        public override string GetEventType() => "Конференция";

        public override void DisplayFullInfo()
        {
            Console.WriteLine($"=== КОНФЕРЕНЦИЯ ===");
            Console.WriteLine($"ID: {EventId}");
            Console.WriteLine($"Название: {Name}");
            Console.WriteLine($"Дата: {Date:dd.MM.yyyy HH:mm}");
            Console.WriteLine($"Место: {Location}");
            Console.WriteLine($"Организатор: {Organizer}");
            Console.WriteLine($"Спикер: {Speaker}");
            Console.WriteLine($"Тема: {Topic}");
            Console.WriteLine($"Участники: {BookedParticipants}/{MaxParticipants}");
            Console.WriteLine($"Цена: {PricePerPerson} руб/чел");
            Console.WriteLine($"Воркшопы: {(HasWorkshops ? "Да" : "Нет")}");
            Console.WriteLine($"Описание: {Description}");
            Console.WriteLine($"Статус: {(IsPastEvent() ? "Завершено" : "Предстоящее")}");
        }

        public bool Book(int participants)
        {
            if (participants <= 0 || participants > AvailableSpaces)
                return false;

            BookedParticipants += participants;
            return true;
        }

        public bool CancelBooking()
        {
            if (BookedParticipants == 0)
                return false;

            BookedParticipants = 0;
            return true;
        }

        public decimal CalculatePrice(int participants)
        {
            decimal price = participants * PricePerPerson;
            if (HasWorkshops)
                price = price * 1.2m; // +20% за воркшопы
            return price;
        }
    }

    // Конкретный класс: Концерт
    public class Concert : Event, IBookable, IPayable
    {
        public string Artist { get; private set; }
        public string Genre { get; private set; }
        public int Capacity { get; private set; }
        public int TicketsSold { get; private set; }
        public decimal TicketPrice { get; private set; }
        public bool IsOutdoor { get; private set; }

        public int AvailableSpaces => Capacity - TicketsSold;
        public decimal PricePerPerson => TicketPrice;

        public Concert(string id, string name, DateTime date, string location,
                      string organizer, string artist, string genre,
                      int capacity, decimal ticketPrice, bool isOutdoor)
            : base(id, name, date, location, organizer)
        {
            Artist = artist;
            Genre = genre;
            Capacity = capacity;
            TicketPrice = ticketPrice;
            IsOutdoor = isOutdoor;
            TicketsSold = 0;
        }

        public override string GetEventType() => "Концерт";

        public override void DisplayFullInfo()
        {
            Console.WriteLine($"=== КОНЦЕРТ ===");
            Console.WriteLine($"ID: {EventId}");
            Console.WriteLine($"Название: {Name}");
            Console.WriteLine($"Дата: {Date:dd.MM.yyyy HH:mm}");
            Console.WriteLine($"Место: {Location}");
            Console.WriteLine($"Организатор: {Organizer}");
            Console.WriteLine($"Артист: {Artist}");
            Console.WriteLine($"Жанр: {Genre}");
            Console.WriteLine($"Билеты: {TicketsSold}/{Capacity}");
            Console.WriteLine($"Цена билета: {TicketPrice} руб");
            Console.WriteLine($"На открытом воздухе: {(IsOutdoor ? "Да" : "Нет")}");
            Console.WriteLine($"Описание: {Description}");
            Console.WriteLine($"Статус: {(IsPastEvent() ? "Завершено" : "Предстоящее")}");
        }

        public bool Book(int tickets)
        {
            if (tickets <= 0 || tickets > AvailableSpaces)
                return false;

            TicketsSold += tickets;
            return true;
        }

        public bool CancelBooking()
        {
            if (TicketsSold == 0)
                return false;

            TicketsSold = 0;
            return true;
        }

        public decimal CalculatePrice(int tickets)
        {
            return tickets * TicketPrice;
        }
    }

    // Конкретный класс: Воркшоп
    public class Workshop : Event, IBookable, IPayable
    {
        public string Instructor { get; private set; }
        public string SkillLevel { get; private set; }
        public int MaxAttendees { get; private set; }
        public int RegisteredAttendees { get; private set; }
        public decimal WorkshopFee { get; private set; }
        public int DurationHours { get; private set; }

        public int AvailableSpaces => MaxAttendees - RegisteredAttendees;
        public decimal PricePerPerson => WorkshopFee;

        public Workshop(string id, string name, DateTime date, string location,
                       string organizer, string instructor, string skillLevel,
                       int maxAttendees, decimal workshopFee, int durationHours)
            : base(id, name, date, location, organizer)
        {
            Instructor = instructor;
            SkillLevel = skillLevel;
            MaxAttendees = maxAttendees;
            WorkshopFee = workshopFee;
            DurationHours = durationHours;
            RegisteredAttendees = 0;
        }

        public override string GetEventType() => "Воркшоп";

        public override void DisplayFullInfo()
        {
            Console.WriteLine($"=== ВОРКШОП ===");
            Console.WriteLine($"ID: {EventId}");
            Console.WriteLine($"Название: {Name}");
            Console.WriteLine($"Дата: {Date:dd.MM.yyyy HH:mm}");
            Console.WriteLine($"Место: {Location}");
            Console.WriteLine($"Организатор: {Organizer}");
            Console.WriteLine($"Инструктор: {Instructor}");
            Console.WriteLine($"Уровень: {SkillLevel}");
            Console.WriteLine($"Участники: {RegisteredAttendees}/{MaxAttendees}");
            Console.WriteLine($"Стоимость: {WorkshopFee} руб");
            Console.WriteLine($"Длительность: {DurationHours} часов");
            Console.WriteLine($"Описание: {Description}");
            Console.WriteLine($"Статус: {(IsPastEvent() ? "Завершено" : "Предстоящее")}");
        }

        public bool Book(int attendees)
        {
            if (attendees <= 0 || attendees > AvailableSpaces)
                return false;

            RegisteredAttendees += attendees;
            return true;
        }

        public bool CancelBooking()
        {
            if (RegisteredAttendees == 0)
                return false;

            RegisteredAttendees = 0;
            return true;
        }

        public decimal CalculatePrice(int attendees)
        {
            return attendees * WorkshopFee;
        }
    }

    // Статический класс для управления мероприятиями
    public static class EventManager
    {
        public static string CompanyName { get; set; } = "EventPro Management";
        private static List<Event> events = new List<Event>();
        private static string dataFile = "events_data.txt";

        public static void AddEvent(Event eventItem)
        {
            events.Add(eventItem);
            Console.WriteLine($"Мероприятие '{eventItem.Name}' добавлено.");
            LogToFile($"Добавлено мероприятие: {eventItem.Name} ({eventItem.GetEventType()})");
        }

        public static void RemoveEvent(string eventId)
        {
            var eventToRemove = events.FirstOrDefault(e => e.EventId == eventId);
            if (eventToRemove != null)
            {
                events.Remove(eventToRemove);
                Console.WriteLine($"Мероприятие '{eventToRemove.Name}' удалено.");
                LogToFile($"Удалено мероприятие: {eventToRemove.Name}");
            }
            else
            {
                Console.WriteLine($"Мероприятие с ID {eventId} не найдено.");
            }
        }

        public static void DisplayAllEvents()
        {
            Console.WriteLine($"\n=== ВСЕ МЕРОПРИЯТИЯ ({events.Count}) ===");
            if (events.Count == 0)
            {
                Console.WriteLine("Список мероприятий пуст.");
            }
            else
            {
                foreach (var e in events)
                {
                    Console.WriteLine($"- {e.Name} ({e.GetEventType()}) - {e.Date:dd.MM.yyyy} - ID: {e.EventId}");
                }
            }
        }

        public static void DisplayEventsByType(string type)
        {
            var filtered = events.Where(e => e.GetEventType().ToLower() == type.ToLower()).ToList();
            Console.WriteLine($"\n=== {type.ToUpper()} ({filtered.Count}) ===");

            if (filtered.Count == 0)
            {
                Console.WriteLine($"Мероприятия типа '{type}' не найдены.");
            }
            else
            {
                foreach (var e in filtered)
                {
                    e.DisplayFullInfo();
                    Console.WriteLine();
                }
            }
        }

        public static void DisplayEventDetails(string eventId)
        {
            var eventItem = events.FirstOrDefault(e => e.EventId == eventId);
            if (eventItem != null)
            {
                eventItem.DisplayFullInfo();
            }
            else
            {
                Console.WriteLine($"Мероприятие с ID {eventId} не найдено.");
            }
        }

        public static void BookEvent(string eventId, int participants)
        {
            var eventItem = events.FirstOrDefault(e => e.EventId == eventId);
            if (eventItem == null)
            {
                Console.WriteLine($"Мероприятие с ID {eventId} не найдено.");
                return;
            }

            if (eventItem is IBookable bookable)
            {
                if (bookable.Book(participants))
                {
                    Console.WriteLine($"Успешно забронировано {participants} мест на мероприятие '{eventItem.Name}'.");

                    if (eventItem is IPayable payable)
                    {
                        decimal totalPrice = payable.CalculatePrice(participants);
                        Console.WriteLine($"Общая стоимость: {totalPrice} руб");
                    }

                    LogToFile($"Бронирование: {eventItem.Name}, {participants} мест");
                }
                else
                {
                    Console.WriteLine($"Ошибка бронирования! Доступно мест: {bookable.AvailableSpaces}");
                }
            }
            else
            {
                Console.WriteLine("Это мероприятие не поддерживает бронирование.");
            }
        }

        public static void CancelBookingEvent(string eventId)
        {
            var eventItem = events.FirstOrDefault(e => e.EventId == eventId);
            if (eventItem == null)
            {
                Console.WriteLine($"Мероприятие с ID {eventId} не найдено.");
                return;
            }

            if (eventItem is IBookable bookable)
            {
                if (bookable.CancelBooking())
                {
                    Console.WriteLine($"Бронирование на мероприятие '{eventItem.Name}' отменено.");
                    LogToFile($"Отмена бронирования: {eventItem.Name}");
                }
                else
                {
                    Console.WriteLine($"Нет активных бронирований для мероприятия '{eventItem.Name}'.");
                }
            }
        }

        public static decimal CalculateTotalRevenue()
        {
            decimal total = 0;
            foreach (var e in events)
            {
                if (e is Concert concert)
                    total += concert.TicketsSold * concert.TicketPrice;
                else if (e is Workshop workshop)
                    total += workshop.RegisteredAttendees * workshop.WorkshopFee;
                else if (e is Conference conference)
                    total += conference.BookedParticipants * conference.PricePerPerson;
            }
            return total;
        }

        public static void ShowStatistics()
        {
            Console.WriteLine($"\n=== СТАТИСТИКА {CompanyName} ===");
            Console.WriteLine($"Всего мероприятий: {events.Count}");
            Console.WriteLine($"Конференций: {events.OfType<Conference>().Count()}");
            Console.WriteLine($"Концертов: {events.OfType<Concert>().Count()}");
            Console.WriteLine($"Воркшопов: {events.OfType<Workshop>().Count()}");

            int totalBooked = 0;
            foreach (var e in events)
            {
                if (e is Conference c) totalBooked += c.BookedParticipants;
                else if (e is Concert ct) totalBooked += ct.TicketsSold;
                else if (e is Workshop w) totalBooked += w.RegisteredAttendees;
            }

            Console.WriteLine($"Всего забронировано мест: {totalBooked}");
            Console.WriteLine($"Ожидаемая выручка: {CalculateTotalRevenue()} руб");

            var upcomingEvents = events.Where(e => !e.IsPastEvent()).Count();
            Console.WriteLine($"Предстоящих мероприятий: {upcomingEvents}");
            Console.WriteLine($"Прошедших мероприятий: {events.Count - upcomingEvents}");
        }

        public static void SaveToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(dataFile))
                {
                    writer.WriteLine($"Компания: {CompanyName}");
                    writer.WriteLine($"Дата экспорта: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
                    writer.WriteLine($"Всего мероприятий: {events.Count}");
                    writer.WriteLine("===========================================");

                    foreach (var e in events)
                    {
                        writer.WriteLine($"Тип: {e.GetEventType()}");
                        writer.WriteLine($"ID: {e.EventId}");
                        writer.WriteLine($"Название: {e.Name}");
                        writer.WriteLine($"Дата: {e.Date:yyyy-MM-dd HH:mm}");
                        writer.WriteLine($"Место: {e.Location}");
                        writer.WriteLine($"Организатор: {e.Organizer}");
                        writer.WriteLine($"Описание: {e.Description}");

                        if (e is Conference conf)
                        {
                            writer.WriteLine($"Спикер: {conf.Speaker}");
                            writer.WriteLine($"Тема: {conf.Topic}");
                            writer.WriteLine($"Участники: {conf.BookedParticipants}/{conf.MaxParticipants}");
                            writer.WriteLine($"Цена: {conf.PricePerPerson} руб");
                        }
                        else if (e is Concert concert)
                        {
                            writer.WriteLine($"Артист: {concert.Artist}");
                            writer.WriteLine($"Жанр: {concert.Genre}");
                            writer.WriteLine($"Билеты: {concert.TicketsSold}/{concert.Capacity}");
                            writer.WriteLine($"Цена: {concert.TicketPrice} руб");
                        }
                        else if (e is Workshop workshop)
                        {
                            writer.WriteLine($"Инструктор: {workshop.Instructor}");
                            writer.WriteLine($"Уровень: {workshop.SkillLevel}");
                            writer.WriteLine($"Участники: {workshop.RegisteredAttendees}/{workshop.MaxAttendees}");
                            writer.WriteLine($"Стоимость: {workshop.WorkshopFee} руб");
                            writer.WriteLine($"Длительность: {workshop.DurationHours} часов");
                        }

                        writer.WriteLine("---");
                    }
                }
                Console.WriteLine($"Данные сохранены в файл: {dataFile}");
                LogToFile("Сохранение данных в файл");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении: {ex.Message}");
            }
        }

        public static void LoadFromFile()
        {
            if (!File.Exists(dataFile))
            {
                Console.WriteLine($"Файл {dataFile} не найден.");
                return;
            }

            try
            {
                string[] lines = File.ReadAllLines(dataFile);
                Console.WriteLine($"\nСодержимое файла {dataFile}:");
                foreach (string line in lines)
                {
                    Console.WriteLine(line);
                }
                LogToFile("Загрузка данных из файла");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке: {ex.Message}");
            }
        }

        private static void LogToFile(string message)
        {
            try
            {
                string logFile = "event_log.txt";
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                File.AppendAllText(logFile, $"[{timestamp}] {message}\n");
            }
            catch { }
        }
    }

    // Основной класс программы
    class Program
    {
        static List<Event> events = new List<Event>();

        static void Main(string[] args)
        {
            InitializeSampleEvents();
            ShowWelcomeMessage();

            bool running = true;
            while (running)
            {
                ShowMenu();
                string command = Console.ReadLine()?.Trim() ?? "";

                switch (command)
                {
                    case "1": AddNewEvent(); break;
                    case "2": ShowAllEvents(); break;
                    case "3": ShowEventsByType(); break;
                    case "4": ShowEventDetails(); break;
                    case "5": BookEvent(); break;
                    case "6": CancelBooking(); break;
                    case "7": EventManager.ShowStatistics(); break;
                    case "8": EventManager.SaveToFile(); break;
                    case "9": EventManager.LoadFromFile(); break;
                    case "10": ShowSystemInfo(); break;
                    case "11": ShowHelp(); break;
                    case "12": running = false; break;
                    default: Console.WriteLine("Неизвестная команда."); break;
                }

                if (running)
                {
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }
        }

        static void InitializeSampleEvents()
        {
            var conf1 = new Conference(
                "CONF001", "ИТ-Конференция 2026",
                new DateTime(2026, 3, 15, 10, 0, 0),
                "Конгресс-центр", "ИТ-Академия",
                "Иван Петров", "Искусственный интеллект",
                200, 5000m, true
            );
            conf1.UpdateDescription("Ежегодная конференция по IT-технологиям");
            EventManager.AddEvent(conf1);

            var concert1 = new Concert(
                "CONC001", "Рок-фестиваль",
                new DateTime(2026, 6, 20, 19, 0, 0),
                "Стадион", "RockPromo",
                "Металлика", "Рок",
                50000, 3000m, true
            );
            concert1.UpdateDescription("Грандиозный рок-фестиваль под открытым небом");
            EventManager.AddEvent(concert1);

            var workshop1 = new Workshop(
                "WORK001", "Веб-разработка",
                new DateTime(2026, 4, 10, 9, 0, 0),
                "Офис Google", "TechSchool",
                "Алексей Сидоров", "Начальный",
                30, 10000m, 8
            );
            workshop1.UpdateDescription("Интенсивный курс по веб-разработке");
            EventManager.AddEvent(workshop1);

            var conf2 = new Conference(
                "CONF002", "Медицинский форум",
                new DateTime(2026, 5, 5, 10, 0, 0),
                "Медицинский центр", "Минздрав",
                "Елена Смирнова", "Новые технологии в медицине",
                150, 4000m, false
            );
            EventManager.AddEvent(conf2);
        }

        static void ShowWelcomeMessage()
        {
            Console.Clear();
            Console.WriteLine("===========================================");
            Console.WriteLine("     СИСТЕМА УПРАВЛЕНИЯ МЕРОПРИЯТИЯМИ");
            Console.WriteLine("===========================================");
            Console.WriteLine($"Добро пожаловать в {EventManager.CompanyName}!");
            Console.WriteLine("===========================================");
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        static void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine($"===========================================");
            Console.WriteLine($"        {EventManager.CompanyName}");
            Console.WriteLine($"===========================================");
            Console.WriteLine($"Всего мероприятий: {Event.TotalEvents}");
            Console.WriteLine($"===========================================");
            Console.WriteLine("1. Добавить мероприятие");
            Console.WriteLine("2. Показать все мероприятия");
            Console.WriteLine("3. Показать мероприятия по типу");
            Console.WriteLine("4. Детальная информация о мероприятии");
            Console.WriteLine("5. Забронировать места");
            Console.WriteLine("6. Отменить бронирование");
            Console.WriteLine("7. Статистика");
            Console.WriteLine("8. Сохранить данные в файл");
            Console.WriteLine("9. Загрузить данные из файла");
            Console.WriteLine("10. Информация о системе");
            Console.WriteLine("11. Справка (HELP)");
            Console.WriteLine("12. Выход");
            Console.WriteLine("===========================================");
            Console.Write("Введите номер команды: ");
        }

        static void AddNewEvent()
        {
            Console.Clear();
            Console.WriteLine("===========================================");
            Console.WriteLine("         ДОБАВЛЕНИЕ МЕРОПРИЯТИЯ");
            Console.WriteLine("===========================================");
            Console.WriteLine("Выберите тип мероприятия:");
            Console.WriteLine("1. Конференция");
            Console.WriteLine("2. Концерт");
            Console.WriteLine("3. Воркшоп");
            Console.Write("Ваш выбор: ");

            string typeChoice = Console.ReadLine()?.Trim() ?? "";
            Event newEvent = null;

            try
            {
                switch (typeChoice)
                {
                    case "1": // Конференция
                        Console.WriteLine("\n--- КОНФЕРЕНЦИЯ ---");
                        Console.Write("ID: "); string confId = Console.ReadLine()?.Trim() ?? $"CONF{DateTime.Now.Ticks % 10000}";
                        Console.Write("Название: "); string confName = Console.ReadLine()?.Trim() ?? "";
                        Console.Write("Дата (гггг-мм-дд): "); DateTime confDate = DateTime.Parse(Console.ReadLine() ?? "2026-01-01");
                        Console.Write("Место: "); string confLocation = Console.ReadLine()?.Trim() ?? "";
                        Console.Write("Организатор: "); string confOrg = Console.ReadLine()?.Trim() ?? "";
                        Console.Write("Спикер: "); string speaker = Console.ReadLine()?.Trim() ?? "";
                        Console.Write("Тема: "); string topic = Console.ReadLine()?.Trim() ?? "";
                        Console.Write("Макс. участников: "); int confMax = int.Parse(Console.ReadLine() ?? "100");
                        Console.Write("Цена/чел (руб): "); decimal confPrice = decimal.Parse(Console.ReadLine() ?? "0");
                        Console.Write("Есть воркшопы? (да/нет): "); bool hasWorkshops = (Console.ReadLine()?.ToLower() == "да");

                        newEvent = new Conference(confId, confName, confDate, confLocation, confOrg,
                                                 speaker, topic, confMax, confPrice, hasWorkshops);
                        break;

                    case "2": // Концерт
                        Console.WriteLine("\n--- КОНЦЕРТ ---");
                        Console.Write("ID: "); string concertId = Console.ReadLine()?.Trim() ?? $"CONC{DateTime.Now.Ticks % 10000}";
                        Console.Write("Название: "); string concertName = Console.ReadLine()?.Trim() ?? "";
                        Console.Write("Дата (гггг-мм-дд): "); DateTime concertDate = DateTime.Parse(Console.ReadLine() ?? "2026-01-01");
                        Console.Write("Место: "); string concertLocation = Console.ReadLine()?.Trim() ?? "";
                        Console.Write("Организатор: "); string concertOrg = Console.ReadLine()?.Trim() ?? "";
                        Console.Write("Артист: "); string artist = Console.ReadLine()?.Trim() ?? "";
                        Console.Write("Жанр: "); string genre = Console.ReadLine()?.Trim() ?? "";
                        Console.Write("Вместимость: "); int capacity = int.Parse(Console.ReadLine() ?? "1000");
                        Console.Write("Цена билета (руб): "); decimal ticketPrice = decimal.Parse(Console.ReadLine() ?? "0");
                        Console.Write("На открытом воздухе? (да/нет): "); bool isOutdoor = (Console.ReadLine()?.ToLower() == "да");

                        newEvent = new Concert(concertId, concertName, concertDate, concertLocation, concertOrg,
                                              artist, genre, capacity, ticketPrice, isOutdoor);
                        break;

                    case "3": // Воркшоп
                        Console.WriteLine("\n--- ВОРКШОП ---");
                        Console.Write("ID: "); string workshopId = Console.ReadLine()?.Trim() ?? $"WORK{DateTime.Now.Ticks % 10000}";
                        Console.Write("Название: "); string workshopName = Console.ReadLine()?.Trim() ?? "";
                        Console.Write("Дата (гггг-мм-дд): "); DateTime workshopDate = DateTime.Parse(Console.ReadLine() ?? "2026-01-01");
                        Console.Write("Место: "); string workshopLocation = Console.ReadLine()?.Trim() ?? "";
                        Console.Write("Организатор: "); string workshopOrg = Console.ReadLine()?.Trim() ?? "";
                        Console.Write("Инструктор: "); string instructor = Console.ReadLine()?.Trim() ?? "";
                        Console.Write("Уровень (Начальный/Средний/Продвинутый): "); string skillLevel = Console.ReadLine()?.Trim() ?? "";
                        Console.Write("Макс. участников: "); int workshopMax = int.Parse(Console.ReadLine() ?? "20");
                        Console.Write("Стоимость (руб): "); decimal fee = decimal.Parse(Console.ReadLine() ?? "0");
                        Console.Write("Длительность (часов): "); int duration = int.Parse(Console.ReadLine() ?? "4");

                        newEvent = new Workshop(workshopId, workshopName, workshopDate, workshopLocation, workshopOrg,
                                               instructor, skillLevel, workshopMax, fee, duration);
                        break;

                    default:
                        Console.WriteLine("Неверный выбор типа мероприятия.");
                        return;
                }

                if (newEvent != null)
                {
                    Console.Write("Описание мероприятия: ");
                    string description = Console.ReadLine()?.Trim() ?? "";
                    if (!string.IsNullOrEmpty(description))
                        newEvent.UpdateDescription(description);

                    EventManager.AddEvent(newEvent);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nОшибка при создании мероприятия: {ex.Message}");
            }

            Console.WriteLine("===========================================");
        }

        static void ShowAllEvents()
        {
            Console.Clear();
            EventManager.DisplayAllEvents();
            Console.WriteLine("===========================================");
        }

        static void ShowEventsByType()
        {
            Console.Clear();
            Console.WriteLine("===========================================");
            Console.Write("Введите тип мероприятия (конференция/концерт/воркшоп): ");
            string type = Console.ReadLine()?.Trim() ?? "";
            EventManager.DisplayEventsByType(type);
            Console.WriteLine("===========================================");
        }

        static void ShowEventDetails()
        {
            Console.Clear();
            Console.WriteLine("===========================================");
            Console.Write("Введите ID мероприятия: ");
            string id = Console.ReadLine()?.Trim() ?? "";
            Console.WriteLine();
            EventManager.DisplayEventDetails(id);
            Console.WriteLine("===========================================");
        }

        static void BookEvent()
        {
            Console.Clear();
            Console.WriteLine("===========================================");
            Console.WriteLine("           БРОНИРОВАНИЕ МЕСТ");
            Console.WriteLine("===========================================");
            Console.Write("Введите ID мероприятия: ");
            string id = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Введите количество мест: ");

            if (int.TryParse(Console.ReadLine(), out int count) && count > 0)
            {
                EventManager.BookEvent(id, count);
            }
            else
            {
                Console.WriteLine("Ошибка: некорректное количество мест.");
            }
            Console.WriteLine("===========================================");
        }

        static void CancelBooking()
        {
            Console.Clear();
            Console.WriteLine("===========================================");
            Console.WriteLine("           ОТМЕНА БРОНИРОВАНИЯ");
            Console.WriteLine("===========================================");
            Console.Write("Введите ID мероприятия: ");
            string id = Console.ReadLine()?.Trim() ?? "";
            EventManager.CancelBookingEvent(id);
            Console.WriteLine("===========================================");
        }

        static void ShowSystemInfo()
        {
            Console.Clear();
            Console.WriteLine("===========================================");
            Console.WriteLine("         ИНФОРМАЦИЯ О СИСТЕМЕ");
            Console.WriteLine("===========================================");
            Console.WriteLine("\n=== ИЕРАРХИЯ КЛАССОВ ===");
            Console.WriteLine("Абстрактный класс: Event");
            Console.WriteLine("  Свойства: EventId, Name, Date, Location, Organizer, Description");
            Console.WriteLine("  Методы: GetEventType(), DisplayFullInfo(), IsPastEvent()");
            Console.WriteLine("  Статическое поле: TotalEvents");
            Console.WriteLine();
            Console.WriteLine("Интерфейсы:");
            Console.WriteLine("  IBookable: Book(), CancelBooking(), AvailableSpaces");
            Console.WriteLine("  IPayable: CalculatePrice(), PricePerPerson");
            Console.WriteLine();
            Console.WriteLine("Конкретные классы (наследуют Event, реализуют интерфейсы):");
            Console.WriteLine("  Conference : Event, IBookable, IPayable");
            Console.WriteLine("    Свойства: Speaker, Topic, MaxParticipants, BookedParticipants, PricePerPerson, HasWorkshops");
            Console.WriteLine();
            Console.WriteLine("  Concert : Event, IBookable, IPayable");
            Console.WriteLine("    Свойства: Artist, Genre, Capacity, TicketsSold, TicketPrice, IsOutdoor");
            Console.WriteLine();
            Console.WriteLine("  Workshop : Event, IBookable, IPayable");
            Console.WriteLine("    Свойства: Instructor, SkillLevel, MaxAttendees, RegisteredAttendees, WorkshopFee, DurationHours");
            Console.WriteLine();
            Console.WriteLine("Статический класс:");
            Console.WriteLine("  EventManager - управление всеми мероприятиями");
            Console.WriteLine("    Методы: AddEvent, RemoveEvent, DisplayAllEvents, BookEvent, CancelBookingEvent, ShowStatistics, SaveToFile, LoadFromFile");
            Console.WriteLine();
            Console.WriteLine("Принципы ООП:");
            Console.WriteLine("1. Наследование: Conference, Concert, Workshop наследуют от Event");
            Console.WriteLine("2. Полиморфизм: DisplayFullInfo() переопределен в каждом классе");
            Console.WriteLine("3. Инкапсуляция: свойства с private set, доступ через методы");
            Console.WriteLine("4. Абстракция: абстрактный класс Event и интерфейсы IBookable, IPayable");
            Console.WriteLine("===========================================");
        }

        static void ShowHelp()
        {
            Console.Clear();
            Console.WriteLine("===========================================");
            Console.WriteLine("               СПРАВКА (HELP)");
            Console.WriteLine("===========================================");
            Console.WriteLine("1. Добавить мероприятие - создание нового мероприятия");
            Console.WriteLine("2. Показать все мероприятия - список всех мероприятий");
            Console.WriteLine("3. Показать по типу - фильтрация по типу");
            Console.WriteLine("4. Детальная информация - полная информация по ID");
            Console.WriteLine("5. Забронировать места - бронирование мест на мероприятие");
            Console.WriteLine("6. Отменить бронирование - отмена всех бронирований");
            Console.WriteLine("7. Статистика - общая статистика по мероприятиям");
            Console.WriteLine("8. Сохранить данные - сохранение в текстовый файл");
            Console.WriteLine("9. Загрузить данные - просмотр данных из файла");
            Console.WriteLine("10. Информация о системе - описание иерархии классов");
            Console.WriteLine("11. Справка - это сообщение");
            Console.WriteLine("12. Выход - завершение работы");
            Console.WriteLine("===========================================");
            Console.WriteLine("Файлы данных: events_data.txt, event_log.txt");
            Console.WriteLine("===========================================");
        }
    }
}
