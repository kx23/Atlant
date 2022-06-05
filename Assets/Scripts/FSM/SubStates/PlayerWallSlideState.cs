using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atlant
{
    public class PlayerWallSlideState : PlayerTouchingWallState
    {
        public PlayerWallSlideState(CharacterController characterController, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(characterController, stateMachine, playerData, animBoolName)
        {
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();
            if (!_isExitingState)
            {
                _characterController.core.movement.SetVelocityY(-_playerData.wallSlideVelocity);

                if (_grabInput && _yInput == 0)
                {
                    _stateMachine.ChangeState(_characterController.wallGrabState);
                }
            }

        }
    }
}