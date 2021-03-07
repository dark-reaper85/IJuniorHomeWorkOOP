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
            BattleGround battleGround = new BattleGround();

            battleGround.OrganizeBattle();
        }
    }

    class BattleGround
    {
        public Random _random = new Random();
        private Gladiator _firstGladiator;
        private Gladiator _secondGladiator;
        private Team _team1;
        private Team _team2;
        private bool _allTeamsCreated;
        private bool _allTeamsLive = true;
        
        private List<Gladiator> _gladiators = new List<Gladiator>
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

        private List<Team> _teams = new List<Team>();

        public void OrganizeBattle()
        {
            ShowGladiators();
            while (_allTeamsLive)
            {
                if (_allTeamsCreated == false)
                {
                    CreateTeams();
                }
                else 
                {
                    ShowTeams();
                    TakeTeamsToFight();
                    Fight(_team1, _team2);
                }
                Thread.Sleep(500);
            }
        }

        private bool AllTeamsAreLive() 
        {
            if (_allTeamsCreated && _teams.Count > 1)
            {
                return true;
            }
            else 
            {
                Console.WriteLine($"{_teams[0].GetTeamLabel()} Абсолютный Чемпион");
                Console.ReadKey();
                _allTeamsLive = false;
                return false;
            }
        }

        public void ShowGladiators()
        {
            for (int i = 0; i < _gladiators.Count; i++)
            {
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
            Gladiator gladiator1 = team1.GetRandomGladiator();
            Gladiator gladiator2 = team2.GetRandomGladiator();
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
            switch (_random.Next(1, 3))
            {
                case 1:
                    Console.WriteLine($"\n{gladiator1.Name} из команды {team1.GetTeamLabel()} наносит урон {gladiator1.Attack()} по {gladiator2.Name} из команды {team2.GetTeamLabel()}");
                    gladiator2.TakeDamage(gladiator1.Attack());
                    Console.WriteLine($"Здоровье {gladiator2.Name} из команды {team2.GetTeamLabel()}: {gladiator2.Health}");
                    
                    team2.IsGladiatorDead(gladiator2);
                    CheckWinner(team2);
                    
                    break;
                case 2:
                    Console.WriteLine($"\n{gladiator1.Name} из команды {team1.GetTeamLabel()} применяет {gladiator1.SpecialAttackName} и наносит урон {gladiator1.SpecialAttack()} по {gladiator2.Name} из команды {team2.GetTeamLabel()}");
                    gladiator2.TakeDamage(gladiator1.SpecialAttack());
                    Console.WriteLine($"Здоровье {gladiator2.Name}: {gladiator2.Health}");
                    
                    team2.IsGladiatorDead(gladiator2);
                    CheckWinner(team2);
                    break;
            }
        }

        private void CheckWinner(Team team) 
        {
            if (team.IsTeamLeaves())
            {
                Console.WriteLine($"Команда {team.GetTeamLabel()} проиграла");
                _teams.Remove(team);
                AllTeamsAreLive();
            }
        }

        private void CreateTeams() 
        {
            Console.WriteLine("Сколько команд вы хотите создать?");
            string userInput = Console.ReadLine();

            if (int.TryParse(userInput, out int teamsCount))
            {
                if (teamsCount > 1)
                {
                    for (int i = 0; i < teamsCount; i++)
                    {
                        Console.WriteLine("Введите название команды:");
                        userInput = Console.ReadLine();

                        _teams.Add(FillTeam(userInput, teamsCount));
                    }

                    Console.WriteLine("Команды сформированы. Теперь их можно стравить друг с другом");
                    _allTeamsCreated = true;
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Необходимо создать минимум 2 команды");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("Ошибка ввода.");
                Console.ReadKey();
            }
        }

        private Team FillTeam(string teamLabel ,int teamsCount) 
        {
            List<Gladiator> gladiators = new List<Gladiator>();

            for (int i = 0; i < _gladiators.Count / teamsCount; i++)
            {
                int gladiatorNumber = _random.Next(0, _gladiators.Count);

                gladiators.Add(_gladiators[gladiatorNumber]);

                _gladiators.RemoveAt(gladiatorNumber);
            }

            Team team = new Team(teamLabel, gladiators);
            return team;
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
        private List<Gladiator> _gladiatorsInTeam;
        private string _teamLabel;

        public Team(string teamLabel, List<Gladiator> gladiators)
        {
            _teamLabel = teamLabel;
            _gladiatorsInTeam = gladiators;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Команда: {_teamLabel}");
            foreach (var gladiator in _gladiatorsInTeam)
            {
                gladiator.ShowInfo();
            }
        }

        public int GetGladiatorsNumber() 
        {
            return _gladiatorsInTeam.Count;
        }

        public Gladiator GetRandomGladiator()
        {
            return _gladiatorsInTeam[_random.Next(0, _gladiatorsInTeam.Count)];
        }

        public string GetTeamLabel() 
        {
            return _teamLabel;
        }

        public void IsGladiatorDead(Gladiator gladiator) 
        {
            if (gladiator.Health <= 0)
            {
                Console.WriteLine($"{gladiator.Name} умер и выбывает");
                _gladiatorsInTeam.Remove(gladiator);
            }  
        }

        public bool IsTeamLeaves() 
        {
            if (GetGladiatorsNumber() == 0)
            {
                return true;
            }
            return false;
        }
    }

    abstract class Gladiator
    {
        protected int Damage;
        protected Random _random = new Random();

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
            Health = _random.Next(100, 150);
            Damage = _random.Next(10, 25);
            _armor = _random.Next(5, 40);
            SpecialAttackName = specialAttackName;
        }

        public override void ShowInfo()
        {
            Console.WriteLine($"{ClassLabel} - {Name}. Здоровье - {Health}. Броня - {_armor}. Урон - {Damage}. Спецприем - {SpecialAttackName}");
        }

        public override int SpecialAttack()
        {
            return Damage * _random.Next(2, 5);
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
            Health = _random.Next(100, 120);
            Damage = _random.Next(15, 40);
            SpecialAttackName = specialAttackName;
        }

        public override void ShowInfo()
        {
            Console.WriteLine($"{ClassLabel} - {Name}. Здоровье - {Health}. Урон - {Damage}. Спецприем - {SpecialAttackName}"); 
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
            ClassLabel = "Маг";
            Name = name;
            Health = _random.Next(70, 120);
            Damage = _random.Next(10, 25);
            _mana = _random.Next(120, 150);
            _specialAttackManaCost = _random.Next(15, 50);
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
            ClassLabel = "Вор";
            Name = name;
            Health = _random.Next(70, 120);
            Damage = _random.Next(15, 35);
            _dodgeChance = _random.Next(20, 50);
            SpecialAttackName = specialAttackName;
        }

        public override void ShowInfo()
        {
            Console.WriteLine($"{ClassLabel} - {Name}. Здоровье - {Health}. Уклонение - {_dodgeChance}. Урон - {Damage}. Спецприем - {SpecialAttackName}"); 
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
            ClassLabel = "Варвар";
            Name = name;
            Health = _random.Next(150, 220);
            Damage = _random.Next(25, 50);
            SpecialAttackName = specialAttackName;
        }

        public override void ShowInfo()
        {
            Console.WriteLine($"{ClassLabel} - {Name}. Здоровье - {Health}. Урон - {Damage}. Спецприем - {SpecialAttackName}");
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
