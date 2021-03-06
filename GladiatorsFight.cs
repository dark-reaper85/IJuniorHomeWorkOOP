using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GladiatorFight
{
    class GladiatorsFight
    {
        static void Main(string[] args)
        {
            BattleGround battleGround = new BattleGround();

            battleGround.OrganizeBattle();
        }
    }

    class BattleGround 
    {
        public Random _random;
        private Gladiator _firstGladiator;
        private Gladiator _secondGladiator;

        
        private List<Gladiator> _gladiators = new List<Gladiator>
        {
             new Warrior("Боб", "Мощный удар"),
             new Archer("Робин", "Powershot"),
             new Mage("Гендальф", "FireBall"),
             new Rogue("Фесс", "Потрошение"),
             new Barbarian("Кратос", "Размашистый удар"),
        };

        public BattleGround()
        {
            _random = new Random();
        }

        public void OrganizeBattle() 
        {
            bool fight = true;
            while (fight)
            {
                Console.Clear();
                ShowGladiators();
                SelectGladiator();

                Console.SetCursorPosition(0, 10);

                if (_firstGladiator != null && _secondGladiator != null )
                {
                    while (_firstGladiator.Health > 0 && _secondGladiator.Health > 0)
                    {
                        Fight(_firstGladiator, _secondGladiator);

                        Thread.Sleep(500);
                    }

                    if (_firstGladiator.Health < 0 && _gladiators.Count > 1)
                    {
                        Console.WriteLine($"{_secondGladiator.Name} победил \n{_secondGladiator.ShowInfo()}");
                        _gladiators.Remove(_firstGladiator);
                        Console.ReadKey();
                    }
                    else if (_secondGladiator.Health < 0 && _gladiators.Count > 1)
                    {
                        Console.WriteLine($"{_firstGladiator.Name} победил \n{_firstGladiator.ShowInfo()}");
                        _gladiators.Remove(_secondGladiator);
                        Console.ReadKey();
                    }

                    if (_gladiators.Count == 1)
                    {
                        Console.Clear();
                        Console.WriteLine($"{_gladiators[0].Name} Признан абсолютным чемпионом!!! \n{_gladiators[0].ShowInfo()}");
                        fight = false;
                        Console.ReadKey();
                    }
                }
            }
        }

        public void ShowGladiators()
        {
            for (int i = 0; i < _gladiators.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {_gladiators[i].ShowInfo()}");
            }
        }

        private void SelectGladiator() 
        {
            Console.WriteLine("Выберите первого гладиатора");
            string userInput = Console.ReadLine();
            _firstGladiator = TakeGladiator(userInput);
            if (_firstGladiator != null)
            {
                Console.WriteLine("Выберите второго гладиатора");
                userInput = Console.ReadLine();
                _secondGladiator = TakeGladiator(userInput);
            }

            if (_firstGladiator != null && _secondGladiator != null && _firstGladiator.Name == _secondGladiator.Name)
            {
                Console.WriteLine("Гладиатор не может сражаться сам с собой");
                _firstGladiator = null;
                Console.ReadKey();
            }
        }

        private Gladiator TakeGladiator(string userInput) 
        {
            if (int.TryParse(userInput, out int secondGladiatorNumber) && secondGladiatorNumber > 0 && secondGladiatorNumber <= _gladiators.Count)
            {
                
                return _gladiators[secondGladiatorNumber - 1];
            }
            else
            {
                Console.WriteLine("Ошибка ввода, или гладиатор не существует");
                Console.ReadKey();
                return null;
            }
        }

        private void Fight(Gladiator gladiator1, Gladiator gladiator2) 
        {
            switch (_random.Next(1, 5))
            {
                case 1:
                    ChoseGladiatorAttack(gladiator1, gladiator2);
                    break;
                case 2:
                    ChoseGladiatorAttack(gladiator2, gladiator1);
                    break;
                default:
                    Console.WriteLine("Бойцы обходят друг друга.");
                    break;
            }
        }

        private void ChoseGladiatorAttack(Gladiator gladiator1, Gladiator gladiator2) 
        {
            switch (_random.Next(1, 3))
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
        protected int Damage;
        protected Random _random;

        public string Name { get; protected set; }
        public int Health { get; protected set; }
        public string SpecialAttackName { get; protected set; }

        public abstract void TakeDamage(int damage);
        public abstract string ShowInfo();
        public abstract int SpecialAttack();
        public int Attack() 
        {
            return Damage;
        }
    }

    class Warrior : Gladiator
    {
        private int _armor;

        public Warrior(string name, string specialAttackName)
        {
            _random = new Random();
            Name = name;
            Health = _random.Next(100, 150);
            Damage = _random.Next(10, 25);
            _armor = _random.Next(5, 40);
            SpecialAttackName = specialAttackName;
        }

        public override string ShowInfo()
        {
            return $"Воин - {Name}. Здоровье - {Health}. Броня - {_armor}. Урон - {Damage}. Спецприем - {SpecialAttackName}";
        }

        public override int SpecialAttack()
        {
            return Damage * _random.Next(2,5) ;
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
            _random = new Random();
            Name = name;
            Health = _random.Next(100, 120);
            Damage = _random.Next(15, 40);
            SpecialAttackName = specialAttackName;
        }

        public override string ShowInfo()
        {
            return $"Лучник - {Name}. Здоровье - {Health}. Урон - {Damage}. Спецприем - {SpecialAttackName}";
        }

        public override int SpecialAttack()
        {
            return Damage * 2 * _random.Next(2, 5);
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
            _random = new Random();
            Name = name;
            Health = _random.Next(70, 120);
            Damage = _random.Next(10, 25);
            _mana = _random.Next(120, 150);
            _specialAttackManaCost = _random.Next(15, 50);
            SpecialAttackName = specialAttackName;
        }

        public override string ShowInfo()
        {
            return $"Маг - {Name}. Здоровье - {Health}. Мана - {_mana}. Урон - {Damage}. Спецприем - {SpecialAttackName}";
        }

        public override int SpecialAttack()
        {
            if (_mana < _specialAttackManaCost)
            {
                return 0;
            }
            else
            {
                _mana -= _specialAttackManaCost;
                return Damage * _random.Next(3, 8);
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
            _random = new Random();
            Name = name;
            Health = _random.Next(70, 120);
            Damage = _random.Next(15, 35);
            _dodgeChance = _random.Next(20, 50);
            SpecialAttackName = specialAttackName;
        }

        public override string ShowInfo()
        {
            return $"Вор - {Name}. Здоровье - {Health}. Уклонение - {_dodgeChance}. Урон - {Damage}. Спецприем - {SpecialAttackName}";
        }

        public override int SpecialAttack()
        {
            return Damage * _random.Next(3, 8);
        }

        private bool IsDodged() 
        {
            int chance = _random.Next(101);
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
            _random = new Random();
            Name = name;
            Health = _random.Next(150, 220);
            Damage = _random.Next(25, 50);
            SpecialAttackName = specialAttackName;
        }

        public override string ShowInfo()
        {
            return $"Варвар - {Name}. Здоровье - {Health}. Урон - {Damage}. Спецприем - {SpecialAttackName}";
        }

        public override int SpecialAttack()
        {
            _random = new Random();
            return Damage * _random.Next(2, 5);
        }

        public override void TakeDamage(int damage)
        {
            Health -= damage;
        }
    }
}
