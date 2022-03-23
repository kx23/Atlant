using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atlant
{
    public class PlayerInAirState : BaseState
    {
        private int _xInput;
        private bool _isGrounded;
        private bool _isTouchingWall;
        private bool _oldIsTouchingWall;
        private bool _oldIsTouchingWallBack;
        private bool _isTouchingWallBack;
        private bool _jumpInput;
        private bool _jumpInputStop;
        private bool _grabInput;
        private bool _coyoteTime;
        private bool _wallCoyoteTime;
        private bool _isJumping;
        private float _startWallCoyoteTime;


        public PlayerInAirState(CharacterController characterController, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(characterController, stateMachine, playerData, animBoolName)
        {
        }

        public override void DoChecks()
        {
            base.DoChecks();

            _oldIsTouchingWall = _isTouchingWall;
            _oldIsTouchingWallBack = _isTouchingWallBack;
            _isGrounded = _characterController.CheckIfGrounded();
            _isTouchingWall = _characterController.CheckIfTouchingWall();
            _isTouchingWallBack = _characterController.CheckIfTouchingWallBack();

            if (!_wallCoyoteTime && !_isTouchingWall && !_isTouchingWallBack && (_oldIsTouchingWall || _oldIsTouchingWallBack))
            {
                StartWallCoyoteTime();
            }
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

            CheckCoyoteTime();
            CheckWallCoyoteTime();

            _xInput = _characterController.inputHandler.normInputX;
            _jumpInput = _characterController.inputHandler.jumpInput;
            _jumpInputStop = _characterController.inputHandler.jumpInputStop;
            _grabInput = _characterController.inputHandler.grabInput;

            CheckJumpMultiplayer();

            if (_isGrounded && _characterController.currentVelocity.y < Mathf.Epsilon)
            {
                _stateMachine.ChangeState(_characterController.landState);
            }
            else if (_jumpInput && (_isTouchingWall||_isTouchingWallBack||_wallCoyoteTime))
            {
                StopWallCoyoteTime();
                _isTouchingWall = _characterController.CheckIfTouchingWall();
                _characterController.wallJumpState.DetermineWallJumpDirection(_isTouchingWall);
                _stateMachine.ChangeState(_characterController.wallJumpState);
            }
            else if (_jumpInput && _characterController.jumpState.CanJump())
            {
                
                _stateMachine.ChangeState(_characterController.jumpState);
            }
            else if (_isTouchingWall && _grabInput)
            {
                _stateMachine.ChangeState(_characterController.wallGrabState);
            }
            else if (_isTouchingWall && _xInput == _characterController.facingDirection && _characterController.currentVelocity.y < 0)
            {
                _stateMachine.ChangeState(_characterController.wallSlideState);
            }
            else
            {
                _characterController.CheckIfShoudFlip(_xInput);
                _characterController.SetVelocityX(_playerData.movementVelocity * _xInput);

                _characterController.animator.SetFloat("yVelocity", _characterController.currentVelocity.y);
                _characterController.animator.SetFloat("xVelocity", Mathf.Abs(_characterController.currentVelocity.x));
            }
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
        }

        private void CheckJumpMultiplayer()
        {
            if (_isJumping)
            {
                if (_jumpInputStop)
                {
                    _characterController.SetVelocityY(_characterController.currentVelocity.y * _playerData.variableJumpHeightMultiplayer);
                    _isJumping = false;
                }
                else if (_characterController.currentVelocity.y <= 0f)
                {
                    _isJumping = false;
                }
            }
        }
        private void CheckCoyoteTime()
        {
            if (_coyoteTime && Time.time > startTime + _playerData.coyoteTime)
            {
                _coyoteTime = false;
                _characterController.jumpState.DecreaseAmountOfJumpsLeft();
            }
        }
        private void CheckWallCoyoteTime()
        {
            if (_wallCoyoteTime && Time.time > _startWallCoyoteTime + _playerData.coyoteTime)
            {
                _wallCoyoteTime = false;
                _characterController.jumpState.DecreaseAmountOfJumpsLeft();
            }
        }
        public void StartCoyoteTime() => _coyoteTime = true;
        public void StartWallCoyoteTime() 
        {
            _startWallCoyoteTime = Time.time;
            _wallCoyoteTime = true;
        } 
        public void StopWallCoyoteTime() => _wallCoyoteTime = false;
        public void SetIsJumping() => _isJumping = true;

    }
}