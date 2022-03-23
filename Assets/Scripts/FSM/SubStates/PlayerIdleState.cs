using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Atlant
{
    public class PlayerIdleState : PlayerGroundedState
    {
        public PlayerIdleState(CharacterController characterController, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(characterController, stateMachine, playerData, animBoolName)
        {
        }

        public override void DoChecks()
        {
            base.DoChecks();
        }

        public override void Enter()
        {
            base.Enter();
            _characterController.SetVelocityX(0f);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();

            if (_xInput != 0f&&!_isExitingState)
            {
                _stateMachine.ChangeState(_characterController.moveState);
            }
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
        }
    }
}
