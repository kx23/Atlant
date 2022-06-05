using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atlant
{
    public class PlayerDashState : PlayerAbilityState
    {
        public bool canDash { get; private set; }
        private bool _isHolding;
        private bool _dashInputStop;
        private float _lastDashTime;

        private Vector2 _dashDirection;
        private Vector2 _dashDirectionInput;
        private Vector2 _lastAfterImagePosition;

        public PlayerDashState(CharacterController characterController, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(characterController, stateMachine, playerData, animBoolName)
        {
        }
        public override void Enter()
        {
            base.Enter();

            canDash = false;
            _characterController.inputHandler.UseDashUnput();

            _isHolding = true;
            _dashDirection = Vector2.right * _characterController.core.movement.facingDirection;

            Time.timeScale = _playerData.holdTimeScale;
            startTime = Time.unscaledTime;

            _characterController.dashDirectionIndicator.gameObject.SetActive(true);
        }

        public override void Exit()
        {
            base.Exit();

            if (_characterController.core.movement.currentVelocity.y > 0)
            {
                _characterController.core.movement.SetVelocityY(_characterController.core.movement.currentVelocity.y * _playerData.dashEndYMultiplier);
            }
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();

            if (!_isExitingState)
            {
                _characterController.animator.SetFloat("yVelocity", _characterController.core.movement.currentVelocity.y);
                _characterController.animator.SetFloat("xVelocity", Mathf.Abs(_characterController.core.movement.currentVelocity.x));
                if (_isHolding)
                {
                    _dashDirectionInput = _characterController.inputHandler.dashDirectionInput;
                    _dashInputStop = _characterController.inputHandler.dashInputStop;

                    if (_dashDirectionInput != Vector2.zero)
                    {
                        _dashDirection = _dashDirectionInput;
                        _dashDirection.Normalize();

                    }

                    float angle = Vector2.SignedAngle(Vector2.right, _dashDirection);
                    _characterController.dashDirectionIndicator.rotation = Quaternion.Euler(0f, 0f, angle);
                    if (_dashInputStop || Time.unscaledTime >= startTime + _playerData.maxHoldTime) 
                    {
                        _isHolding = false;
                        Time.timeScale = 1f;
                        startTime = Time.time;
                        _characterController.core.movement.CheckIfShoudFlip(Mathf.RoundToInt(_dashDirection.x));
                        _characterController.rb.drag = _playerData.drag;
                        _characterController.core.movement.SetVelocity(_playerData.dashVelocity, _dashDirection);
                        _characterController.dashDirectionIndicator.gameObject.SetActive(false);
                        PlaceAfterImage();
                    }
                }
                else
                {
                    _characterController.core.movement.SetVelocity(_playerData.dashVelocity, _dashDirection);
                    CheckIfShoudPlaceAfterImage();
                    if (Time.time >= startTime + _playerData.dashTime)
                    {
                        _characterController.rb.drag = 0;
                        _isAbilityDone = true;
                        _lastDashTime = Time.time;
                    }
                }
            }
        }
        private void CheckIfShoudPlaceAfterImage()
        {
            if (Vector2.Distance(_characterController.transform.position, _lastAfterImagePosition) >= _playerData.distBetweenAfterImages)
            {
                PlaceAfterImage();
            }
        }

        private void PlaceAfterImage()
        {
            PlayerAfterImagePool.instance.GetFromPool();
            _lastAfterImagePosition = _characterController.transform.position;
        }

        public bool CheckIfCanDash()
        {
            return canDash && Time.time >= _lastDashTime + _playerData.dashCooldown;
        }

        public void ResetCanDash() => canDash = true;

    }
}