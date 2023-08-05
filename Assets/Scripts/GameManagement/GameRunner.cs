using UnityEngine;

namespace GameManagement {
    public class GameRunner : MonoBehaviour
    {
        void Start()
        {
            GameStateMachine.SetState<GameStateMachine.PreGame>();
        }

        void Update()
        {
            GameStateMachine.UpdateCurrentState();
        }
    }
}
