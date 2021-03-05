using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    class WorkWithDataBase
    {
        static void Main(string[] args)
        {
            Database db = new Database();
            db.Operate();
        }
    }
    class Database 
    {
        private List<Player> _players;

        public Database() { _players = new List<Player>(); }
        public void Operate() 
        {
            bool isWorking = true;

            while (isWorking)
            {
                Console.Clear();
                Console.WriteLine("1 - Добавить игрока в базу");
                if (_players.Count > 0 )
                {
                    Console.WriteLine("2 - Вывести список всех игроков");
                    Console.WriteLine("3 - Забанить игрока");
                    Console.WriteLine("4 - Разбанить игрока");
                }
                
                Console.WriteLine("5 - Выход");

                string userInput = Console.ReadLine();
                switch (userInput)
                {
                    case "1":
                        AddPlayer();
                        break;
                    case "2":
                        if (_players.Count > 0)
                            ShowAllPlayers();
                        break;
                    case "3":
                        if (_players.Count > 0)
                            BannPlayer();
                        break;
                    case "4":
                        if (_players.Count > 0)
                            UnbannPlayer();
                        break;
                    case "5":
                        isWorking = false;
                        break;
                    default:
                        Console.WriteLine("неизвестная команда");
                        break;
                }
            }
        }

        private void AddPlayer()
        {
            Console.WriteLine("Введите никнейм нового игрока");
            string nickname = Console.ReadLine();
            Console.WriteLine("Введите уровень нового игрока");
            int level = Convert.ToInt32(Console.ReadLine());

            Player player = new Player(nickname, level);

            _players.Add(player);
        }

        private Player FindPlayer(int id) 
        {
            Player findingPlayer;
            foreach (var player in _players)
            {
                if (player.ID == id)
                {
                    findingPlayer = player;
                    return findingPlayer;
                }
                else
                {
                    Console.WriteLine("Игрок с таким ID не найден");
                    Console.ReadKey();
                    return null;
                }
            }
            return null;
        }

        private void ShowAllPlayers() 
        {
            foreach (var player in _players)
            {
                player.ShowInfo();
            }
            Console.ReadKey();
        }

        private void BannPlayer() 
        {
            Console.WriteLine("Введите id игрока которого нужно забанить");
            int id = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("На сколько часов забанить игрока?");
            double time = Convert.ToDouble(Console.ReadLine());

            Player banningPlayer = FindPlayer(id);
            banningPlayer.Bann(time);

            Console.WriteLine($"Игрок с ником {banningPlayer.Nickname} забанен");
            Console.ReadKey();
        }
        private void UnbannPlayer() 
        {
            Console.WriteLine("Введите id игрока которого нужно разбанить");
            int id = Convert.ToInt32(Console.ReadLine());


            Player banningPlayer = FindPlayer(id);
            banningPlayer.UnBann();

            Console.WriteLine($"Игрок с ником {banningPlayer.Nickname} разбанен");
            Console.ReadKey();
        }

    }

    class Player 
    {
        private static int count = 1;
        public int ID { get; private set; }
        public string Nickname { get; private set; }
        public int Level { get; private set; }
        public bool isBanned { get; private set; }
        public DateTime BannedTime { get; private set; }

        public Player(string nickname, int level) 
        {
            ID = count;
            Nickname = nickname;
            Level = level;
            isBanned = false;
            count++;
        }

        public void ShowInfo()
        {
            if (isBanned)
                Console.WriteLine($"ID Игрока - {ID}\nНикнейм - {Nickname}\nУровень - {Level}\nИгрок забанен до {BannedTime}\n");
            else 
                Console.WriteLine($"ID Игрока - {ID}\nНикнейм - {Nickname}\nУровень - {Level}\nИгрок активен\n");
        }

        public void Bann(double time) 
        {
            if (time > 0)
            {
                BannedTime = DateTime.Now;
                BannedTime = BannedTime.AddHours(time);
                isBanned = true;
            }
            else
            {
                Console.WriteLine("Вы хотите разбанить игрока?\nЕсли Да, то выберите соответствующий пункт меню.");
                Console.ReadKey();
            }
        }
        public void UnBann() 
        {
            isBanned = false;
        }
    }
}
