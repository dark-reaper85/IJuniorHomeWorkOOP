using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TeamGladiatorFight
{
    class GladiatorsFight
    {
        static void Main(string[] args)
        {
            Battleground battleground = new Battleground();

            battleground.OrganizeBattle();
        }
    }

    class Battleground
    {
        private Random _random = new Random();
        private Team _team1;
        private Team _team2;
        private bool _allTeamsLive;
        private List<Gladiator> _gladiators;
        private List<Team> _teams;

        public Battleground()
        {
            _allTeamsLive = true;
            _gladiators = new List<Gladiator>
            {
                 new Warrior("Боб", "Мощный удар"),
                 new Archer("Робин", "Powershot"),
                 new Mage("Гендальф", "FireBall"),
                 new Rogue("Фесс", "Потрошение"),
                 new Barbarian("Кратос", "Размашистый удар"),
                 new Warrior("Вася", "Мощный удар"),
                 new Archer("Леголас", "Powershot"),
                 new Mage("Джайна", "FireBall"),
                 new Rogue("Дзирт", "Потрошение"),
                 new Barbarian("Конан", "Размашистый удар")
            };

            _teams = new List<Team>();
        }

        public void OrganizeBattle()
        {
            ShowGladiators();
            CreateTeams();
            while (_allTeamsLive)
            {
                ShowTeams();
                TakeTeamsToFight();
                Fight(_team1, _team2);
                Thread.Sleep(500);
            }
        }

        private bool AreAllTeamsLive() 
        {
            if (_teams.Count > 1)
            {
                return true;
            }
            else 
            {
                Console.Clear();
                Console.WriteLine($"{_teams[0].Label} Абсолютный Чемпион");
                Console.ReadKey();
                _allTeamsLive = false;
                return false;
            }
        }

        public void ShowGladiators()
        {
            for (int i = 0; i < _gladiators.Count; i++)
            {
                Console.Write(i + 1 + ": ");
                _gladiators[i].ShowInfo();
            }
        }

        private void TakeTeamsToFight() 
        {
            if (_teams.Count > 0)
            {
                int firstTeamNumber = _random.Next(0, _teams.Count);
                int secondTeamNumber = _random.Next(0, _teams.Count);
                while (firstTeamNumber == secondTeamNumber)
                    secondTeamNumber = _random.Next(0, _teams.Count);

                _team1 = _teams[firstTeamNumber];
                _team2 = _teams[secondTeamNumber];
            }
            else
            {
                Console.WriteLine();
            }       
        }

        private void Fight(Team team1, Team team2)
        {
            switch (_random.Next(1, 3))
            {
                case 1:
                    ChoseGladiatorAttack(team1, team2);
                    break;
                case 2:
                    ChoseGladiatorAttack(team2, team1);
                    break;
            }
        }

        private void ChoseGladiatorAttack(Team team1, Team team2)
        {
            Gladiator gladiator1 = team1.GetRandomGladiator();
            Gladiator gladiator2 = team2.GetRandomGladiator();
            string gladiatorTakeDamage = $" по { gladiator2.Name} из команды { team2.Label}\n";
            string showGladiatorHealth = $"Здоровье {gladiator2.Name} из команды {team2.Label}: {gladiator2.Health}";
            switch (_random.Next(1, 3))
            {
                case 1:
                    Console.WriteLine($"\n{gladiator1.Name} из команды {team1.Label} наносит урон {gladiator1.Attack()}" + gladiatorTakeDamage);
                    
                    gladiator2.TakeDamage(gladiator1.Attack());
                    Console.WriteLine(showGladiatorHealth);
                    
                    team2.IsGladiatorDead(gladiator2);
                    
                    CheckWinner(team2);
                    break;
                case 2:
                    Console.WriteLine($"\n{gladiator1.Name} из команды {team1.Label} применяет {gladiator1.SpecialAttackName} и наносит урон {gladiator1.SpecialAttack()}" + gladiatorTakeDamage);
                    
                    gladiator2.TakeDamage(gladiator1.Attack());
                    Console.WriteLine(showGladiatorHealth);
                    
                    team2.IsGladiatorDead(gladiator2);
                    
                    CheckWinner(team2);
                    break;
            }
        }

        private void CheckWinner(Team team) 
        {
            if (team.IsTeamLeaveBattle())
            {
                Console.WriteLine($"Команда {team.Label} проиграла");
                _teams.Remove(team);
                AreAllTeamsLive();
            }
        }

        private void CreateTeams() 
        {
            string userInput;
            int teamsCount = _random.Next(2, _gladiators.Count);
            int teamsToCreate = teamsCount;
            for (int i = 0; i < teamsCount; i++)
            {
                Console.WriteLine($"\nВведите название команды №{i+1}");
                userInput = Console.ReadLine();

                _teams.Add(FillTeam(userInput, teamsToCreate));
                teamsToCreate--;
            }
        }

        private Team FillTeam(string teamLabel , int teamsToCreate) 
        {
            List<Gladiator> gladiators = new List<Gladiator>();
            int gladiatorsCount = _gladiators.Count / teamsToCreate;

            for (int i = 0; i < gladiatorsCount; i++)
            {
                int gladiatorNumber = _random.Next(0, _gladiators.Count);

                gladiators.Add(_gladiators[gladiatorNumber]);

                _gladiators.RemoveAt(gladiatorNumber);
            }
            
            return new Team(teamLabel, gladiators);
        }

        public void ShowTeams() 
        {
            Console.Clear();
            foreach (var team in _teams)
            {
                team.ShowInfo();
                Console.WriteLine();
            }
        }
    }

    class Team
    {
        private Random _random = new Random();
        private List<Gladiator> _gladiators;

        public string Label { get; private set; }
        public int GladiatorsCount { get; private set; }

        public Team(string teamLabel, List<Gladiator> gladiators)
        {
            Label = teamLabel;
            _gladiators = gladiators;
            GladiatorsCount = _gladiators.Count;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Команда: {Label}");

            for (int i = 0; i < _gladiators.Count; i++)
            {
                Console.Write(i+1 + ": ");
                _gladiators[i].ShowInfo();
            }
        }

        public Gladiator GetRandomGladiator()
        {
            return _gladiators[_random.Next(0, _gladiators.Count)];
        }

        public void IsGladiatorDead(Gladiator gladiator) 
        {
            if (gladiator.Health <= 0)
            {
                Console.WriteLine($"{gladiator.Name} умер и выбывает");
                _gladiators.Remove(gladiator);
            }  
        }

        public bool IsTeamLeaveBattle() 
        {
            return _gladiators.Count == 0;
        }
    }

    abstract class Gladiator
    {
        protected int Damage;
        protected Random Random = new Random();

        public string ClassLabel { get; protected set; }
        public string Name { get; protected set; }
        public int Health { get; protected set; }
        public string SpecialAttackName { get; protected set; }

        public abstract void TakeDamage(int damage);
        public abstract void ShowInfo();
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
            ClassLabel = "Воин";
            Name = name;
            Health = Random.Next(100, 150);
            Damage = Random.Next(10, 25);
            _armor = Random.Next(5, 40);
            SpecialAttackName = specialAttackName;
        }

        public override void ShowInfo()
        {
            Console.WriteLine($"{ClassLabel} - {Name}. Здоровье - {Health}. Броня - {_armor}. Урон - {Damage}. Спецприем - {SpecialAttackName}");
        }

        public override int SpecialAttack()
        {
            return Damage * Random.Next(2, 5);
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
            ClassLabel = "Лучник";
            Name = name;
            Health = Random.Next(100, 120);
            Damage = Random.Next(15, 40);
            SpecialAttackName = specialAttackName;
        }

        public override void ShowInfo()
        {
            Console.WriteLine($"{ClassLabel} - {Name}. Здоровье - {Health}. Урон - {Damage}. Спецприем - {SpecialAttackName}"); 
        }

        public override int SpecialAttack()
        {
            return Damage * 2 * Random.Next(2, 5);
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
            ClassLabel = "Маг";
            Name = name;
            Health = Random.Next(70, 120);
            Damage = Random.Next(10, 25);
            _mana = Random.Next(120, 150);
            _specialAttackManaCost = Random.Next(15, 50);
            SpecialAttackName = specialAttackName;
        }

        public override void ShowInfo()
        {
            Console.WriteLine($"{ClassLabel} - {Name}. Здоровье - {Health}. Мана - {_mana}. Урон - {Damage}. Спецприем - {SpecialAttackName}");
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
                return Damage * Random.Next(3, 8);
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
            ClassLabel = "Вор";
            Name = name;
            Health = Random.Next(70, 120);
            Damage = Random.Next(15, 35);
            _dodgeChance = Random.Next(20, 50);
            SpecialAttackName = specialAttackName;
        }

        public override void ShowInfo()
        {
            Console.WriteLine($"{ClassLabel} - {Name}. Здоровье - {Health}. Уклонение - {_dodgeChance}. Урон - {Damage}. Спецприем - {SpecialAttackName}"); 
        }

        public override int SpecialAttack()
        {
            return Damage * Random.Next(3, 8);
        }

        private bool IsDodged()
        {
            int chance = Random.Next(101);
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
            ClassLabel = "Варвар";
            Name = name;
            Health = Random.Next(150, 220);
            Damage = Random.Next(25, 50);
            SpecialAttackName = specialAttackName;
        }

        public override void ShowInfo()
        {
            Console.WriteLine($"{ClassLabel} - {Name}. Здоровье - {Health}. Урон - {Damage}. Спецприем - {SpecialAttackName}");
        }

        public override int SpecialAttack()
        {
            Random = new Random();
            return Damage * Random.Next(2, 5);
        }

        public override void TakeDamage(int damage)
        {
            Health -= damage;
        }
    }
}
