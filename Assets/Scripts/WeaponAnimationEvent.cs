using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atlant
{
    public class WeaponAnimationEvent : MonoBehaviour
    {
        private Weapon _weapon;
        private void Awake()
        {
            _weapon = GetComponentInParent<Weapon>();
        }
        //private void AnimationTrigger() => _weapon.AnimationTrigger();
        private void AnimationFinishTrigger() => _weapon.AnimationFinishTrigger();
    }
}