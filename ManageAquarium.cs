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
        private int _loopIterationNumber;
        private int _maxFishCount;
        private bool _isWorking;
        private List<Fish> _fishes;

        public Aquarium() 
        {
            _loopIterationNumber = 0;
            _maxFishCount = 6;
            _isWorking = true;
            _fishes = new List<Fish>();
        }

        public void ManageAquarium() 
        {
            while (_isWorking)
            {
                Console.Clear();
                ShowAllFishes();

                ControlFishesLives();

                ControlAquarium();

                _loopIterationNumber++;
                Thread.Sleep(200);
            }
        }

        private void TryAddFish() 
        {
            if (_fishes.Count <= _maxFishCount)
            {
                _fishes.Add(new Fish(_loopIterationNumber));
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
                    fish.ShowInfo(_loopIterationNumber);
                }
            }
        }

        private void ControlFishesLives() 
        {
            foreach (var fish in _fishes)
            {
                fish.CheckStates(_loopIterationNumber);
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

        public Fish(int loopIterationNumber) 
        {
            _fishNumber = _lastFishNumber;
            _birthTime = loopIterationNumber;
            _lifetime = _random.Next(10, 20);
            IsAlive = true;
            _lastFishNumber++;
        }

        public void ShowInfo(int loopIterationNumber) 
        {
            if (IsAlive)
                Console.WriteLine($"Рыба №{_fishNumber}: возраст {CheckAge(loopIterationNumber)}. РЫба жива.");
            else
                Console.WriteLine($"Рыба №{_fishNumber} - мертва.");
        }

        private int CheckAge(int loopIterationNumber) 
        {
            return loopIterationNumber - _birthTime;
        }

        public void CheckStates(int loopIterationNumber) 
        {
            _age = CheckAge(loopIterationNumber);
            if (_age >= _lifetime)
            {
                IsAlive = false;
            }
        }
    }
}
