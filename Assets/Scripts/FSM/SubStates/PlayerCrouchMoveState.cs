using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atlant
{
    public class PlayerCrouchMoveState : PlayerGroundedState
    {
        public PlayerCrouchMoveState(CharacterController characterController, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(characterController, stateMachine, playerData, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _characterController.SetColliderHeight(_playerData.crouchColliderHeight);
        }

        public override void Exit()
        {
            base.Exit();

            _characterController.SetColliderHeight(_playerData.standColliderHeight);
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();
            if (!_isExitingState)
            {
                _characterController.core.movement.SetVelocityX(_playerData.crouchMovementVelocity *_characterController.core.movement.facingDirection);
                _characterController.core.movement.CheckIfShoudFlip(_xInput);


                if (_xInput == 0)
                {
                    _stateMachine.ChangeState(_characterController.crouchIdleState);

                }
                else if (_yInput != -1&&!_isTouchingCeiling)
                {
                    _stateMachine.ChangeState(_characterController.moveState);
                }
            }
        }
    }
}