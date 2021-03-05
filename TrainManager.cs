using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectionConfigurator
{
    class TrainManager
    {
        static void Main(string[] args)
        {
            TrainConfigurator configurator = new TrainConfigurator();

            bool isWorking = true;
            while (isWorking)
            {
                Console.Clear();
                configurator.ShowInfo();
                Console.WriteLine("\n1 - Создать направление");
                Console.WriteLine("2 - Продать билеты");
                Console.WriteLine("3 - Сформировать поезд");
                Console.WriteLine("4 - Отправить поезд");
                Console.WriteLine("5 - Выход из конфигуратора\n");


                string userInput = Console.ReadLine();
                switch (userInput)
                {
                    case "1":
                        configurator.CreateDirection();
                        break;
                    case "2":
                        configurator.SellTickets();
                        break;
                    case "3":
                        configurator.CreateTrain();
                        break;
                    case "4":
                        configurator.SendTrain();
                        break;
                    case "5":
                        isWorking = false;
                        break;
                    default:
                        Console.WriteLine("Неизвестное действие");
                        break;
                }
            }

        }
    }

    class Carriage 
    {
        public int SeatsCount { get; private set; }

        public Carriage(int seatsCount) 
        {
            SeatsCount = seatsCount;
        }
    }

    class TrainConfigurator
    {
        private List<Carriage> _carriages = new List<Carriage>
        {
            new Carriage(10),
            new Carriage(15),
            new Carriage(5),
            new Carriage(7),
            new Carriage(20),
            new Carriage(14),
            new Carriage(30),
            new Carriage(42),
        };

        public List<string> Cities 
        {
            get 
            {
                return Cities;
            }

            private set 
            {
                Cities = new List<string>
                {
                "Москва",
                "Санкт-Петербург",
                "Владивосток",
                "Псков",
                "Волгоград",
                "Тверь",
                "Барнаул"
                };
            } 
        }

        private string _startCity;
        private string _endCity;
        private bool _directionCreated;
        private int _passengerCount;
        private bool _ticketsAreSold;
        private bool _trainCreated;
        private int _trainSeatsCount;
        private bool _allReady;

        public void ShowCities() 
        {
            for (int i = 0; i < Cities.Count; i++)
            {
                Console.WriteLine(i + " - " + Cities[i]);
            }
            Console.WriteLine();
        }

        public void CreateDirection() 
        {
            ShowCities();
            Console.WriteLine("Введите номер города отправления");
            int startSityNumber = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Введите номер города прибытия");
            int endSityNumber = Convert.ToInt32(Console.ReadLine());

            if (startSityNumber < 0 || startSityNumber > Cities.Count - 1 || endSityNumber < 0 || endSityNumber > Cities.Count - 1)
            {
                Console.WriteLine("Введен неверный номер города");
                Console.ReadKey();
            }
            else
            {
                _startCity = Cities[startSityNumber];
                _endCity = Cities[endSityNumber];
                Console.WriteLine($"Направление {_startCity} - {_endCity} создано");
                _directionCreated = true;
                Console.ReadKey();
            }
        }

        public void SellTickets() 
        {
            if (_directionCreated)
            {
                Random rand = new Random();
                _passengerCount = rand.Next(5, 100);
                _ticketsAreSold = true;
            }
            else
            {
                Console.WriteLine("Сначала создайте направление");
                Console.ReadKey();
            }
        }

        public void CreateTrain() 
        {
            _trainSeatsCount = 0;
            if (_directionCreated && _ticketsAreSold)
            {
                int carriageNumber = 0;
                while (_passengerCount >= _trainSeatsCount)
                {
                    _trainSeatsCount += _carriages[carriageNumber].SeatsCount;
                    carriageNumber++;
                }

                _trainCreated = true;
                _allReady = true;

                Console.WriteLine("Поезд сформирован");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Сначала продайте билеты");
                Console.ReadKey();
            }
        }

        public void ShowInfo() 
        {
            if (_directionCreated && _ticketsAreSold && _trainCreated)
            {
                Console.WriteLine($"Направление {_startCity} - {_endCity} сформировано. Поезд готов к отправке. \nПродано билетов {_passengerCount}. \nВсего мест в поезде {_trainSeatsCount}");
            }
            else if (_directionCreated && _ticketsAreSold)
            {
                Console.WriteLine($"Направление {_startCity} - {_endCity} сформировано. \nПродано билетов {_passengerCount}. Сформируйте поезд");
            }
            else if (_directionCreated)
            {
                Console.WriteLine($"Направление {_startCity} - {_endCity} сформировано.");
            }
            else
            {
                Console.WriteLine("Ниодного направления не создано. Создайте новое направление.");
            }
        }

        public void SendTrain() 
        {
            if (_allReady)
            {
                _directionCreated = false;
                _ticketsAreSold = false;
                _trainCreated = false;
                Console.WriteLine($"Поезд {_startCity} - {_endCity} отправлен.");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Сформируйте направление полностью");
                Console.ReadKey();
            }
        }
    }
}
