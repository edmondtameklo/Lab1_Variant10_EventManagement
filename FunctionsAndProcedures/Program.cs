/* Тамекло, Коку Эдмон */
/* Часть 2: Функции и процедуры (Вариант 10) */

using System;
using System.IO;

namespace FunctionsAndProcedures
{
    class Program
    {
        static string logFile = "math_log.txt";

        static void Main(string[] args)
        {
            ShowWelcomeMessage();

            bool running = true;
            while (running)
            {
                ShowMenu();
                string command = Console.ReadLine()?.Trim() ?? "";

                switch (command)
                {
                    case "1": CalculateFactorial(); break;
                    case "2": CheckArmstrong(); break;
                    case "3": SumDiagonal(); break;
                    case "4": PrintNumberPyramid(); break;
                    case "5": PrintStarCircle(); break;
                    case "6": ShowHistory(); break;
                    case "7": ClearHistory(); break;
                    case "8": ShowHelp(); break;
                    case "9": running = false; break;
                    default: Console.WriteLine("Неизвестная команда."); break;
                }

                if (running)
                {
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }
        }

        static void ShowWelcomeMessage()
        {
            Console.Clear();
            Console.WriteLine("===========================================");
            Console.WriteLine("     ФУНКЦИИ И ПРОЦЕДУРЫ (Вариант 10)");
            Console.WriteLine("===========================================");
            Console.WriteLine("Добро пожаловать в программу математических вычислений!");
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
            Console.WriteLine("  ФУНКЦИИ (возвращают результат):");
            Console.WriteLine("1. Факториал числа (рекурсия)");
            Console.WriteLine("2. Проверка числа Армстронга");
            Console.WriteLine("3. Сумма главной диагонали матрицы");
            Console.WriteLine();
            Console.WriteLine("  ПРОЦЕДУРЫ (выполняют действия):");
            Console.WriteLine("4. Числовая пирамида");
            Console.WriteLine("5. Круг из звездочек");
            Console.WriteLine();
            Console.WriteLine("  ДОПОЛНИТЕЛЬНО:");
            Console.WriteLine("6. История операций");
            Console.WriteLine("7. Очистить историю");
            Console.WriteLine("8. Справка (HELP)");
            Console.WriteLine("9. Выход");
            Console.WriteLine("===========================================");
            Console.Write("Введите номер команды: ");
        }

        // ---------- ФУНКЦИИ ----------

        // Функция 1: Рекурсивный факториал
        static long Factorial(int n)
        {
            if (n < 0)
                throw new ArgumentException("Факториал отрицательного числа не определен");
            if (n <= 1)
                return 1;
            return n * Factorial(n - 1);
        }

        static void CalculateFactorial()
        {
            Console.Clear();
            Console.WriteLine("===========================================");
            Console.WriteLine("          ВЫЧИСЛЕНИЕ ФАКТОРИАЛА");
            Console.WriteLine("===========================================");
            Console.Write("Введите целое неотрицательное число: ");

            try
            {
                if (int.TryParse(Console.ReadLine(), out int n))
                {
                    long result = Factorial(n);
                    Console.WriteLine($"\n{n}! = {result}");
                    Console.WriteLine($"Рекурсивное вычисление: {n}! = {n} * {n - 1}! ...");
                    LogToFile($"Факториал: {n}! = {result}");
                }
                else
                {
                    Console.WriteLine("\nОшибка: введено некорректное число.");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"\nОшибка: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nОшибка: {ex.Message}");
            }
            Console.WriteLine("===========================================");
        }

        // Функция 2: Проверка числа Армстронга
        static bool IsArmstrong(int number)
        {
            if (number < 0) return false;

            string digits = number.ToString();
            int power = digits.Length;
            int sum = 0;
            int temp = number;

            while (temp > 0)
            {
                int digit = temp % 10;
                sum += (int)Math.Pow(digit, power);
                temp /= 10;
            }

            return sum == number;
        }

        static void CheckArmstrong()
        {
            Console.Clear();
            Console.WriteLine("===========================================");
            Console.WriteLine("         ПРОВЕРКА ЧИСЛА АРМСТРОНГА");
            Console.WriteLine("===========================================");
            Console.Write("Введите целое число: ");

            if (int.TryParse(Console.ReadLine(), out int number))
            {
                Console.WriteLine($"\nЧисло: {number}");

                if (IsArmstrong(number))
                {
                    Console.WriteLine($"Результат: {number} является числом Армстронга");

                    // Показываем разложение
                    string digits = number.ToString();
                    Console.Write($"{number} = ");
                    for (int i = 0; i < digits.Length; i++)
                    {
                        int digit = int.Parse(digits[i].ToString());
                        Console.Write($"{digit}^{digits.Length}");
                        if (i < digits.Length - 1)
                            Console.Write(" + ");
                    }
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine($"Результат: {number} НЕ является числом Армстронга");
                }

                LogToFile($"Проверка Армстронга: {number} = {(IsArmstrong(number) ? "да" : "нет")}");
            }
            else
            {
                Console.WriteLine("\nОшибка: введено некорректное число.");
            }
            Console.WriteLine("===========================================");
        }

        // Функция 3: Сумма главной диагонали матрицы
        static int SumMainDiagonal(int[,] matrix)
        {
            int sum = 0;
            int size = matrix.GetLength(0);

            for (int i = 0; i < size; i++)
            {
                sum += matrix[i, i];
            }

            return sum;
        }

        static void SumDiagonal()
        {
            Console.Clear();
            Console.WriteLine("===========================================");
            Console.WriteLine("      СУММА ГЛАВНОЙ ДИАГОНАЛИ МАТРИЦЫ");
            Console.WriteLine("===========================================");
            Console.Write("Введите размер квадратной матрицы: ");

            if (int.TryParse(Console.ReadLine(), out int size) && size > 0 && size <= 10)
            {
                int[,] matrix = new int[size, size];
                Random rand = new Random();

                Console.WriteLine("\nСгенерированная матрица:");
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        matrix[i, j] = rand.Next(1, 10);
                        Console.Write($"{matrix[i, j],4}");
                    }
                    Console.WriteLine();
                }

                Console.WriteLine("\nГлавная диагональ: ");
                for (int i = 0; i < size; i++)
                {
                    Console.Write($"{matrix[i, i]} ");
                }

                int sum = SumMainDiagonal(matrix);
                Console.WriteLine($"\n\nСумма элементов главной диагонали: {sum}");

                LogToFile($"Сумма диагонали матрицы {size}x{size} = {sum}");
            }
            else
            {
                Console.WriteLine("\nОшибка: введен некорректный размер (от 1 до 10).");
            }
            Console.WriteLine("===========================================");
        }

