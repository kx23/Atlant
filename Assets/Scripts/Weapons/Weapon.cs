using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atlant
{
    public class Weapon : MonoBehaviour
    {
        protected Animator _baseAnimator;
        protected Animator _weaponAnimator;
        protected PlayerAttackState _attackState;
        private void Start()
        {
            _baseAnimator = transform.Find("Base").GetComponent<Animator>();
            _weaponAnimator = transform.Find("Weapon").GetComponent<Animator>();

            gameObject.SetActive(false);
        }
        public virtual void EnterWeapon()
        {
            gameObject.SetActive(true);
            _baseAnimator.SetBool("attack", true);
            _weaponAnimator.SetBool("attack", true);
        }
        public virtual void ExitWeapon()
        {
            
            _baseAnimator.SetBool("attack", false);
            _weaponAnimator.SetBool("attack", false);
            gameObject.SetActive(false);
        }
        #region Animation Triggers
        public virtual void AnimationFinishTrigger()
        {
            _attackState.AnimationFinishTrigger();
        }
        #endregion
        public void InitializeWeapon(PlayerAttackState state)
        {
            this._attackState = state;
        }
    }


}