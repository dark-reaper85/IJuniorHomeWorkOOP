using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket
{
    class SuperMarketAdminister
    {
        static void Main(string[] args)
        {
            Market Shop = new Market();
            Shop.Operate();
        }
    }

    class Market 
    {
        private int _money;
        private Random _random;
        public List<Product> Goods = new List<Product>
        {
            new Product("огурец", 10),
            new Product("помидор", 9),
            new Product("свинина", 20),
            new Product("кабачок", 5),
            new Product("пиво", 15),
            new Product("хлеб", 7),
            new Product("молоко", 6),
            new Product("картофель", 8),
            new Product("лимонад", 7),
            new Product("конфеты", 12),
            new Product("пастила", 13),
            new Product("бумага", 5),
            new Product("игрушка", 22),
            new Product("пакет", 3),
            new Product("сметана", 11),
            new Product("майонез", 8),
        };
        private Queue<Client> _clientsQueue;

        public Market() 
        {
            _random = new Random();
            _clientsQueue = new Queue<Client>();
            _clientsQueue.Enqueue(new Client(Goods));
        }

        public void Operate() 
        {
            bool isWorking = true;
            while (isWorking)
            {
                Console.Clear();
                Console.WriteLine("1 - Обслужить клиента?");
                Console.WriteLine("2 - Позвать нового клиента");
                Console.WriteLine("3 - прервать работу магазина");
                string userInput = Console.ReadLine();
                switch (userInput)
                {
                    case "1":
                        ServeClient();
                        break;
                    case "2":
                        CallNewClient();
                        break;
                    case "3":
                        isWorking = false;
                        break;
                    default:
                        Console.WriteLine("Неизвестное действие");
                        Console.ReadKey();
                        break;
                }
            }
        }

        public void CallNewClient() 
        {
            _clientsQueue.Enqueue(new Client(Goods));
            Console.WriteLine("Новый клиент в очереди");
            Console.ReadKey();
        }

        private int RateTheCostOfShopping() 
        {
            int costOfFoods = 0;
            foreach (var product in _clientsQueue.Peek().BasketWithProducts)
            {
                costOfFoods += product.Price;
            }
            Console.WriteLine($"Стоимость покупок: {costOfFoods}");
            return costOfFoods;
        }

        private void ServeClient() 
        {
            if (_clientsQueue.Count > 0)
            {
                int costOfFoods = RateTheCostOfShopping();
                _clientsQueue.Peek().ShowProducts();

                Console.WriteLine("Готовы произвести расчет?");
                Console.ReadKey();

                if (costOfFoods > _clientsQueue.Peek().Money)
                {
                    Console.WriteLine("У клиента недостаточно средств. Он убирает лишнее.");
                    _clientsQueue.Peek().RemoveProduct();
                    costOfFoods = RateTheCostOfShopping();
                    _clientsQueue.Peek().ShowProducts();
                }

                _money += costOfFoods;
                Console.WriteLine($"Клиент обслужен. У магазина {_money} денег.");
                _clientsQueue.Dequeue();
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine($"Очередь пуста. Позовите нового клиента");
                Console.ReadKey();
            }
        }
    }

    class Client
    {
        private Random _random;
        public int Money { get; private set; }
        public List<Product> BasketWithProducts { get; private set; }
        
        public Client(List<Product> goods)
        {
            _random = new Random();
            BasketWithProducts = new List<Product>();
            Money = _random.Next(10, 40);
            FillTheBasket(goods);
        }

        public void ShowProducts() 
        {
            if (BasketWithProducts.Count > 0)
            {
                Console.WriteLine("Клиент хочет купить:");
                foreach (var product in BasketWithProducts)
                {
                    product.ShowInfo();
                }
            }
            else
            {
                Console.WriteLine("Клиент уходит без покупок");
            }
            
        }

        private void FillTheBasket(List<Product> goods) 
        {
            for (int i = 0; i < _random.Next(0,goods.Count); i++)
            {
                int j = _random.Next(0, goods.Count);
                BasketWithProducts.Add(goods[j]);
            }
        }

        public void RemoveProduct() 
        {
            int sum = SumProductsInBasket();
            
            while (sum >= Money)
            {
                if (BasketWithProducts.Count > 0)
                {
                    BasketWithProducts.RemoveAt(_random.Next(0, BasketWithProducts.Count));
                }
                else
                {
                    Console.WriteLine("У клиента недостаточно денег.");
                    Console.ReadKey();
                    break;
                }

                sum = SumProductsInBasket();
            }
        }

        private int SumProductsInBasket() 
        {
            int sum = 0;
            foreach (var product in BasketWithProducts)
            {
                sum += product.Price;
            }
            return sum;
        }
    }

    class Product 
    {
        private string _name;
        public int Price { get; private set; }
        public Product(string name, int price) 
        {
            _name = name;
            Price = price;
        }

        public void ShowInfo() 
        {
            Console.WriteLine(_name);
        }
    }
}
