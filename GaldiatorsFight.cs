using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GladiatorFight
{
    class GaldiatorsFight
    {
        static void Main(string[] args)
        {
            List<Gladiator> gladiators = new List<Gladiator>
            {
                new Warrior("Боб", "Мощный удар"),
                new Archer("Робин", "Powershot"),
                new Mage("Гендальф", "FireBall"),
                new Rogue("Фесс", "Потрошение"),
                new Barbarian("Кратос", "Размашистый удар"),
            };

            BattleGround battleGround = new BattleGround(gladiators);

            battleGround.ShowGladiators();
            Console.WriteLine("Выберите первого гладиатора");
            int i = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Выберите второго гладиатора");
            int j = Convert.ToInt32(Console.ReadLine());
            Gladiator gladiator1 = battleGround.Gladiators[i-1];
            Gladiator gladiator2 = battleGround.Gladiators[j-1];

            while (gladiator1.Health > 0 && gladiator2.Health > 0)
            {
                Random rand = new Random();
                battleGround.Fight(gladiator1, gladiator2, rand);

                Thread.Sleep(500);
            }

            if (gladiator1.Health < 0)
            {
                Console.WriteLine($"{gladiator2.Name} победил \n{gladiator2.ShowInfo()}");
                Console.ReadKey();
            }
            else 
            {
                Console.WriteLine($"{gladiator1.Name} победил \n{gladiator1.ShowInfo()}");
                Console.ReadKey();
            }
        }
    }

    class BattleGround 
    {
        public List<Gladiator> Gladiators { get; private set; }

        public BattleGround(List<Gladiator> gladiators) 
        {
            Gladiators = gladiators;
        }

        public void ShowGladiators() 
        {
            for (int i = 0; i < Gladiators.Count; i++)
            {
                Console.WriteLine($"{i+1} - {Gladiators[i].ShowInfo()}");
            }
        }

        public void Fight(Gladiator gladiator1, Gladiator gladiator2, Random random) 
        {
                switch (random.Next(1, 5))
                {
                    case 1:
                        GladiatorsFight(gladiator1, gladiator2, random);
                        break;
                    case 2:
                        GladiatorsFight(gladiator2, gladiator1, random);
                        break;
                    default:
                        Console.WriteLine("Бойцы обходят друг друга.");
                        break;
                }
        }

        private void GladiatorsFight(Gladiator gladiator1, Gladiator gladiator2, Random rand1) 
        {
                switch (rand1.Next(1, 3))
                {
                    case 1:
                        Console.WriteLine($"\n{gladiator1.Name} наносит урон {gladiator1.Attack()} по {gladiator2.Name}");
                        gladiator2.TakeDamage(gladiator1.Attack());
                        Console.WriteLine($"Здоровье {gladiator2.Name}: {gladiator2.Health}");
                        break;
                    case 2:
                        Console.WriteLine($"\n{gladiator1.Name} применяет {gladiator1.SpecialAttackName} и наносит урон {gladiator1.SpecialAttack()} по {gladiator2.Name}");
                        gladiator2.TakeDamage(gladiator1.SpecialAttack());
                        Console.WriteLine($"Здоровье {gladiator2.Name}: {gladiator2.Health}");
                        break;
                }
        }
    }

    abstract class Gladiator 
    {
        public string Name { get; protected set; }
        public int Health { get; protected set; }
        protected int Damage;
        public string SpecialAttackName { get; protected set; }

        public abstract void TakeDamage(int damage);
        public abstract string ShowInfo();
        public int Attack() 
        {
            return Damage;
        }
        public abstract int SpecialAttack();
    }

    class Warrior : Gladiator
    {
        private int _armor;

        public Warrior(string name, string specialAttackName)
        {
            Random rand = new Random();
            Name = name;
            Health = rand.Next(100, 150);
            Damage = rand.Next(10, 25);
            _armor = rand.Next(5, 40);
            SpecialAttackName = specialAttackName;
        }

        public override string ShowInfo()
        {
            return $"Воин - {Name}. Здоровье - {Health}. Броня - {_armor}. Урон - {Damage}. Спецприем - {SpecialAttackName}";
        }

        public override int SpecialAttack()
        {
            Random rand = new Random();
            return Damage * rand.Next(2,5) ;
        }

        public override void TakeDamage(int damage)
        {
            Health -= damage * _armor / 100; 
        }
    }

    class Archer : Gladiator
    {
        public Archer(string name, string specialAttackName)
        {
            Random rand = new Random();
            Name = name;
            Health = rand.Next(100, 120);
            Damage = rand.Next(15, 40);
            SpecialAttackName = specialAttackName;
        }

        public override string ShowInfo()
        {
            return $"Лучник - {Name}. Здоровье - {Health}. Урон - {Damage}. Спецприем - {SpecialAttackName}";
        }

        public override int SpecialAttack()
        {
            Random rand = new Random();
            return Damage * 2 * rand.Next(2, 5);
        }

        public override void TakeDamage(int damage)
        {
            Health -= damage;
        }
    }
    class Mage : Gladiator
    {
        private int _mana;
        private int _specialAttackManaCost;

        public Mage(string name, string specialAttackName)
        {
            Random rand = new Random();
            Name = name;
            Health = rand.Next(70, 120);
            Damage = rand.Next(10, 25);
            _mana = rand.Next(120, 150);
            _specialAttackManaCost = rand.Next(15, 50);
            SpecialAttackName = specialAttackName;
        }

        public override string ShowInfo()
        {
            return $"Маг - {Name}. Здоровье - {Health}. Мана - {_mana}. Урон - {Damage}. Спецприем - {SpecialAttackName}";
        }

        public override int SpecialAttack()
        {
            Random rand = new Random();
            if (_mana < _specialAttackManaCost)
            {
                return 0;
            }
            else
            {
                _mana -= _specialAttackManaCost;
                return Damage * rand.Next(3, 8);
            }
        }

        public override void TakeDamage(int damage)
        {
            if (_mana > damage)
            {
                _mana -= damage;
            }
            else if (_mana > 0 && _mana < damage)
            {
                Health -= damage - _mana;
                _mana = 0;
            }
            else
            {
                Health -= damage;
            }
        }
    }
    class Rogue : Gladiator
    {
        private int _dodgeChance;

        public Rogue(string name, string specialAttackName)
        {
            Random rand = new Random();
            Name = name;
            Health = rand.Next(70, 120);
            Damage = rand.Next(15, 35);
            _dodgeChance = rand.Next(20, 50);
            SpecialAttackName = specialAttackName;
        }

        public override string ShowInfo()
        {
            return $"Вор - {Name}. Здоровье - {Health}. Уклонение - {_dodgeChance}. Урон - {Damage}. Спецприем - {SpecialAttackName}";
        }

        public override int SpecialAttack()
        {
            Random rand = new Random();
            return Damage * rand.Next(3, 8);
        }
        private bool IsDodged() 
        {
            Random rand = new Random();
            int chance = rand.Next(101);
            if (chance < _dodgeChance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void TakeDamage(int damage)
        {
            bool isDodged = IsDodged();
            if (isDodged)
            {
                Console.WriteLine("Вор уклоняется");
            }
            else 
            {
                Health -= damage;
            }
        }
    }

    class Barbarian : Gladiator
    {
        public Barbarian(string name, string specialAttackName)
        {
            Random rand = new Random();
            Name = name;
            Health = rand.Next(150, 220);
            Damage = rand.Next(25, 50);
            SpecialAttackName = specialAttackName;
        }

        public override string ShowInfo()
        {
            return $"Варвар - {Name}. Здоровье - {Health}. Урон - {Damage}. Спецприем - {SpecialAttackName}";
        }

        public override int SpecialAttack()
        {
            Random rand = new Random();
            return Damage * rand.Next(2, 5);
        }

        public override void TakeDamage(int damage)
        {
            Health -= damage;
        }
    }
}