        // ---------- ПРОЦЕДУРЫ ----------

        // Процедура 1: Числовая пирамида
        static void PrintNumberPyramid(int height)
        {
            for (int i = 1; i <= height; i++)
            {
                // Пробелы для центрирования
                for (int j = 0; j < height - i; j++)
                    Console.Write(" ");

                // Возрастающая часть
                for (int j = 1; j <= i; j++)
                    Console.Write(j);

                // Убывающая часть
                for (int j = i - 1; j >= 1; j--)
                    Console.Write(j);

                Console.WriteLine();
            }
        }

        static void PrintNumberPyramid()
        {
            Console.Clear();
            Console.WriteLine("===========================================");
            Console.WriteLine("             ЧИСЛОВАЯ ПИРАМИДА");
            Console.WriteLine("===========================================");
            Console.Write("Введите высоту пирамиды: ");

            if (int.TryParse(Console.ReadLine(), out int height) && height > 0 && height <= 20)
            {
                Console.WriteLine();
                PrintNumberPyramid(height);
                LogToFile($"Числовая пирамида высотой {height}");
            }
            else
            {
                Console.WriteLine("\nОшибка: введите положительное число (макс. 20).");
            }
            Console.WriteLine("===========================================");
        }

        // Процедура 2: Круг из звездочек
        static void PrintStarCircle(int radius)
        {
            for (int y = -radius; y <= radius; y++)
            {
                for (int x = -radius; x <= radius; x++)
                {
                    double distance = Math.Sqrt(x * x + y * y);
                    // Рисуем круг толщиной в 1 символ
                    if (Math.Abs(distance - radius) < 0.5)
                        Console.Write("*");
                    else
                        Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

        static void PrintStarCircle()
        {
            Console.Clear();
            Console.WriteLine("===========================================");
            Console.WriteLine("            КРУГ ИЗ ЗВЕЗДОЧЕК");
            Console.WriteLine("===========================================");
            Console.Write("Введите радиус круга (целое число): ");

            if (int.TryParse(Console.ReadLine(), out int radius) && radius > 0 && radius <= 20)
            {
                Console.WriteLine();
                PrintStarCircle(radius);
                LogToFile($"Круг радиусом {radius}");
            }
            else
            {
                Console.WriteLine("\nОшибка: введите положительное число (макс. 20).");
            }
            Console.WriteLine("===========================================");
        }

        // ---------- РАБОТА С ФАЙЛАМИ ----------

        static void ShowHistory()
        {
            Console.Clear();
            Console.WriteLine("===========================================");
            Console.WriteLine("            ИСТОРИЯ ОПЕРАЦИЙ");
            Console.WriteLine("===========================================");

            if (!File.Exists(logFile))
            {
                Console.WriteLine("История операций пуста.");
                Console.WriteLine($"Файл {logFile} не найден.");
            }
            else
            {
                try
                {
                    string[] lines = File.ReadAllLines(logFile);
                    if (lines.Length == 0)
                    {
                        Console.WriteLine("История операций пуста.");
                    }
                    else
                    {
                        Console.WriteLine($"Всего записей: {lines.Length}");
                        Console.WriteLine("-------------------------------------------");
                        foreach (string line in lines)
                        {
                            Console.WriteLine(line);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
                }
            }
            Console.WriteLine("===========================================");
        }

        static void ClearHistory()
        {
            Console.Clear();
            Console.WriteLine("===========================================");
            Console.WriteLine("           ОЧИСТКА ИСТОРИИ");
            Console.WriteLine("===========================================");

            if (File.Exists(logFile))
            {
                try
                {
                    File.Delete(logFile);
                    Console.WriteLine("История операций успешно очищена.");
                    Console.WriteLine($"Файл {logFile} удален.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при удалении файла: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Файл истории не существует.");
            }
            Console.WriteLine("===========================================");
        }

        static void LogToFile(string message)
        {
            try
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string logEntry = $"[{timestamp}] {message}";
                using (StreamWriter writer = File.AppendText(logFile))
                {
                    writer.WriteLine(logEntry);
                }
            }
            catch
            {
                // Игнорируем ошибки записи в лог
            }
        }

        static void ShowHelp()
        {
            Console.Clear();
            Console.WriteLine("===========================================");
            Console.WriteLine("               СПРАВКА (HELP)");
            Console.WriteLine("===========================================");
            Console.WriteLine("ФУНКЦИИ:");
            Console.WriteLine("1. Факториал - вычисление факториала рекурсивно");
            Console.WriteLine("   Формула: n! = n * (n-1) * ... * 1");
            Console.WriteLine("   Пример: 5! = 5 * 4 * 3 * 2 * 1 = 120");
            Console.WriteLine();
            Console.WriteLine("2. Число Армстронга - число, равное сумме своих цифр,");
            Console.WriteLine("   возведенных в степень, равную количеству цифр");
            Console.WriteLine("   Пример: 153 = 1^3 + 5^3 + 3^3");
            Console.WriteLine();
            Console.WriteLine("3. Сумма диагонали - сумма элементов на главной диагонали");
            Console.WriteLine("   квадратной матрицы (элементы a[i,i])");
            Console.WriteLine();
            Console.WriteLine("ПРОЦЕДУРЫ:");
            Console.WriteLine("4. Числовая пирамида - вывод пирамиды из чисел");
            Console.WriteLine("   Пример для высоты 3:");
            Console.WriteLine("    1");
            Console.WriteLine("   121");
            Console.WriteLine("  12321");
            Console.WriteLine();
            Console.WriteLine("5. Круг из звездочек - рисование круга символами '*'");
            Console.WriteLine();
            Console.WriteLine("ДОПОЛНИТЕЛЬНО:");
            Console.WriteLine("6. История - просмотр всех выполненных операций");
            Console.WriteLine("7. Очистить историю - удаление файла с историей");
            Console.WriteLine("8. Справка - это сообщение");
            Console.WriteLine("9. Выход - завершение работы");
            Console.WriteLine("===========================================");
            Console.WriteLine($"Файл истории: {logFile}");
            Console.WriteLine("===========================================");
        }
    }
}
