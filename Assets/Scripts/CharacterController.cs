using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atlant
{
    public class CharacterController : MonoBehaviour
    {
        #region State Variables
        public StateMachine stateMachine { get; private set; }
        public PlayerIdleState idleState { get; private set; }
        public PlayerMoveState moveState { get; private set; }
        public PlayerJumpState jumpState { get; private set; }
        public PlayerInAirState inAirState { get; private set; }
        public PlayerLandState landState { get; private set; }
        public PlayerWallClimbState wallClimbState { get; private set; }
        public PlayerWallSlideState wallSlideState { get; private set; }
        public PlayerWallGrabState wallGrabState { get; private set; }
        public PlayerWallJumpState wallJumpState { get; private set; }
        public PlayerLedgeClimbState ledgeClimbState { get; private set; }
        public PlayerDashState dashState { get; private set; }
        public PlayerCrouchIdleState crouchIdleState { get; private set; }
        public PlayerCrouchMoveState crouchMoveState { get; private set; }
        public PlayerAttackState primaryAttackState { get; private set; }
        public PlayerAttackState secondaryAttackState { get; private set; }



        [SerializeField]
        private PlayerData _playerData;
        #endregion

        #region Components
        public Core core { get; private set; }
        public Animator animator { get; private set; }
        public PlayerInputHandler inputHandler { get; private set; }
        public Rigidbody2D rb { get; private set; }
        public Transform dashDirectionIndicator { get; private set; }
        public PlayerAfterImagePool afterImagePool { get; private set; }
        public BoxCollider2D movementCollider { get; private set; }
        public PlayerInventoryForTests inventoryForTests { get; private set; }
        #endregion



        #region Other Variables
        private Vector2 workspace;
        #endregion

        #region Unity Callback Functions
        private void Awake()
        {
            core = GetComponentInChildren<Core>();

            animator = GetComponentInChildren<Animator>();
            inputHandler = GetComponent<PlayerInputHandler>();
            stateMachine = new StateMachine();
            idleState = new PlayerIdleState(this, stateMachine, _playerData, "idle");
            moveState = new PlayerMoveState(this, stateMachine, _playerData, "move");
            jumpState = new PlayerJumpState(this, stateMachine, _playerData, "inAir");
            inAirState = new PlayerInAirState(this, stateMachine, _playerData, "inAir");
            landState = new PlayerLandState(this, stateMachine, _playerData, "land");

            wallClimbState = new PlayerWallClimbState(this, stateMachine, _playerData, "wallClimb");
            wallSlideState = new PlayerWallSlideState(this, stateMachine, _playerData, "wallSlide");
            wallGrabState = new PlayerWallGrabState(this, stateMachine, _playerData, "wallGrab");

            wallJumpState = new PlayerWallJumpState(this, stateMachine, _playerData, "inAir");

            ledgeClimbState = new PlayerLedgeClimbState(this, stateMachine, _playerData, "ledgeClimbState");
            dashState = new PlayerDashState(this, stateMachine, _playerData, "inAir");

            crouchIdleState = new PlayerCrouchIdleState(this, stateMachine, _playerData, "crouchIdle");
            crouchMoveState = new PlayerCrouchMoveState(this, stateMachine, _playerData, "crouchMove");

            primaryAttackState = new PlayerAttackState(this, stateMachine, _playerData, "attack");
            secondaryAttackState = new PlayerAttackState(this, stateMachine, _playerData, "attack");

            rb = GetComponent<Rigidbody2D>();
            
        }

        private void Start()
        {
            stateMachine.Initialize(idleState);
            dashDirectionIndicator = transform.Find("DashDirectionIndicator");
            movementCollider = GetComponent<BoxCollider2D>();
            inventoryForTests = GetComponent<PlayerInventoryForTests>();
            primaryAttackState.SetWeapon(inventoryForTests.weapons[(int)CombatInputs.primary]);
            //secondaryAttackState.SetWeapon(inventoryForTests.weapons[(int)CombatInputs.secondary]);


        }

        private void Update()
        {
            core.LogicUpdate();
            stateMachine.currentState.UpdateLogic();
        }


        private void FixedUpdate()
        {
            stateMachine.currentState.UpdatePhysics();
        }
        #endregion

        

        

        #region Other Functions

        public void SetColliderHeight(float height)
        {
            Vector2 center = movementCollider.offset;
            workspace.Set(movementCollider.size.x, height);

            center.y += (height - movementCollider.size.y) / 2;

            movementCollider.size = workspace;
            movementCollider.offset = center;
        }



        //private void AnimationTrigger() => stateMachine.currentState.AnimationTrigger();
        //private void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

        #endregion
    }
}