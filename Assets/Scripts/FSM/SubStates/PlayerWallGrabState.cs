using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atlant
{
    public class PlayerWallGrabState : PlayerTouchingWallState
    {
        private Vector2 _holdPosition;
        public PlayerWallGrabState(CharacterController characterController, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(characterController, stateMachine, playerData, animBoolName)
        {
        }

        public override void AnimationFinishTrigger()
        {
            base.AnimationFinishTrigger();
        }

        public override void AnimationTrigger()
        {
            base.AnimationTrigger();
        }

        public override void DoChecks()
        {
            base.DoChecks();
        }

        public override void Enter()
        {
            base.Enter();

            _holdPosition = _characterController.transform.position;
            HoldPosition();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();

           

            if (!_isExitingState)
            {
                HoldPosition();
                if (_yInput > 0)
                {
                    _stateMachine.ChangeState(_characterController.wallClimbState);
                }
                else if (_yInput < 0 || !_grabInput)
                {
                    _stateMachine.ChangeState(_characterController.wallSlideState);
                }
            }


        }

        private void HoldPosition()
        {
            _characterController.transform.position = _holdPosition;
            _characterController.core.movement.SetVelocityX(0f);
            _characterController.core.movement.SetVelocityY(0f);
        }
        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
        }
    }
}