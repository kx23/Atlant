using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Atlant
{
    public class PlayerMoveState : PlayerGroundedState
    {
        public PlayerMoveState(CharacterController characterController, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(characterController, stateMachine, playerData, animBoolName)
        {
        }

        public override void DoChecks()
        {
            base.DoChecks();
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
            _characterController.CheckIfShoudFlip(_xInput);
            _characterController.SetVelocityX(_playerData.movementVelocity * _xInput);

            if (_xInput == 0f&&!_isExitingState)
            {
                _stateMachine.ChangeState(_characterController.idleState);
            }
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
        }


    }
}