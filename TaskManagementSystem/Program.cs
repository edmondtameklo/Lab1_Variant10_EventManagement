/* Тамекло, Коку Эдмон */
/* Программа 2: Управление задачами */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TaskManagementSystem
{
    class Program
    {
        static List<string> tasks = new List<string>();
        static Dictionary<string, int> taskPriorities = new Dictionary<string, int>();
        static string dataFile = "tasks_data.txt";

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
                    case "1": AddTask(); break;
                    case "2": SetPriority(); break;
                    case "3": ShowAllTasks(); break;
                    case "4": SortTasksByPriority(); break;
                    case "5": SaveData(); break;
                    case "6": LoadDataFromFile(); break;
                    case "7": ShowHelp(); break;
                    case "8": running = false; break;
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
            Console.WriteLine("           УПРАВЛЕНИЕ ЗАДАЧАМИ");
            Console.WriteLine("===========================================");
            Console.WriteLine("Добро пожаловать в систему управления задачами!");
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
            Console.WriteLine("1. Добавить задачу");
            Console.WriteLine("2. Установить приоритет");
            Console.WriteLine("3. Показать все задачи");
            Console.WriteLine("4. Сортировать по приоритету");
            Console.WriteLine("5. Сохранить данные в файл");
            Console.WriteLine("6. Загрузить данные из файла");
            Console.WriteLine("7. Справка (HELP)");
            Console.WriteLine("8. Выход");
            Console.WriteLine("===========================================");
            Console.Write("Введите номер команды: ");
        }

        static void AddTask()
        {
            Console.Clear();
            Console.WriteLine("===========================================");
            Console.WriteLine("             ДОБАВЛЕНИЕ ЗАДАЧИ");
            Console.WriteLine("===========================================");
            Console.Write("Введите название задачи: ");

            string task = Console.ReadLine()?.Trim() ?? "";

            if (!string.IsNullOrWhiteSpace(task))
            {
                if (!tasks.Contains(task))
                {
                    tasks.Add(task);
                    taskPriorities[task] = 0; // Приоритет по умолчанию (0 - не установлен)
                    Console.WriteLine($"\nЗадача '{task}' успешно добавлена.");
                    Console.WriteLine($"Всего задач: {tasks.Count}");
                }
                else
                {
                    Console.WriteLine($"\nОшибка: задача '{task}' уже существует.");
                }
            }
            else
            {
                Console.WriteLine("\nОшибка: название задачи не может быть пустым.");
            }
            Console.WriteLine("===========================================");
        }

        static void SetPriority()
        {
            Console.Clear();
            Console.WriteLine("===========================================");
            Console.WriteLine("           УСТАНОВКА ПРИОРИТЕТА");
            Console.WriteLine("===========================================");

            if (tasks.Count == 0)
            {
                Console.WriteLine("Нет задач для установки приоритета.");
                Console.WriteLine("===========================================");
                return;
            }

            Console.WriteLine("Выберите задачу:");
            for (int i = 0; i < tasks.Count; i++)
            {
                string priorityText = GetPriorityText(taskPriorities[tasks[i]]);
                Console.WriteLine($"{i + 1}. {tasks[i]} - {priorityText}");
            }

            Console.Write("\nВведите номер задачи: ");
            if (int.TryParse(Console.ReadLine(), out int taskIndex) && taskIndex > 0 && taskIndex <= tasks.Count)
            {
                Console.WriteLine("\nВыберите приоритет:");
                Console.WriteLine("1. Высокий");
                Console.WriteLine("2. Средний");
                Console.WriteLine("3. Низкий");
                Console.Write("Ваш выбор: ");

                if (int.TryParse(Console.ReadLine(), out int priorityChoice) && priorityChoice >= 1 && priorityChoice <= 3)
                {
                    int priority = 0;
                    string priorityName = "";

                    switch (priorityChoice)
                    {
                        case 1: priority = 1; priorityName = "Высокий"; break;
                        case 2: priority = 2; priorityName = "Средний"; break;
                        case 3: priority = 3; priorityName = "Низкий"; break;
                    }

                    string selectedTask = tasks[taskIndex - 1];
                    taskPriorities[selectedTask] = priority;
                    Console.WriteLine($"\nПриоритет задачи '{selectedTask}' установлен на '{priorityName}'.");
                }
                else
                {
                    Console.WriteLine("\nОшибка: неверный выбор приоритета.");
                }
            }
            else
            {
                Console.WriteLine("\nОшибка: неверный номер задачи.");
            }
            Console.WriteLine("===========================================");
        }

        static void ShowAllTasks()
        {
            Console.Clear();
            Console.WriteLine("===========================================");
            Console.WriteLine("             ВСЕ ЗАДАЧИ");
            Console.WriteLine("===========================================");

            if (tasks.Count == 0)
            {
                Console.WriteLine("Список задач пуст.");
            }
            else
            {
                for (int i = 0; i < tasks.Count; i++)
                {
                    string priorityText = GetPriorityText(taskPriorities[tasks[i]]);
                    Console.WriteLine($"{i + 1}. {tasks[i]} - {priorityText}");
                }
                Console.WriteLine($"\nВсего задач: {tasks.Count}");
            }
            Console.WriteLine("===========================================");
        }

        static void SortTasksByPriority()
        {
            Console.Clear();
            Console.WriteLine("===========================================");
            Console.WriteLine("        ЗАДАЧИ ПО ПРИОРИТЕТУ");
            Console.WriteLine("===========================================");

            if (tasks.Count == 0)
            {
                Console.WriteLine("Список задач пуст.");
                Console.WriteLine("===========================================");
                return;
            }

            // Сортировка: сначала высокий (1), потом средний (2), потом низкий (3), потом без приоритета (0)
            var sortedTasks = tasks.OrderBy(t => taskPriorities[t] == 0 ? 4 : taskPriorities[t]).ToList();

            Console.WriteLine("Высокий приоритет:");
            bool hasHigh = false;
            foreach (var task in sortedTasks.Where(t => taskPriorities[t] == 1))
            {
                Console.WriteLine($"  - {task}");
                hasHigh = true;
            }
            if (!hasHigh) Console.WriteLine("  (нет задач)");

            Console.WriteLine("\nСредний приоритет:");
            bool hasMedium = false;
            foreach (var task in sortedTasks.Where(t => taskPriorities[t] == 2))
            {
                Console.WriteLine($"  - {task}");
                hasMedium = true;
            }
            if (!hasMedium) Console.WriteLine("  (нет задач)");

            Console.WriteLine("\nНизкий приоритет:");
            bool hasLow = false;
            foreach (var task in sortedTasks.Where(t => taskPriorities[t] == 3))
            {
                Console.WriteLine($"  - {task}");
                hasLow = true;
            }
            if (!hasLow) Console.WriteLine("  (нет задач)");

            Console.WriteLine("\nБез приоритета:");
            bool hasNoPriority = false;
            foreach (var task in sortedTasks.Where(t => taskPriorities[t] == 0))
            {
                Console.WriteLine($"  - {task}");
                hasNoPriority = true;
            }
            if (!hasNoPriority) Console.WriteLine("  (нет задач)");

            Console.WriteLine("===========================================");
        }

        static string GetPriorityText(int priority)
        {
            switch (priority)
            {
                case 1: return "Высокий";
                case 2: return "Средний";
                case 3: return "Низкий";
                default: return "Не установлен";
            }
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
                    writer.WriteLine($"Дата сохранения: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
                    writer.WriteLine($"Количество задач: {tasks.Count}");
                    writer.WriteLine("---");

                    foreach (string task in tasks)
                    {
                        writer.WriteLine($"{task}|{taskPriorities[task]}");
                    }
                }
                Console.WriteLine($"Данные успешно сохранены в файл: {dataFile}");
                Console.WriteLine($"Сохранено задач: {tasks.Count}");
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
                    tasks.Clear();
                    taskPriorities.Clear();
                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        // Пропускаем строки с метаданными (дата, количество, разделители)
                        if (line.Contains("|"))
                        {
                            string[] parts = line.Split('|');
                            if (parts.Length == 2)
                            {
                                string task = parts[0];
                                if (int.TryParse(parts[1], out int priority))
                                {
                                    tasks.Add(task);
                                    taskPriorities[task] = priority;
                                }
                            }
                        }
                    }
                }
                Console.WriteLine($"Данные успешно загружены из файла: {dataFile}");
                Console.WriteLine($"Загружено задач: {tasks.Count}");
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
            Console.WriteLine("1. Добавить задачу - создание новой задачи");
            Console.WriteLine("2. Установить приоритет - назначение приоритета задаче");
            Console.WriteLine("3. Показать все задачи - отображение всех задач");
            Console.WriteLine("4. Сортировать по приоритету - группировка задач по приоритету");
            Console.WriteLine("5. Сохранить данные - сохранение всех задач в файл");
            Console.WriteLine("6. Загрузить данные - загрузка задач из файла");
            Console.WriteLine("7. Справка - отображение этого сообщения");
            Console.WriteLine("8. Выход - завершение работы программы");
            Console.WriteLine("===========================================");
            Console.WriteLine($"Файл данных: {dataFile}");
            Console.WriteLine("===========================================");
            Console.WriteLine("\nПриоритеты:");
            Console.WriteLine("  1 - Высокий");
            Console.WriteLine("  2 - Средний");
            Console.WriteLine("  3 - Низкий");
            Console.WriteLine("  0 - Не установлен");
            Console.WriteLine("===========================================");
        }
    }
}
