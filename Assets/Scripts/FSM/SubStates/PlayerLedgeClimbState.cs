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

        private int _xInput;
        private int _yInput;

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
            _characterController.SetVelocityZero();
            _characterController.transform.position = _detectedPos;
            _cornerPos = _characterController.DetermineCornerPosition();

            _startPos.Set(_cornerPos.x-(_characterController.facingDirection*_playerData.startOffset.x),_cornerPos.y-_playerData.startOffset.y);
            _stopPos.Set(_cornerPos.x+(_characterController.facingDirection*_playerData.stopOffset.x),_cornerPos.y+_playerData.stopOffset.y);

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

        public override void UpdateLogic()
        {
            base.UpdateLogic();

            if (_isAnmationFinished)
            {
                _stateMachine.ChangeState(_characterController.idleState);
            }
            else
            {

                _xInput = _characterController.inputHandler.normInputX;
                _yInput = _characterController.inputHandler.normInputY;
                _jumpInput = _characterController.inputHandler.jumpInput;

                _characterController.SetVelocityZero();
                _characterController.transform.position = _startPos;

                if (_xInput == _characterController.facingDirection && _isHanging && !_isClimbing)
                {
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