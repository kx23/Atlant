using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atlant
{
    public class PlayerAbilityState : BaseState
    {

        protected bool _isAbilityDone;
        private bool _isGrounded;
        public PlayerAbilityState(CharacterController characterController, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(characterController, stateMachine, playerData, animBoolName)
        {
        }

        public override void DoChecks()
        {
            base.DoChecks();

            _isGrounded = _core.collisionSenses.ground;
        }

        public override void Enter()
        {
            base.Enter();
            _isAbilityDone = false;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();

            if (_isAbilityDone)
            {
                if (_isGrounded && _characterController.core.movement.currentVelocity.y < Mathf.Epsilon)
                {
                    _stateMachine.ChangeState(_characterController.idleState);
                }
                else
                {
                    _stateMachine.ChangeState(_characterController.inAirState);
                }
            }
                

        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
        }
    }
}