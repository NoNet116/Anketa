using System;
using System.Linq;

namespace Anketa
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Заполните анкету. Для выхода наберите exit");
            Console.ForegroundColor = ConsoleColor.White;
            var userdata = EnterUser();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Завершено. Для продолжения нажмите клавишу.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey();
            Console.Clear();
            
            if(userdata.Name != default)
            {
                bool showinfo = true;

                while (showinfo)
                {
                   
                    log("Для получения информации о пользователе укажите цифру");
                    byte i = 1;
                    Console.WriteLine($"{i++} - Имя");
                    Console.WriteLine($"{i++} - Фамилия");
                    Console.WriteLine($"{i++} - Возраст");
                    Console.WriteLine($"{i++} - Питомцы");
                    Console.WriteLine($"{i++} - Любимые цвета");
                    Console.WriteLine("0 - Завершить работу приложения");
                    int cancel = 0;
                    int selectednum;
                    Console.Write("Ваш выбор: ");
                    GetNum(out selectednum, ref cancel);
                    Console.Clear();
                    if(cancel > 0)
                        break;
                    if (selectednum >= 0 && selectednum < 6)
                        Console.WriteLine($"Выбран ответ {selectednum}:");

                    switch (selectednum)
                    {
                        case 0:
                            showinfo = false;
                            break; 
                        case 1:
                            log(userdata.Name);
                            break;
                        case 2:
                            log(userdata.LastName);
                            break;
                        case 3:
                            log(userdata.Age.ToString());
                            break;
                        case 4:
                            log(userdata.Pets);
                            break;
                        case 5:
                            log(userdata.FavColors);
                            break;
                    }
                }
            }
            else
            {
                log("Нет сохраненных значений.");
            }
            log("Работа программы завершена.");

            Console.ReadLine();
        }
        
        static (string Name, string LastName, int Age, string[] Pets, string[] FavColors)  EnterUser()
        {
           
            int cancel = 0;
            var User = default((string Name, string LastName, int Age, string[] Pets, string[] FavColors));

            log("Введите имя");
            User.Name = SaveUserNameOrLastName(userData.Name,ref cancel);
            
            if (ShowSaveResult(ref cancel, User.Name))
            {
                log("Введите фамилию");
                User.LastName = SaveUserNameOrLastName(userData.LastName, ref cancel);
            }
           
            if (ShowSaveResult(ref cancel, User.LastName))
            {
                log("Введите возраст");
                User.Age = SaveAge(ref cancel);
            }

            
            if (ShowSaveResult(ref cancel,User.Age.ToString()))
            {
                log("У вас есть питомцы (да или нет)?");

                if (GetInput(ref cancel).ToLower() == "да")
                {
                    log("Укажите количество питомцев.");
                    int countPets;
                    GetNum(out countPets, ref cancel);
                    User.Pets = SavePetsOrFavColors(countPets,userData.Pets);

                }
                else
                    User.Pets = new string[] { "Нет" };
            }

           
            if (ShowSaveResult(ref cancel,User.Pets))
            {
                log("Количество любимых цветов?");
                int fcolorscount;
                GetNum(out fcolorscount, ref cancel);
                if (fcolorscount > 0)
                {
                    log(fcolorscount == 1 ? "Укажите любимый цвет?": "Укажите любимые цвета?");
                    User.FavColors = SavePetsOrFavColors(fcolorscount,userData.FavColors);
                }
                else
                {
                    User.FavColors = new string[] { "Нет" };
                }
            }

            ShowSaveResult(ref cancel, User.FavColors);
            if(cancel > 0)
                User = default((string Name, string LastName, int Age, string[] Pets, string[] FavColors));
            return  User;
        }
       
        static void log(params string[] str)
        {
            Console.WriteLine();
            Console.WriteLine(string.Join(Environment.NewLine,str));
        }

        private static bool ShowSaveResult(ref int cancel, params string[] str)
        {
            if (cancel > 0)
            {
                if(cancel <= 1)
                {
                    log("Процесс отменен");
                    cancel++;
                }
                return false;
            }
            log($"Сохранено: {Environment.NewLine + string.Join(Environment.NewLine, str)}");
            return true;
        }
                   
        private static int SaveAge(ref int cancel)
        {
            int age = 0;
            GetNum(out age, ref cancel);
            if (age < 7 || age > 100)
            {
                Console.Write("Сомнительно но okеу.");
            }
            return age;
        }

        static bool GetNum(out int intnumber,ref int cancel)
        {

            bool res = false;
            intnumber = 0;
            
            while (!res )
            {
                if (cancel > 0)
                    break;
                res = int.TryParse(GetInput(ref cancel), out int number);
                if (!res)
                {
                    log("Введено не корректное число.");
                    continue;
                }
                if (0 > number)
                {
                    log("Значение должно быть не меньше нуля.");
                    res = false;
                    continue;
                }
                intnumber = number;
                res = true;
            }
            return res;
        }

        enum userData :byte
        {
             Name,
             LastName,
             Pets = 0,
             FavColors
        }
        static string SaveUserNameOrLastName(userData userData, ref int cancel)
        {

            string[] ud = { "Имя не должно", "Фамилия не должна" };
            string name;

            while ((name = GetInput(ref cancel)).Length == 0)
            {
                if (cancel > 0)
                    break;
                log($"{ud[(byte)userData]} быть пустым");
            }
            return name;
        }

        private static string[] SavePetsOrFavColors(int count, userData userData)
        {
            int cancel = 0;
            var tmp = new string[count];
            string[] str = { $"Укажите имя питомца", $"Укажите любимый цвет" };
            for (int i = 0; i < tmp.Length; i++)
            {
                if(cancel > 0)
                    break;
                log(str[(byte)userData] + " " + (i + 1));
                tmp[i] = GetInput(ref cancel);
            }
            return tmp;
        }
        
        static string GetInput(ref int cancel)
        {

            var result = Console.ReadLine();
            string[] validInputs = { "quit", "exit", "close" };

            if (validInputs.Contains(result.ToLower()))
            {
                cancel++;
                return string.Empty;
            }
            else
            {
                return result;
            }

        }
    }
}
