using GameManagement;
using UnityEngine;

namespace UI {
    public class StartEndButtonListener : MonoBehaviour
    {
        public void StartGame() {
            GameStateMachine.SetState<GameStateMachine.Gameplay>();
        }
    
        public void EndGame() {
            GameStateMachine.SetState<GameStateMachine.EndScreen>();
        }
    }
}
