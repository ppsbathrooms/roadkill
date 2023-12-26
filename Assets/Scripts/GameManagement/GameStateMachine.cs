using System;
using System.Collections.Generic;
using UnityEngine;
using Vehicles.Controllers;

namespace GameManagement {
    public static class GameStateMachine {
        private static GameState _currentState;
        private static readonly Dictionary<Type, GameState> _stateInstances = new();

        public static void SetState<T>() where T : GameState, new() {
            if (_currentState?.GetType() == typeof(T))
                throw new Exception($"Game state already set to {typeof(T)}");
            
            _currentState?.End(); // End previous state
            
            if (_stateInstances.TryGetValue(typeof(T), out var state))
                _currentState = state;
            else {
                var newState = new T();
                _stateInstances.Add(typeof(T), newState);
                _currentState = newState;
            }
            
            _currentState.Start(); // Start new state
        }
        
        public static void UpdateCurrentState() {
            _currentState.Update();
        }

        static GameStateMachine() {
            SetState<None>();
        }

        public interface GameState {
            public void Start();
            public void Update();
            public void End();
        }

        public class None : GameState {
            public void Start() { }
            public void Update() { }
            public void End() { }
        }
        
        public class PreGame : GameState {
            public void Start() { UIManager.Instance.StartGameButton.SetActive(true); }
            public void Update() { }

            public void End() { UIManager.Instance.StartGameButton.SetActive(false); }
        }
        
        public class Gameplay : GameState {
            public void Start() {
                SphereObjectSpawner.Instance.StartSpawning();
                VehicleController.Instance.gameObject.SetActive(true);
                UIManager.Instance.EndGameButton.SetActive(true);
            }
            
            public void Update() { }

            public void End() {
                SphereObjectSpawner.Instance.StopSpawning();
                VehicleController.Instance.gameObject.SetActive(false);
                UIManager.Instance.EndGameButton.SetActive(false);
                foreach (Transform chicken in Settings.instance.chickenContainer) {
                    UnityEngine.Object.Destroy(chicken.gameObject);
                }
                foreach (Transform coop in Settings.instance.coopContainer) {
                    UnityEngine.Object.Destroy(coop.gameObject);
                }
            }
        }

        public class EndScreen : GameState {
            public void Start() { UIManager.Instance.GameOverScreen.SetActive(true); }
            public void Update() { }
            public void End() { }
        }
    }
}