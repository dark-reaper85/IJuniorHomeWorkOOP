using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IJuniorHomeWorkOOP
{
    class PlayerClass
    {
        static void Main(string[] args)
        {
            Player player = new Player("Игрок 1", 200, 25, 15, 32);
            player.ShowInfo();
        }
    }

    class Player
    {
        private string _name;
        private int _health;
        private int _armor;
        private int _dodge;
        private int _damage;

        public Player(string name, int health, int armor, int dodge, int damage) 
        {
            _name = name;
            _health = health;
            _armor = armor;
            _dodge = dodge;
            _damage = damage;
        }

        public void ShowInfo() 
        {
            Console.WriteLine($"Имя - {_name}\nЗдоровье - {_health}\nБроня - {_armor}\nУклонение - {_dodge}\nУрон - {_damage}");
        }
    }
}
