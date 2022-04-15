using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atlant
{
    public class PlayerGroundedState : BaseState
    {
        protected int _xInput;
        protected int _yInput;

        protected bool _isTouchingCeiling;

        private bool _grabInput;
        private bool _isGrounded;
        private bool _jumpInput;
        private bool _isTouchingWall;
        private bool _isTouchingLedge;
        private bool _dashInput;



        public PlayerGroundedState(CharacterController characterController, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(characterController, stateMachine, playerData, animBoolName)
        {
        }

        public override void DoChecks()
        {
            base.DoChecks();

            _isTouchingCeiling = _characterController.CheckForCeiling();
            _isGrounded = _characterController.CheckIfGrounded();
            _isTouchingWall = _characterController.CheckIfTouchingWall();
            _isTouchingLedge = _characterController.CheckIfTouchingLedge();
        }

        public override void Enter()
        {
            base.Enter();

            _characterController.jumpState.ResetAmountOfJumpsLeft();
            _characterController.dashState.ResetCanDash();
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
            _jumpInput = _characterController.inputHandler.jumpInput;
            _grabInput= _characterController.inputHandler.grabInput;
            _dashInput= _characterController.inputHandler.dashInput;

            if (_jumpInput && _characterController.jumpState.CanJump())
            {
                _stateMachine.ChangeState(_characterController.jumpState);
            }
            else if (!_isGrounded)
            {
                _characterController.inAirState.StartCoyoteTime();
                _stateMachine.ChangeState(_characterController.inAirState);
            }
            else if(_isTouchingWall&&_grabInput && _isTouchingLedge)
            {
                _stateMachine.ChangeState(_characterController.wallGrabState);
            }
            else if (_dashInput && _characterController.dashState.CheckIfCanDash()&&!_isTouchingCeiling)
            {
                _stateMachine.ChangeState(_characterController.dashState);
            }
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
        }
    }
}