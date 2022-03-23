using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atlant
{
    public class PlayerLandState : PlayerGroundedState
    {
        public PlayerLandState(CharacterController characterController, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(characterController, stateMachine, playerData, animBoolName)
        {
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();
            if(!_isExitingState)
            {
                if (_xInput != 0)
                {
                    _stateMachine.ChangeState(_characterController.moveState);
                }
                else if (_isAnmationFinished)
                {
                    _stateMachine.ChangeState(_characterController.idleState);
                }
            }
            
        }

    }
}
