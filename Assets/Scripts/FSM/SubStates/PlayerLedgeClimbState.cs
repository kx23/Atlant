using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atlant
{
    public class PlayerLedgeClimbState : BaseState
    {
        private Vector2 _detectedPos;
        private Vector2 _cornerPos;
        private Vector2 _startPos;
        private Vector2 _stopPos;

        private bool _isHanging;
        private bool _isClimbing;
        private bool _jumpInput;
        private bool _isTouchingCeiling;

        private int _xInput;
        private int _yInput;
        private Vector2 workspace;

        public PlayerLedgeClimbState(CharacterController characterController, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(characterController, stateMachine, playerData, animBoolName)
        {
        }

        public override void AnimationFinishTrigger()
        {
            base.AnimationFinishTrigger();
            _characterController.animator.SetBool("ledgeClimb", false);
        }

        public override void AnimationTrigger()
        {
            base.AnimationTrigger();
            _isHanging = true;
        }

        public override void Enter()
        {
            base.Enter();
            _characterController.core.movement.SetVelocityZero();
            _characterController.transform.position = _detectedPos;
            _cornerPos = DetermineCornerPosition();

            _startPos.Set(_cornerPos.x-(_characterController.core.movement.facingDirection*_playerData.startOffset.x),_cornerPos.y-_playerData.startOffset.y);
            _stopPos.Set(_cornerPos.x+(_characterController.core.movement.facingDirection*_playerData.stopOffset.x),_cornerPos.y+_playerData.stopOffset.y);

            _characterController.transform.position = _startPos;
        }

        public override void Exit()
        {
            base.Exit();
            _isHanging = false;

            if (_isClimbing)
            {
                _characterController.transform.position = _stopPos;
                _isClimbing = false;
            }
        }

        public void SetDetectedPos(Vector2 pos) => _detectedPos = pos;

        private void CheckForSpace()
        {
            _isTouchingCeiling = Physics2D.Raycast(_cornerPos+(Vector2.up*0.015f)+(Vector2.right*_characterController.core.movement.facingDirection * 0.015f),Vector2.up,_playerData.standColliderHeight,_core.collisionSenses.groundLayer);
            _characterController.animator.SetBool("isTouchingCeiling", _isTouchingCeiling);
        }

        private Vector2 DetermineCornerPosition()
        {
            //исправить
            RaycastHit2D xHit = Physics2D.Raycast(_core.collisionSenses.wallChecker.position, Vector2.right * _core.movement.facingDirection, _core.collisionSenses.wallCheckDistance, _core.collisionSenses.groundLayer);
            float xDist = xHit.distance;
            workspace.Set((xDist + 0.015f) * _core.movement.facingDirection, 0f);
            RaycastHit2D yHit = Physics2D.Raycast(_core.collisionSenses.ledgeChecker.position + (Vector3)(workspace), Vector2.down, _core.collisionSenses.ledgeChecker.position.y - _core.collisionSenses.wallChecker.position.y + 0.015f, _core.collisionSenses.groundLayer);
            float yDist = yHit.distance;
            workspace.Set(_core.collisionSenses.wallChecker.position.x + (xDist * _core.movement.facingDirection), _core.collisionSenses.ledgeChecker.position.y - yDist);
            return workspace;
        }



        public override void UpdateLogic()
        {
            base.UpdateLogic();

            if (_isAnmationFinished)
            {
                if (_isTouchingCeiling)
                {
                    _stateMachine.ChangeState(_characterController.crouchIdleState);
                }
                else
                {
                    _stateMachine.ChangeState(_characterController.idleState);
                }
                
            }
            else
            {

                _xInput = _characterController.inputHandler.normInputX;
                _yInput = _characterController.inputHandler.normInputY;
                _jumpInput = _characterController.inputHandler.jumpInput;

                _characterController.core.movement.SetVelocityZero();
                _characterController.transform.position = _startPos;

                if (_xInput == _characterController.core.movement.facingDirection && _isHanging && !_isClimbing)
                {
                    CheckForSpace();
                    //_characterController.animator.SetBool("isTouchingCeiling", _isTouchingCeiling);
                    _isClimbing = true;
                    _characterController.animator.SetBool("ledgeClimb", true);
                }
                else if (_yInput == -1 && _isHanging && !_isClimbing)
                {
                    _stateMachine.ChangeState(_characterController.inAirState);
                }
                else if (_jumpInput && !_isClimbing)
                {
                    _characterController.wallJumpState.DetermineWallJumpDirection(true);
                    _stateMachine.ChangeState(_characterController.wallJumpState);
                }
            }

        }
    }
}