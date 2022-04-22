using UnityEngine;
using UnityEngine.InputSystem;
using System;



namespace Atlant
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private PlayerInput _playerInput;
        private Camera _camera;

        public Vector2 rawMovementInput { get; private set; }
        public Vector2 rawDashDirectionInput { get; private set; }
        public Vector2Int dashDirectionInput { get; private set; }
        public int normInputX { get; private set; }
        public int normInputY { get; private set; }
        public bool jumpInput { get; private set; }
        public bool jumpInputStop { get; private set; }
        public bool grabInput { get; private set; }
        public bool dashInput { get; private set; }
        public bool dashInputStop { get; private set; }

        public bool[] AttackInputs { get; private set; }

        [SerializeField]
        private float _inputHoldTime=0.2f;
        private float _jumpInputStartTime;
        private float _dashInputStartTime;


        private void Start()
        {
            _playerInput = GetComponent<PlayerInput>();
            int count = Enum.GetValues(typeof(CombatInputs)).Length;
            AttackInputs = new bool[count];
            _camera = Camera.main;
        }

        private void Update()
        {
            CheckJumpInputHoldTime();
            CheckDashInputHoldTime();
        }
        public void OnPrimaryAttackInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                AttackInputs[(int)CombatInputs.primary] = true;
            }
            if (context.canceled)
            {
                AttackInputs[(int)CombatInputs.primary] = false;
            }
        }        
        public void OnSecondaryAttackInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                AttackInputs[(int)CombatInputs.secondary] = true;
            }
            if (context.canceled)
            {
                AttackInputs[(int)CombatInputs.secondary] = false;
            }
        }


        public void OnMoveInput(InputAction.CallbackContext context)
        {
            rawMovementInput = context.ReadValue<Vector2>();
            normInputX = Mathf.RoundToInt(rawMovementInput.x);
            normInputY = Mathf.RoundToInt(rawMovementInput.y);

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
        public void OnDashInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                dashInput = true;
                dashInputStop = false;
                _dashInputStartTime = Time.time;
            }

            if (context.canceled)
            {
                dashInputStop = true;
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

        public void OnDashDirectionInput(InputAction.CallbackContext context)
        {
            rawDashDirectionInput = context.ReadValue<Vector2>();

            if (_playerInput.currentControlScheme == "Keyboard + Mouse")
            {
                rawDashDirectionInput = _camera.ScreenToWorldPoint((Vector3)rawDashDirectionInput) - transform.position;
            }
            dashDirectionInput = Vector2Int.RoundToInt(rawDashDirectionInput.normalized);
        }



        public void UseJumpUnput() => jumpInput = false;
        public void UseDashUnput() => dashInput = false;

        private void CheckJumpInputHoldTime()
        {
            if (Time.time >= _jumpInputStartTime + _inputHoldTime)
            {
                jumpInput = false;
            }
        }        
        private void CheckDashInputHoldTime()
        {
            if (Time.time >= _dashInputStartTime + _inputHoldTime)
            {
                dashInput = false;
            }
        }


    }

    public enum CombatInputs
    {
        primary,
        secondary
    }
}