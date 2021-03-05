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
            
            List<Product> goods = new List<Product>();
            goods.Add(new Product("Яблоко", 2));
            goods.Add(new Product("Груша", 3));
            goods.Add(new Product("Гранат", 5));
            goods.Add(new Product("Огурец", 1));
            goods.Add(new Product("Помидор", 2));
            goods.Add(new Product("Лук", 1));
            goods.Add(new Product("Хлеб", 5));

            Vendor vendor = new Vendor(50, goods);

            bool trading = true;
            while (trading)
            {
                Console.Clear();
                Console.WriteLine("1 - Купить товар");
                Console.WriteLine("2 - Посмотреть инвентарь");
                Console.WriteLine("3 - Проверить свой баланс");
                Console.WriteLine("4 - Выход из окна торговли\n");
                

                int userInput = Convert.ToInt32(Console.ReadLine());
                switch (userInput)
                {
                    case 1:
                        Console.WriteLine("\nСписок товаров:");
                        vendor.ShowAllGoods();

                        Console.WriteLine("Введите номер товара который хотите купить");
                        int indexOfGood = Convert.ToInt32(Console.ReadLine());
                        vendor.SellGood(player, indexOfGood);
                        break;
                    case 2:
                        player.ShowInventory();
                        break;
                    case 3:
                        Console.WriteLine("У вас " + player.Coins + "монет");
                        Console.ReadKey();
                        break;
                    case 4:
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
        public int Balance { get; private set; }
        public List<Product> Goods { get; private set; }

        public Vendor(int balance, List<Product> goods) 
        {
            Balance = balance;
            Goods = goods;
        }

        public void SellGood(Player player, int indexOfGood) 
        {
            if (indexOfGood < 0 || indexOfGood > Goods.Count - 1)
            {
                Console.WriteLine("Товар с таким номером отсутствует");
                Console.ReadKey();
            }
            else if (player.Coins < Goods[indexOfGood].Cost)
            {
                Console.WriteLine("У вас недостаточно средств");
                Console.ReadKey();
            }
            else
            {
                Balance += Goods[indexOfGood].Cost;
                player.BuyProduct(Goods[indexOfGood]);
                Goods.RemoveAt(indexOfGood);
            }
        }

        public void ShowAllGoods() 
        {
            for (int i = 0; i < Goods.Count; i++)
            {
                Console.WriteLine(i + " " + Goods[i].ShowInfo()); 
            }
        }


    }

    class Player 
    {
        public int Coins { get; private set; }
        public List<Product> Inventory;

        public Player(int coins) 
        {
            Coins = coins;
            Inventory = new List<Product>();
        }
        public void ShowInventory() 
        {
            foreach (var product in Inventory)
            {
                Console.WriteLine(product.ShowInfo());
            }
            Console.ReadKey();
        }

        public void BuyProduct(Product product) 
        {
            Coins -= product.Cost; 
            Inventory.Add(product);
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
