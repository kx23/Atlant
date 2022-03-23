using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atlant
{
    public class PlayerWallClimbState : PlayerTouchingWallState
    {
        public PlayerWallClimbState(CharacterController characterController, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(characterController, stateMachine, playerData, animBoolName)
        {
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();

            
            if (!_isExitingState)
            {
                _characterController.SetVelocityY(_playerData.wallClimbVelocity);
                if (_yInput != 1)
                {
                    _stateMachine.ChangeState(_characterController.wallGrabState);
                }
            }

        }
    }
}
