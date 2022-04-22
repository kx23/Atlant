using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atlant
{
    public class PlayerAttackState : PlayerAbilityState
    {
        private Weapon _weapon;
        public PlayerAttackState(CharacterController characterController, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(characterController, stateMachine, playerData, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _weapon.EnterWeapon();
        }

        public override void Exit()
        {
            base.Exit();

            _weapon.ExitWeapon();
        }

        public void SetWeapon(Weapon weapon)
        {
            
            this._weapon = weapon;
            weapon.InitializeWeapon(this);
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