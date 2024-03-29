using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atlant
{
    public class PlayerTouchingWallState : BaseState
    {
        protected int _xInput;
        protected int _yInput;
        protected bool _grabInput;
        protected bool _jumpInput;
        protected bool _isGrounded;
        protected bool _isTouchingWall;
        protected bool _isTouchingLedge;


        public PlayerTouchingWallState(CharacterController characterController, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(characterController, stateMachine, playerData, animBoolName)
        {
        }

        public override void AnimationFinishTrigger()
        {
            base.AnimationFinishTrigger();
        }

        public override void AnimationTrigger()
        {
            base.AnimationTrigger();
        }

        public override void DoChecks()
        {
            base.DoChecks();
            _isGrounded = _core.collisionSenses.ground;
            _isTouchingWall = _core.collisionSenses.wallFront;
            _isTouchingLedge= _core.collisionSenses.ledge;

            if (_isTouchingWall && !_isTouchingLedge)
            {
                _characterController.ledgeClimbState.SetDetectedPos(_characterController.transform.position);
            }

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

            _xInput = _characterController.inputHandler.normInputX;
            _yInput = _characterController.inputHandler.normInputY;
            _grabInput = _characterController.inputHandler.grabInput;
            _jumpInput = _characterController.inputHandler.jumpInput;


            if (_jumpInput)
            {
                
                _characterController.wallJumpState.DetermineWallJumpDirection(_isTouchingWall);
                _stateMachine.ChangeState(_characterController.wallJumpState);
            }
            else if (_isGrounded&&!_grabInput)
            {
                _stateMachine.ChangeState(_characterController.idleState);
            }
            else if (!_isTouchingWall || (_xInput != _characterController.core.movement.facingDirection&&!_grabInput))
            {
                _stateMachine.ChangeState(_characterController.inAirState);
            }
            else if (_isTouchingWall && !_isTouchingLedge)
            {
                _stateMachine.ChangeState(_characterController.ledgeClimbState);
            }
        }


        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
        }
    }
}
