using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atlant
{
    public class PlayerCrouchIdleState : PlayerGroundedState
    {
        public PlayerCrouchIdleState(CharacterController characterController, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(characterController, stateMachine, playerData, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _characterController.SetVelocityZero();
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
                if (_xInput != 0)
                {
                    _stateMachine.ChangeState(_characterController.crouchMoveState);
                }
                else if (_yInput != -1&&!_isTouchingCeiling)
                {
                    _stateMachine.ChangeState(_characterController.idleState);
                }
            }
        }
    }
}