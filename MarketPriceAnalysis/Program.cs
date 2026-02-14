/* Тамекло, Коку Эдмон */
/* Программа 1: Анализ динамики рыночных цен */

using System;
using System.Collections.Generic;
using System.IO;

namespace MarketPriceAnalysis
{
    class Program
    {
        static List<double> priceHistory = new List<double>();
        static string dataFile = "price_data.txt";

        static void Main(string[] args)
        {
            ShowWelcomeMessage();
            LoadDataFromFile();

            bool running = true;
            while (running)
            {
                ShowMenu();
                string command = Console.ReadLine()?.Trim() ?? "";

                switch (command)
                {
                    case "1": AddPrice(); break;
                    case "2": ShowAllPrices(); break;
                    case "3": CalculateTrends(); break;
                    case "4": SaveData(); break;
                    case "5": LoadDataFromFile(); break;
                    case "6": ShowHelp(); break;
                    case "7": running = false; break;
                    default: Console.WriteLine("Неизвестная команда."); break;
                }

                if (running)
                {
                    Console.WriteLine("\nНажмите любую клавишу...");
                    Console.ReadKey();
                }
            }
        }

        static void ShowWelcomeMessage()
        {
            Console.Clear();
            Console.WriteLine("===========================================");
            Console.WriteLine("     АНАЛИЗ ДИНАМИКИ РЫНОЧНЫХ ЦЕН");
            Console.WriteLine("===========================================");
            Console.WriteLine("Добро пожаловать в программу анализа цен!");
            Console.WriteLine("===========================================");
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        static void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("===========================================");
            Console.WriteLine("                ГЛАВНОЕ МЕНЮ");
            Console.WriteLine("===========================================");
            Console.WriteLine("1. Добавить цену");
            Console.WriteLine("2. Показать историю цен");
            Console.WriteLine("3. Рассчитать тренды");
            Console.WriteLine("4. Сохранить данные в файл");
            Console.WriteLine("5. Загрузить данные из файла");
            Console.WriteLine("6. Справка (HELP)");
            Console.WriteLine("7. Выход");
            Console.WriteLine("===========================================");
            Console.Write("Введите номер команды: ");
        }

        static void AddPrice()
        {
            Console.Clear();
            Console.WriteLine("===========================================");
            Console.WriteLine("             ДОБАВЛЕНИЕ ЦЕНЫ");
            Console.WriteLine("===========================================");
            Console.Write("Введите цену: ");

            if (double.TryParse(Console.ReadLine(), out double price) && price > 0)
            {
                priceHistory.Add(price);
                Console.WriteLine($"\nЦена {price} руб успешно добавлена в историю."); ;
                Console.WriteLine($"Всего записей в истории: {priceHistory.Count}");
            }
            else
            {
                Console.WriteLine("\nОшибка: введено некорректное значение цены.");
            }
            Console.WriteLine("===========================================");
        }

        static void ShowAllPrices()
        {
            Console.Clear();
            Console.WriteLine("===========================================");
            Console.WriteLine("             ИСТОРИЯ ЦЕН");
            Console.WriteLine("===========================================");

            if (priceHistory.Count == 0)
            {
                Console.WriteLine("История цен пуста.");
            }
            else
            {
                for (int i = 0; i < priceHistory.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {priceHistory[i]} руб");
                }
                Console.WriteLine($"\nВсего записей: {priceHistory.Count}");
            }
            Console.WriteLine("===========================================");
        }

        static void CalculateTrends()
        {
            Console.Clear();
            Console.WriteLine("===========================================");
            Console.WriteLine("             АНАЛИЗ ТРЕНДОВ");
            Console.WriteLine("===========================================");

            if (priceHistory.Count < 2)
            {
                Console.WriteLine("Недостаточно данных для анализа (нужно минимум 2 цены).");
                Console.WriteLine($"Текущее количество записей: {priceHistory.Count}");
            }
            else
            {
                double sumChanges = 0;

                for (int i = 1; i < priceHistory.Count; i++)
                {
                    double change = priceHistory[i] - priceHistory[i - 1];
                    sumChanges += change;
                }

                double avgChange = sumChanges / (priceHistory.Count - 1);

                Console.WriteLine($"Среднее изменение цены: {avgChange} руб");
                Console.Write("Общий тренд: ");

                if (avgChange > 0)
                    Console.WriteLine("РОСТ цен");
                else if (avgChange < 0)
                    Console.WriteLine("СНИЖЕНИЕ цен");
                else
                    Console.WriteLine("СТАБИЛЬНОСТЬ цен");
            }
            Console.WriteLine("===========================================");
        }

        static void SaveData()
        {
            Console.Clear();
            Console.WriteLine("===========================================");
            Console.WriteLine("          СОХРАНЕНИЕ ДАННЫХ");
            Console.WriteLine("===========================================");

            try
            {
                using (StreamWriter writer = new StreamWriter(dataFile))
                {
                    foreach (double price in priceHistory)
                    {
                        writer.WriteLine(price.ToString());
                    }
                }
                Console.WriteLine($"Данные успешно сохранены в файл: {dataFile}");
                Console.WriteLine($"Сохранено записей: {priceHistory.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении файла: {ex.Message}");
            }
            Console.WriteLine("===========================================");
        }

        static void LoadDataFromFile()
        {
            if (!File.Exists(dataFile))
            {
                Console.WriteLine("Файл данных не найден. Будет создан новый при сохранении.");
                return;
            }

            try
            {
                using (StreamReader reader = new StreamReader(dataFile))
                {
                    priceHistory.Clear();
                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (double.TryParse(line, out double price))
                        {
                            priceHistory.Add(price);
                        }
                    }
                }
                Console.WriteLine($"Данные успешно загружены из файла: {dataFile}");
                Console.WriteLine($"Загружено записей: {priceHistory.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке файла: {ex.Message}");
            }
        }

        static void ShowHelp()
        {
            Console.Clear();
            Console.WriteLine("===========================================");
            Console.WriteLine("               СПРАВКА (HELP)");
            Console.WriteLine("===========================================");
            Console.WriteLine("1. Добавить цену - добавление новой цены в историю");
            Console.WriteLine("2. Показать историю - отображение всех сохраненных цен");
            Console.WriteLine("3. Рассчитать тренды - анализ изменений цен");
            Console.WriteLine("4. Сохранить данные - сохранение всех цен в текстовый файл");
            Console.WriteLine("5. Загрузить данные - загрузка цен из текстового файла");
            Console.WriteLine("6. Справка - отображение этого сообщения");
            Console.WriteLine("7. Выход - завершение работы программы");
            Console.WriteLine("===========================================");
            Console.WriteLine($"Файл данных: {dataFile}");
            Console.WriteLine("===========================================");
        }
    }
}
