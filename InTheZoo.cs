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
        private List<Aviary> _aviaries;
        private List<string> _animalNames;
        private bool _zooIsWorking;

        public Zoo() 
        {
            _zooIsWorking = true;
            _aviaries = new List<Aviary>();
            _animalNames = new List<string>()
            {
                "Медведь",
                "Лиса",
                "Волк",
                "Змея"
            };
            SettleAnimals();
        }

        public void OperateZoo() 
        {
            while (_zooIsWorking)
            {
                Console.Clear();
                ShowAviaries();
                EnterAviary();
            }
        }

        private void ShowAviaries() 
        {
            for (int i = 0; i < _aviaries.Count; i++)
            {
                Console.WriteLine($"{i + 1} - Посетить вольер с {_aviaries[i].Name}");
            }
            Console.WriteLine($"{_aviaries.Count + 1} - Выйти из зоопарка");
        }

        private void EnterAviary() 
        {
            string userInput = Console.ReadLine();
            if (int.TryParse(userInput, out int index))
            {
                if (index > 0 && index <= _aviaries.Count)
                {
                    _aviaries[index-1].ShowInfo();
                }
                else if (index == _aviaries.Count + 1)
                {
                    _zooIsWorking = false;
                }
                else
                {
                    Console.WriteLine("Неизвестная команда");
                }
            }
            else
            {
                Console.WriteLine("Неизвестная команда");
            }
        }

        private void SettleAnimals() 
        {
            for (int i = 0; i < _animalNames.Count; i++)
            {
                _aviaries.Add(new Aviary(_animalNames[i]));
            }
        }
    }

    class Aviary 
    {
        private Random _random = new Random();
        private List<Animal> _animals;
        private int _maxAnimalsCount;
        private int _malesCount;
        private int _femalesCount;

        public string Name { get; private set; }

        public Aviary(string name) 
        {
            _animals = new List<Animal>();
            _maxAnimalsCount = _random.Next(2, 11);
            Name = name;
            FillAviary(name);
            CountMalesAndFemales();
            Thread.Sleep(1);
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
                if (key.Key == ConsoleKey.Escape)
                {
                    inAviary = false;
                }
            }
        }

        private void FillAviary(string name) 
        {
            for (int i = 0; i < _maxAnimalsCount; i++)
            {
                if (name == "Медведь")
                    _animals.Add(new Bear());
                if (name == "Лиса")
                    _animals.Add(new Fox());
                if (name == "Волк")
                    _animals.Add(new Wolf());
                if (name == "Змея")
                    _animals.Add(new Snake());
            }
        }

        private void CountMalesAndFemales() 
        {
            for (int i = 0; i < _animals.Count; i++)
            {
                if (_animals[i].Gender == Gender.Male)
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

    enum Gender 
    {
        Male,
        Female
    }

    class Animal 
    {
        protected Random Random = new Random();
        public string Name { get; protected set; }
        public Gender Gender { get; protected set; }
        public string Sound { get; protected set; }
    }

    class Bear : Animal 
    {
        public Bear() 
        {
            Name = "Медведь";
            Sound = "ррррр";
            Gender = (Gender)Random.Next(0, 2);
            Thread.Sleep(1);
        }
    }

    class Fox : Animal
    {
        public Fox()
        {
            Name = "Лиса";
            Sound = "фрфрфр";
            Gender = (Gender)Random.Next(0, 2);
            Thread.Sleep(1);
        }
    }

    class Wolf : Animal
    {
        public Wolf()
        {
            Name = "Волк";
            Sound = "ууууууу";
            Gender = (Gender)Random.Next(0, 2);
            Thread.Sleep(1);
        }
    }

    class Snake : Animal
    {
        public Snake()
        {
            Name = "Змея";
            Sound = "шшшшш";
            Gender = (Gender)Random.Next(0, 2);
            Thread.Sleep(1);
        }
    }
}
