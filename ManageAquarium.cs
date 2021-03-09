﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AquariumManager
{
    class ManageAquarium
    {
        static void Main(string[] args)
        {
            Aquarium newAquarium = new Aquarium();
            newAquarium.ManageAquarium();
        }
    }

    class Aquarium 
    {
        private int _timer = 0;
        private int _maxFishCount = 6;
        private bool _isWorking = true;
        private List<Fish> _fishes = new List<Fish>();

        public void ManageAquarium() 
        {
            while (_isWorking)
            {
                Console.Clear();
                ShowAllFishes();

                ControlFishesLives();

                ControlAquarium();

                _timer++;
                Thread.Sleep(200);
            }
        }

        private void TryAddFish() 
        {
            if (_fishes.Count <= _maxFishCount)
            {
                _fishes.Add(new Fish(_timer));
            }
            else
            {
                Console.SetCursorPosition(0, _fishes.Count + 6);
                Console.WriteLine("Аквариум заполнен");
            }
        }

        private void DeleteDeadFish() 
        {
            for (int i = 0; i < _fishes.Count; i++)
            {
                if (_fishes[i].IsAlive == false)
                {
                    _fishes.RemoveAt(i);
                    break;
                }
            }
        }

        private void ShowAllFishes() 
        {
            if (_fishes.Count == 0)
            {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Аквариум пуст. Добавьте повых рыб.");
            }
            else
            {
                foreach (var fish in _fishes)
                {
                    fish.ShowInfo(_timer);
                }
            }
        }

        private void ControlFishesLives() 
        {
            foreach (var fish in _fishes)
            {
                fish.CheckStates(_timer);
            }
        }

        private void ControlAquarium() 
        {
            Console.SetCursorPosition(0, _fishes.Count + 2);
            Console.WriteLine("A - Добавить рыб в аквариум");
            Console.WriteLine("D - Убрать мертвую рыбу из аквариума");
            Console.WriteLine("E - Выход из управления аквариумом");
            
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.A:
                        TryAddFish();
                        break;
                    case ConsoleKey.D:
                        DeleteDeadFish();
                        break;
                    case ConsoleKey.E:
                        _isWorking = false;
                        break;
                    default:
                        Console.SetCursorPosition(0, _fishes.Count + 5);
                        Console.WriteLine("Неизвестная команда");
                        break;
                }
            }
        }
    }

    class Fish 
    {
        private static int _lastFishNumber;
        private int _fishNumber;
        private int _age;
        private int _birthTime;
        private int _lifetime;
        private Random _random = new Random();
        public bool IsAlive { get; private set; }

        public Fish(int timer) 
        {
            _fishNumber = _lastFishNumber;
            _birthTime = timer;
            _lifetime = _random.Next(10, 20);
            IsAlive = true;
            _lastFishNumber++;
        }

        public void ShowInfo(int timer) 
        {
            if (IsAlive)
                Console.WriteLine($"Рыба №{_fishNumber}: возраст {CheckAge(timer)}. РЫба жива.");
            else
                Console.WriteLine($"Рыба №{_fishNumber} - мертва.");
        }

        private int CheckAge(int timer) 
        {
            return timer - _birthTime;
        }

        public void CheckStates(int timer) 
        {
            _age = CheckAge(timer);
            if (_age >= _lifetime)
            {
                IsAlive = false;
            }
        }
    }
}
