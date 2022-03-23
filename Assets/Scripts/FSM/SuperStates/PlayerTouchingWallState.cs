using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atlant
{
    public class PlayerTouchingWallState : BaseState
    {
        protected int _xInput;
        protected int _yInput;
        protected bool _grabInput;
        protected bool _jumpInput;
        protected bool _isGrounded;
        protected bool _isTouchingWall;


        public PlayerTouchingWallState(CharacterController characterController, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(characterController, stateMachine, playerData, animBoolName)
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
            _isGrounded = _characterController.CheckIfGrounded();
            _isTouchingWall = _characterController.CheckIfTouchingWall();
           
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();

            _xInput = _characterController.inputHandler.normInputX;
            _yInput = _characterController.inputHandler.normInputY;
            _grabInput = _characterController.inputHandler.grabInput;
            _jumpInput = _characterController.inputHandler.jumpInput;


            if (_jumpInput)
            {
                
                _characterController.wallJumpState.DetermineWallJumpDirection(_isTouchingWall);
                _stateMachine.ChangeState(_characterController.wallJumpState);
            }
            else if (_isGrounded&&!_grabInput)
            {
                _stateMachine.ChangeState(_characterController.idleState);
            }
            else if (!_isTouchingWall || (_xInput != _characterController.facingDirection&&!_grabInput))
            {
                _stateMachine.ChangeState(_characterController.inAirState);
            }
        }


        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
        }
    }
}
