using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VisitZoo
{
    class InTheZoo
    {
        static void Main(string[] args)
        {
            Zoo centralZoo = new Zoo();
            centralZoo.OperateZoo();
        }
    }

    class Zoo
    {
        private List<string> _animalNames = new List<string>()
        {
             "медведь" ,  
             "волк" ,  
             "лиса" ,  
             "змея" , 
        };

        private List<string> _soundOFAnimals = new List<string>()
        {
             "ррррр",
             "уууууу",
             "фрфрфр",
             "шшшшшш",
        };

        private List<Aviary> _aviaries = new List<Aviary>();

        public Zoo() 
        {
            SettleAnimals();
        }
        public void OperateZoo() 
        {
            bool isWorking = true;
            while (isWorking)
            {
                Console.Clear();
                Console.WriteLine($"1 - Посетить вольер с {_aviaries[0].Name}");
                Console.WriteLine($"2 - Посетить вольер с {_aviaries[1].Name}");
                Console.WriteLine($"3 - Посетить вольер с {_aviaries[2].Name}");
                Console.WriteLine($"4 - Посетить вольер с {_aviaries[3].Name}");
                Console.WriteLine("5 - Выйти из зоопарка");
                string userInput = Console.ReadLine();
                switch (userInput)
                {
                    case "1":
                        _aviaries[0].ShowInfo();
                        break;
                    case "2":
                        _aviaries[1].ShowInfo();
                        break;
                    case "3":
                        _aviaries[2].ShowInfo();
                        break;
                    case "4":
                        _aviaries[3].ShowInfo();
                        break;
                    case "5":
                        isWorking = false;
                        break;
                    default:
                        break;
                }
            }
        }

        private void SettleAnimals() 
        {
            for (int i = 0; i < _animalNames.Count; i++)
            {
                _aviaries.Add(new Aviary(_animalNames[i], _soundOFAnimals[i]));
            }
        }
    }

    class Aviary 
    {
        private Random _random = new Random();
        public string Name { get; private set; }
        private int _maxAnimalsCount;
        private int _malesCount;
        private int _femalesCount;
        private List<Animal> _animals = new List<Animal>();

        public Aviary(string name, string sound) 
        {
            Name = name;
            _maxAnimalsCount = _random.Next(2, 11);
            FillAviary(sound);
            CountMalesAndFemales();
        }

        public void ShowInfo() 
        {
            bool inAviary = true;
            while (inAviary)
            {
                Console.Clear();
                Console.WriteLine($"Вы в вольере с {Name}");
                Console.WriteLine($"Всего {Name} в вольере: {_maxAnimalsCount}");
                Console.WriteLine($"Из них самцов: {_malesCount}. Самок: {_femalesCount}");
                Console.WriteLine($"Вы слышите звук: {_animals[0].Sound}");
                Console.WriteLine("Для выхода из вольера нажмите ESC");

                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.Escape:
                        inAviary = false;
                        break;
                    default:
                        break;
                }
            }
        }

        private void FillAviary(string sound) 
        {
            for (int i = 0; i < _maxAnimalsCount; i++)
            {
                _animals.Add(new Animal(Name, sound));
            }
            
        }

        private void CountMalesAndFemales() 
        {
            for (int i = 0; i < _animals.Count; i++)
            {
                if (_animals[i].Gender == "самец")
                {
                    _malesCount++;
                }
                else
                {
                    _femalesCount++;
                }
            }
        }
    }

    class Animal 
    {
        private Random _random = new Random();
        public string Name { get; private set; }
        public string Gender { get; private set; }
        public string Sound { get; private set; }

        public Animal(string name, string sound) 
        {
            Name = name;
            Sound = sound;
            SetGender();
            Thread.Sleep(1);
        }

        private void SetGender() 
        {
            switch (_random.Next(1,3))
            {
                case 1:
                    Gender = "самка";
                    break;
                case 2:
                    Gender = "самец";
                    break;
            }
        }
    }
}
