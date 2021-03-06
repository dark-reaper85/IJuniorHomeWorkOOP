using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingVendor
{
    class MeetVendor
    {
        static void Main(string[] args)
        {
            Player player = new Player(10);

            Vendor vendor = new Vendor();

            bool trading = true;
            while (trading)
            {
                Console.Clear();
                Console.WriteLine("1 - Купить товар");
                Console.WriteLine("2 - Посмотреть инвентарь");
                Console.WriteLine("3 - Проверить свой баланс");
                Console.WriteLine("4 - Выход из окна торговли\n");

                string userInput = Console.ReadLine();
                switch (userInput)
                {
                    case "1":
                        Console.WriteLine("\nСписок товаров:");
                        vendor.ShowAllGoods();

                        Console.WriteLine("Введите номер товара который хотите купить");
                        userInput = Console.ReadLine();
                        if (int.TryParse(userInput, out int indexOfGood))
                        {
                            vendor.SellGood(player, indexOfGood);
                        }
                        else
                        {
                            Console.WriteLine("Ошибка ввода");
                            Console.ReadKey();
                        }
                        break;
                    case "2":
                        player.ShowInventory();
                        break;
                    case "3":
                        Console.WriteLine("У вас " + player.Coins + " монет");
                        Console.ReadKey();
                        break;
                    case "4":
                        trading = false;
                        break;
                    default:
                        Console.WriteLine("Неизвестное действие");
                        break;
                }
            }
        }
    }

    class Vendor
    {
        private List<Product> _goods = new List<Product>()
        {
            new Product("Яблоко", 2),
            new Product("Груша", 3),
            new Product("Гранат", 5),
            new Product("Огурец", 1),
            new Product("Помидор", 2),
            new Product("Лук", 1),
            new Product("Хлеб", 5)
        };

        public void SellGood(Player player, int indexOfGood) 
        {
            if (indexOfGood < 0 || indexOfGood > _goods.Count - 1)
            {
                Console.WriteLine("Товар с таким номером отсутствует");
                Console.ReadKey();
            }
            else if (player.Coins < _goods[indexOfGood].Cost)
            {
                Console.WriteLine("У вас недостаточно средств");
                Console.ReadKey();
            }
            else
            {
                player.BuyProduct(_goods[indexOfGood]);
                Console.WriteLine($"{_goods[indexOfGood].Name} Куплено");
                Console.ReadKey();
                _goods.RemoveAt(indexOfGood);
            }
        }

        public void ShowAllGoods() 
        {
            for (int i = 0; i < _goods.Count; i++)
            {
                Console.WriteLine(i + " " + _goods[i].ShowInfo()); 
            }
        }
    }

    class Player 
    {
        public int Coins { get; private set; }
        private List<Product> _inventory;

        public Player(int coins) 
        {
            Coins = coins;
            _inventory = new List<Product>();
        }

        public void ShowInventory() 
        {
            if (_inventory.Count == 0)
            {
                Console.WriteLine("Инвентарь пуст");
                Console.ReadKey();
            }
            else
            {
                foreach (var product in _inventory)
                {
                    Console.WriteLine(product.ShowInfo());
                }
                Console.ReadKey();
            }
        }

        public void BuyProduct(Product product) 
        {
            Coins -= product.Cost; 
            _inventory.Add(product);
        }
    }
    
    class Product 
    {
        public string Name { get; private set; }
        public int Cost { get; private set; }

        public Product(string name, int cost) 
        {
            Name = name;
            Cost = cost;
        }

        public string ShowInfo() 
        {
            return $"{Name} цена - {Cost}";
        }
    }
}
