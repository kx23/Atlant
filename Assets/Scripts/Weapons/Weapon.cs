using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atlant
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private SO_WeaponData _weaponData;

        protected Animator _baseAnimator;
        protected Animator _weaponAnimator;
        protected PlayerAttackState _attackState;
        protected int _attackCounter;
        private void Start()
        {
            _baseAnimator = transform.Find("Base").GetComponent<Animator>();
            _weaponAnimator = transform.Find("Weapon").GetComponent<Animator>();

            gameObject.SetActive(false);
        }
        public virtual void EnterWeapon()
        {
            gameObject.SetActive(true);


            if (_attackCounter >= 3)
            {
                _attackCounter = 0;
            }
            _baseAnimator.SetBool("attack", true);
            _weaponAnimator.SetBool("attack", true);

            _baseAnimator.SetInteger("attackCounter", _attackCounter);
            _weaponAnimator.SetInteger("attackCounter", _attackCounter);
        }
        public virtual void ExitWeapon()
        {
            
            _baseAnimator.SetBool("attack", false);
            _weaponAnimator.SetBool("attack", false);
            _attackCounter++;
            gameObject.SetActive(false);
        }
        #region Animation Triggers
        public virtual void AnimationFinishTrigger()
        {
            _attackState.AnimationFinishTrigger();
        }
        public virtual void AnimationStartMovementTrigger()
        {
            _attackState.SetPlayerVelocity(_weaponData.movementSpeed[_attackCounter]);
        }

        public virtual void AnimationStopMovementTrigger()
        {
            _attackState.SetPlayerVelocity(0f);
        }
        public virtual void AnimationTurnOffFlipTrigger()
        {
            _attackState.SetFlipCheck(false);
        }
        public virtual void AnimationTurnOnFlipTrigger()
        {
            _attackState.SetFlipCheck(true);
        }



        #endregion
        public void InitializeWeapon(PlayerAttackState state)
        {
            this._attackState = state;
        }
    }


}