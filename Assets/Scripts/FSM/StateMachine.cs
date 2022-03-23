using UnityEngine;

namespace Atlant
{
    public class StateMachine 
    {
        public BaseState currentState { get; private set; }

        public void Initialize(BaseState initialState) 
        {
            currentState = initialState;
            currentState.Enter();
        }


        public void ChangeState(BaseState newState)
        {
            currentState.Exit();
            currentState = newState;
            currentState.Enter();
        }
    }
}

