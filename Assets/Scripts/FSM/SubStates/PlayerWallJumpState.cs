using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atlant
{
    public class PlayerWallJumpState : PlayerAbilityState
    {
        private int _wallJumpDirection;
        public PlayerWallJumpState(CharacterController characterController, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(characterController, stateMachine, playerData, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _characterController.inputHandler.UseJumpUnput();
            _characterController.jumpState.ResetAmountOfJumpsLeft();
            Debug.Log(_wallJumpDirection);
            _characterController.SetVelocity(_playerData.wallJumpVelocity, _playerData.wallJumpAngle, _wallJumpDirection);
            _characterController.CheckIfShoudFlip(_wallJumpDirection);
            _characterController.jumpState.DecreaseAmountOfJumpsLeft();
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();

            _characterController.animator.SetFloat("yVelocity", _characterController.currentVelocity.y);
            _characterController.animator.SetFloat("xVelocity", Mathf.Abs(_characterController.currentVelocity.x));

            if (Time.time >= startTime + _playerData.wallJumpTime)
            {
                _isAbilityDone = true;
            }

        }

        public void DetermineWallJumpDirection(bool isTouchingWall)
        {
            
            if (isTouchingWall)
            {
                _wallJumpDirection = -_characterController.facingDirection;
            }
            else
            {
                _wallJumpDirection = _characterController.facingDirection;
            }
            
        }
    }
}