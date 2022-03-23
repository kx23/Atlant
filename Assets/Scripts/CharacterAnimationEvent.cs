using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atlant
{
    public class CharacterAnimationEvent : MonoBehaviour
    {

        private CharacterController _characterController;
        private void Awake()
        {
            _characterController = GetComponentInParent<CharacterController>();
        }

        private void AnimationTrigger() => _characterController.stateMachine.currentState.AnimationTrigger();
        private void AnimationFinishTrigger() => _characterController.stateMachine.currentState.AnimationFinishTrigger();

    }
}
