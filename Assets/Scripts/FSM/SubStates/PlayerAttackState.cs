using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atlant
{
    public class PlayerAttackState : PlayerAbilityState
    {
        private Weapon _weapon;

        private int _xInput;
        private float _velocityToSet;
        private bool _setVelocity;

        private bool _shouldCheckFlip;

        public PlayerAttackState(CharacterController characterController, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(characterController, stateMachine, playerData, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _setVelocity = false;

            _weapon.EnterWeapon();
        }

        public override void Exit()
        {
            base.Exit();

            _weapon.ExitWeapon();
        }


        public override void UpdateLogic()
        {
            base.UpdateLogic();
            _xInput = _characterController.inputHandler.normInputX;

            if (_shouldCheckFlip)
            {
                _characterController.core.movement.CheckIfShoudFlip(_xInput);
            }
            if (_setVelocity)
            {
                _characterController.core.movement.SetVelocityX(_velocityToSet * _characterController.core.movement.facingDirection);
            }
        }
        public void SetWeapon(Weapon weapon)
        {
            
            this._weapon = weapon;
            weapon.InitializeWeapon(this);
        }

        public void SetPlayerVelocity(float velocity)
        {
            _characterController.core.movement.SetVelocityX(velocity * _characterController.core.movement.facingDirection);
            _velocityToSet = velocity;
            _setVelocity = true;
        }

        public void SetFlipCheck(bool value)
        {
            _shouldCheckFlip = value;
        }

        #region Animation Triggers
        public override void AnimationFinishTrigger()
        {
            base.AnimationFinishTrigger();
            _isAbilityDone = true;
        }
        public override void AnimationTrigger()
        {
            base.AnimationTrigger();
        }

        #endregion
    }
}