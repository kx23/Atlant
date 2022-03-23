using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atlant
{
    public class PlayerGroundedState : BaseState
    {
        protected int _xInput;
        private bool _grabInput;
        private bool _isGrounded;
        private bool _jumpInput;
        private bool _isTouchingWall;



        public PlayerGroundedState(CharacterController characterController, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(characterController, stateMachine, playerData, animBoolName)
        {
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

            _characterController.jumpState.ResetAmountOfJumpsLeft();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();

            _xInput = _characterController.inputHandler.normInputX;
            _jumpInput = _characterController.inputHandler.jumpInput;
            _grabInput= _characterController.inputHandler.grabInput;

            if (_jumpInput && _characterController.jumpState.CanJump())
            {
                _stateMachine.ChangeState(_characterController.jumpState);
            }
            else if (!_isGrounded)
            {
                _characterController.inAirState.StartCoyoteTime();
                _stateMachine.ChangeState(_characterController.inAirState);
            }else if(_isTouchingWall&&_grabInput)
            {
                _stateMachine.ChangeState(_characterController.wallGrabState);
            }
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
        }
    }
}