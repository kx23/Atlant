using UnityEngine;

namespace Atlant
{
    public class BaseState
    {

        protected Core _core;

        protected CharacterController _characterController;
        protected StateMachine _stateMachine;
        protected PlayerData _playerData;
        protected bool _isExitingState;

        protected bool _isAnmationFinished;
        

        protected float startTime;



        private string _animBoolName;




        public BaseState(CharacterController characterController, StateMachine stateMachine, PlayerData playerData, string animBoolName)
        {
            this._stateMachine = stateMachine;
            this._characterController = characterController;
            this._playerData = playerData;
            this._animBoolName = animBoolName;
            _core = characterController.core;

        }

        public virtual void Enter() 
        {
            DoChecks();
            _characterController.animator.SetBool(_animBoolName, true);
            startTime = Time.time;
            Debug.Log(_animBoolName);
            _isAnmationFinished = false;
            _isExitingState = false;
        }
        public virtual void UpdateLogic() { }
        public virtual void UpdatePhysics() 
        {
            DoChecks();
        }
        public virtual void DoChecks() { }
        public virtual void Exit() 
        {
            _characterController.animator.SetBool(_animBoolName, false);
            _isExitingState = true;
        }
        public virtual void AnimationTrigger() { }

        public virtual void AnimationFinishTrigger() => _isAnmationFinished = true;
    }
}

