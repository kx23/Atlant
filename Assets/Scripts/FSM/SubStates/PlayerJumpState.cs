using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atlant
{
    public class PlayerJumpState : PlayerAbilityState
    {
        private int _amountOfJumpsLeft;

        public PlayerJumpState(CharacterController characterController, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(characterController, stateMachine, playerData, animBoolName)
        {
            _amountOfJumpsLeft = playerData.amountOfJumps;
        }

        public override void Enter()
        {
            base.Enter();
            _characterController.inputHandler.UseJumpUnput();
            _characterController.core.movement.SetVelocityY(_playerData.jumpVelocity);
            _isAbilityDone = true;
            //DecreaseAmountOfJumpsLeft();
            _amountOfJumpsLeft--;
            _characterController.inAirState.SetIsJumping();

        }

        public bool CanJump()
        {
            return _amountOfJumpsLeft > 0;
        }

        public void ResetAmountOfJumpsLeft() => _amountOfJumpsLeft = _playerData.amountOfJumps;

        public void DecreaseAmountOfJumpsLeft() => _amountOfJumpsLeft--;
    }
}
