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
            Database playersDataBase = new Database();
            playersDataBase.Operate();
        }
    }

    class Database 
    {
        private List<Player> _players;

        public Database() 
        { 
            _players = new List<Player>(); 
        }

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
                        ShowAllPlayers();
                        break;
                    case "3":
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
            string levelInput = Console.ReadLine();
            if (int.TryParse(levelInput, out int level))
            {
                Player player = new Player(nickname, level);

                _players.Add(player);
            }
            else
            {
                Console.WriteLine("Ошибка ввода");
                Console.ReadKey();
            }
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
            }
            return null;
        }

        private void ShowAllPlayers() 
        {
            if (_players.Count > 0)
            {
                foreach (var player in _players)
                {
                    player.ShowInfo();
                }
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("База данных пуста");
            } 
        }

        private void BannPlayer() 
        {
            if (_players.Count > 0) 
            {
                Console.WriteLine("Введите id игрока которого нужно забанить");
                string userInput = Console.ReadLine();
                if (int.TryParse(userInput, out int id))
                {
                    if (FindPlayer(id) == null)
                    {
                        Console.WriteLine("Игрок с таким ID не найден");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("На сколько часов забанить игрока?");
                        userInput = Console.ReadLine();
                        if (double.TryParse(userInput, out double time))
                        {
                            Player playerToBann = FindPlayer(id);
                            playerToBann.Bann(time);
                            Console.WriteLine($"Игрок с ником {playerToBann.Nickname} забанен");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine("Ошибка ввода");
                            Console.ReadKey();
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка ввода");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("База данных пуста");
            }
        }

        private void UnbannPlayer() 
        {
            if (_players.Count > 0) 
            {
                Console.WriteLine("Введите id игрока которого нужно разбанить");
                string userInput = Console.ReadLine();
                if (int.TryParse(userInput, out int id))
                {
                    if (FindPlayer(id) == null)
                    {
                        Console.WriteLine("Игрок с таким ID не найден");
                        Console.ReadKey();
                    }
                    else
                    {
                        Player bannedPlayer = FindPlayer(id);
                        if (bannedPlayer.IsBanned == false)
                        {
                            Console.WriteLine("Игрок активен, его не нужно разбанивать");
                            Console.ReadKey();
                        }
                        else
                        {
                            bannedPlayer.UnBann();
                            Console.WriteLine($"Игрок с ником {bannedPlayer.Nickname} разбанен");
                            Console.ReadKey();
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка ввода");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("База данных пуста");
            }
        }
    }

    class Player 
    {
        private static int _lastID = 1;
        public int ID { get; private set; }
        public string Nickname { get; private set; }
        public int Level { get; private set; }
        public bool IsBanned { get; private set; }
        public DateTime BannedTime { get; private set; }

        public Player(string nickname, int level) 
        {
            ID = _lastID;
            Nickname = nickname;
            Level = level;
            IsBanned = false;
            _lastID++;
        }

        public void ShowInfo()
        {
            if (IsBanned)
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
                IsBanned = true;
            }
            else
            {
                Console.WriteLine("Вы хотите разбанить игрока?\nЕсли Да, то выберите соответствующий пункт меню.");
                Console.ReadKey();
            }
        }

        public void UnBann() 
        {
            IsBanned = false;
        }
    }
}
