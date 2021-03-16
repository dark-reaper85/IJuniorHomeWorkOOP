using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService
{
    class ManageAutoservice
    {
        static void Main(string[] args)
        {
            AutoService autoService = new AutoService();
            autoService.Operate();
        }
    }

    

    class AutoService 
    {
        private Random _random = new Random();

        private List<string> _sparePartsNames = new List<string>()
        {
            "Капот",
            "Лобовое стекло",
            "Передний бампер",
            "Задний бампер",
            "Левая передняя дверь",
            "Правая передняя дверь",
            "Левая задняя дверь",
            "Правая задняя дверь"
        };

        private int _moneyBalance;
        private bool _isWorking;
        private List<ReplacementWork> _replacementWorkCheckList;
        private List<SparePart> _spareParts;
        private Queue<ClientsCar> _clientsCarsQueue;

        public AutoService() 
        {
            _moneyBalance = _random.Next(10, 50);
            _isWorking = true;
            _spareParts = CreateSpareParts();
            _replacementWorkCheckList = CreateReplacementWork();
            _clientsCarsQueue = new Queue<ClientsCar>();
            _clientsCarsQueue.Enqueue(new ClientsCar());
        }

        public void Operate() 
        {
            while (_isWorking)
            {
                Console.Clear();
                Console.WriteLine($"На счету автосервиса: {_moneyBalance}");
                ShowSpareParts();

                if (_clientsCarsQueue.Count == 0)
                {
                    _clientsCarsQueue.Enqueue(new ClientsCar());
                }

                ShowCurrentClient();
                OperateServiceMenu();
                Console.WriteLine();
            }
        }

        private void OperateServiceMenu() 
        {
            Console.SetCursorPosition(0, _spareParts.Count + 7);

            ClientsCar currentClient = _clientsCarsQueue.Peek();

            if (CheckAvailableInSpareParts(currentClient))
            {
                Console.WriteLine("2 - Заменить поврежденную деталь.");
            }
            else
            {
                Console.WriteLine("3 - Отказать клиенту. Вы получите штраф.");
                Console.WriteLine("4 - Установить клиенту ненужную деталь.(Вы заплатите клиенту полную стоимость поврежденной детали)");
            }
            Console.WriteLine("5 - Завершить работу");

            string userInput = Console.ReadLine();

            switch (userInput)
            {
                case "1":
                    _clientsCarsQueue.Enqueue(new ClientsCar());
                    break;
                case "2":
                    if (CheckAvailableInSpareParts(currentClient))
                    {
                        ReplaceDetail(currentClient);
                    }
                    break;
                case "3":
                    if (CheckAvailableInSpareParts(currentClient) == false)
                    {
                        RefuseToClient(currentClient);
                    }
                    break;
                case "4":
                    if (CheckAvailableInSpareParts(currentClient) == false)
                    {
                        SetUnnecessarySparePart(currentClient);
                    }
                    break;
                case "5":
                    _isWorking = false;
                    break;
                default:
                    Console.WriteLine("Неизвестная команда");
                    break;
            }
        }

        private void SetUnnecessarySparePart(ClientsCar currentClient) 
        {
            SparePart unnecessarySparePart = _spareParts[_random.Next(0,_spareParts.Count)];
            unnecessarySparePart.RemoveDetail();
            _moneyBalance -= currentClient.DamagedDetail.Cost;
            Console.WriteLine($"Клиенту установили: {unnecessarySparePart.Name}. Сервис возмещает клиенту{currentClient.DamagedDetail.Cost}");
            Console.ReadKey();
            _clientsCarsQueue.Dequeue();
            if (unnecessarySparePart.Count == 0)
            {
                _spareParts.Remove(unnecessarySparePart);
            }
        }

        private void RefuseToClient(ClientsCar currentClient) 
        {
            _moneyBalance -= currentClient.DamagedDetail.Cost;
            Console.WriteLine($"Клиенту отказано. Вы получили штраф в размере: {currentClient.DamagedDetail.Cost}");
            Console.ReadKey();
            _clientsCarsQueue.Dequeue();
        }
        
        private void ReplaceDetail(ClientsCar currentClient) 
        {
            SparePart replaceableDetail;

            for (int i = 0; i < _spareParts.Count; i++)
            {
                if (CheckAvailableInSpareParts(currentClient))
                {
                    replaceableDetail = _spareParts[i];
                    replaceableDetail.RemoveDetail();
                    int workCost = replaceableDetail.Cost + TakeReplacementWorkCost(currentClient);
                    _moneyBalance += workCost;
                    Console.WriteLine($"{replaceableDetail.Name} успешно заменена. Сервис заработал: {workCost} денег");
                    Console.ReadKey();

                    if (replaceableDetail.Count == 0)
                    {
                        _spareParts.RemoveAt(i);
                        break;
                    }
                    break;
                }
            }

            _clientsCarsQueue.Dequeue();
        }

        private int TakeReplacementWorkCost(ClientsCar currentClient) 
        {
            int workCost = 0;
            foreach (var replacementWork in _replacementWorkCheckList)
            {
                if (currentClient.DamagedDetail.Name == replacementWork.Name.Substring(7))
                {
                    workCost += replacementWork.Cost;
                }
            }
            return workCost;
        }

        private bool CheckAvailableInSpareParts(ClientsCar currentClient) 
        {
            foreach (var sparePart in _spareParts)
            {
                if (currentClient.DamagedDetail.Name == sparePart.Name && sparePart.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        private void ShowSpareParts() 
        {
            Console.WriteLine("На складе:");

            foreach (var sparePart in _spareParts)
            {
                sparePart.ShowInfo();
            }
        }

        private void ShowCurrentClient() 
        {
            Console.SetCursorPosition(0, _spareParts.Count + 2);
            if (_clientsCarsQueue.Count == 0)
            {
                Console.WriteLine("Очередь пуста.");
            }
            else
            {
                ClientsCar currentClient = _clientsCarsQueue.Peek();
                Console.WriteLine("У клиента:");
                currentClient.ShowDamagedDetailInfo();
                ShowCostOfReplacementWork(currentClient);
            }
        }

        private void ShowCostOfReplacementWork(ClientsCar currentClient) 
        {
            int workCost = 0;
            foreach (var sparePart in _spareParts)
            {
                if (CheckAvailableInSpareParts(currentClient))
                {
                    workCost += sparePart.Cost;

                    foreach (var replacementWork in _replacementWorkCheckList)
                    {
                        if (currentClient.DamagedDetail.Name == replacementWork.Name.Substring(7))
                        {
                            workCost += replacementWork.Cost;

                            Console.WriteLine($"Стоимость замены: {currentClient.DamagedDetail.Name} составит: {workCost}");
                        }
                    }
                    break;
                }
                else
                {
                    Console.WriteLine("На складе отсутствуют необходимые запчасти");
                    break;
                }
            }
        }

        private List<SparePart> CreateSpareParts() 
        {
            List<SparePart> spareParts = new List<SparePart>();

            for (int i = _random.Next(_sparePartsNames.Count / 2, _sparePartsNames.Count); i > 0 ; i--)
            {
                spareParts.Add(new SparePart(_sparePartsNames[_random.Next(0, i)]));
            }

            return spareParts;
        }

        private List<ReplacementWork> CreateReplacementWork() 
        {
            List<ReplacementWork> replacementWorks = new List<ReplacementWork>();

            for (int i = 0;  i < _spareParts.Count; i++)
            {
                replacementWorks.Add(new ReplacementWork(_spareParts[i]));
            }

            return replacementWorks;
        }
    }
    
    class ClientsCar
    {
        private Random _random = new Random();
        public Detail DamagedDetail { get; private set; }
        private List<Detail> _details = new List<Detail>()
        {
            new Detail("Капот"),
            new Detail("Лобовое стекло"),
            new Detail("Передний бампер"),
            new Detail("Задний бампер"),
            new Detail("Левая передняя дверь"),
            new Detail("Правая передняя дверь"),
            new Detail("Левая задняя дверь"),
            new Detail("Правая задняя дверь"),
        };

        public ClientsCar()
        {
            DamagedDetail = _details[_random.Next(0,_details.Count)];
        }

        public void ShowDamagedDetailInfo() 
        {
            Console.WriteLine($"Повреждена деталь: {DamagedDetail.Name}");
        }
    }

    class Detail
    {
        protected Random Random = new Random();
        public string Name { get; protected set; }
        public int Cost { get; protected set; }

        public Detail(string name)
        {
            Name = name;
            Cost = Random.Next(2, 20);
        }
    }

    class SparePart : Detail
    {
        public int Count { get; private set; }

        public SparePart(string name) : base(name)
        {
            Name = name;
            Count = Random.Next(1, 4);
        }

        public void ShowInfo() 
        {
            Console.WriteLine($"{Name}, количество :{Count}.");
        }

        public void RemoveDetail() 
        {
            Count--;
        }
    }

    class ReplacementWork 
    {
        private Random _random = new Random();
        public string Name { get; protected set; }
        public int Cost { get; protected set; }

        public ReplacementWork(Detail detail) 
        {
            Name = $"Замена {detail.Name}";
            Cost = (int)(detail.Cost * 0.4);
        }

        public void ShowInfo() 
        {
            Console.WriteLine($"{Name}. Стоимость: {Cost}");
        }
    }

}
