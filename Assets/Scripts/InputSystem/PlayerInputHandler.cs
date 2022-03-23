using UnityEngine;
using UnityEngine.InputSystem;


namespace Atlant
{
    public class PlayerInputHandler : MonoBehaviour
    {

        public Vector2 rawMovementInput { get; private set; }
        public int normInputX { get; private set; }
        public int normInputY { get; private set; }
        public bool jumpInput { get; private set; }
        public bool jumpInputStop { get; private set; }
        public bool grabInput { get; private set; }

        [SerializeField]
        private float _inputHoldTime=0.2f;
        private float _jumpInputStartTime;

        private void Update()
        {
            CheckJumpInputHoldTime();
        }

        public void OnMoveInput(InputAction.CallbackContext context)
        {
            rawMovementInput = context.ReadValue<Vector2>();
            if (Mathf.Abs(rawMovementInput.x) > 0.5f)
            {
                normInputX = (int)(rawMovementInput * Vector2.right).normalized.x;
            }
            else
            {
                normInputX = 0;
            }
            if (Mathf.Abs(rawMovementInput.y) > 0.5f)
            {
                normInputY = (int)(rawMovementInput * Vector2.up).normalized.y;
            }
            else
            {
                normInputY = 0;
            }
        }

        public void OnJumpInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                jumpInput = true;
                jumpInputStop = false;
                _jumpInputStartTime = Time.time;
            }

            if (context.canceled)
            {
                jumpInputStop = true;
            }
        }

        public void OnGrabInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                grabInput = true; 
            }
            if (context.canceled)
            {
                grabInput = false;
            }
        }    

        public void UseJumpUnput() => jumpInput = false;
        private void CheckJumpInputHoldTime()
        {
            if (Time.time >= _jumpInputStartTime + _inputHoldTime)
            {
                jumpInput = false;
            }
        }


    }
}