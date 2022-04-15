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
            if (!_isExitingState)
            {
                if (_xInput != 0f)
                {
                    _stateMachine.ChangeState(_characterController.moveState);
                }
                else if (_yInput == -1)
                {
                    _stateMachine.ChangeState(_characterController.crouchIdleState);
                }
            }

        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
        }
    }
}
