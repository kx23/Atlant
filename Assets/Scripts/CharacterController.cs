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
        public Animator animator { get; private set; }
        public PlayerInputHandler inputHandler { get; private set; }
        public Rigidbody2D rb { get; private set; }
        public Transform dashDirectionIndicator { get; private set; }
        public PlayerAfterImagePool afterImagePool { get; private set; }
        public BoxCollider2D movementCollider { get; private set; }
        public PlayerInventoryForTests inventoryForTests { get; private set; }
        #endregion

        #region Check Transform Variables
        [SerializeField]
        private Transform _groundChecker;
        [SerializeField]
        private Transform _wallChecker;
        [SerializeField]
        private Transform _ledgeChecker;
        [SerializeField]
        private Transform _ceilingChecker;


        #endregion

        #region Other Variables
        public Vector2 currentVelocity { get; private set; }
        public int facingDirection { get; private set; }
        private Vector2 workspace;
        #endregion

        #region Unity Callback Functions
        private void Awake()
        {
            facingDirection = 1;
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
            currentVelocity = rb.velocity;
            stateMachine.currentState.UpdateLogic();
        }


        private void FixedUpdate()
        {
            stateMachine.currentState.UpdatePhysics();
        }
        #endregion

        #region Set Functions
        public void SetVelocityZero()
        {
            rb.velocity = Vector2.zero;
            currentVelocity = Vector2.zero;
        }

        public void SetVelocity(float velocity, Vector2 angle, int direction)
        {
            angle.Normalize();
            workspace.Set(angle.x * velocity * direction, angle.y * velocity);
            rb.velocity = workspace;
            currentVelocity = workspace;
        }        
        public void SetVelocity(float velocity, Vector2 direction)
        {
            workspace = direction * velocity;
            rb.velocity = workspace;
            currentVelocity = workspace;
        }

        public void SetVelocityX(float velocity)
        {
            workspace.Set(velocity, currentVelocity.y);
            rb.velocity = workspace;
            currentVelocity = workspace;
        }

        public void SetVelocityY(float velocity)
        {
            workspace.Set(currentVelocity.x,velocity);
            rb.velocity = workspace;
            currentVelocity = workspace;
        }
        #endregion

        #region Check Functions
        public void CheckIfShoudFlip(int xInput)
        {
            if (xInput != 0 && xInput != facingDirection)
            {
                Flip();
            }
        }

        public bool CheckForCeiling()
        {
            //Debug.DrawLine(_groundChecker.position, _groundChecker.position+ (Vector3.down*_playerData.groundCheckRadius),Color.red, 1f);
            return Physics2D.OverlapCircle(_ceilingChecker.position, _playerData.groundCheckRadius, _playerData.groundLayer);
        }

        public bool CheckIfGrounded()
        {
            //Debug.DrawLine(_groundChecker.position, _groundChecker.position+ (Vector3.down*_playerData.groundCheckRadius),Color.red, 1f);
            return Physics2D.OverlapCircle(_groundChecker.position, _playerData.groundCheckRadius, _playerData.groundLayer);
        }

        public bool CheckIfTouchingWall()
        {
            return Physics2D.Raycast(_wallChecker.position, Vector2.right * facingDirection, _playerData.wallCheckDistance, _playerData.groundLayer);
        }

        public bool CheckIfTouchingLedge()
        {
            return Physics2D.Raycast(_ledgeChecker.position, Vector2.right * facingDirection, _playerData.wallCheckDistance, _playerData.groundLayer);
        }

        public bool CheckIfTouchingWallBack()
        {
            return Physics2D.Raycast(_wallChecker.position, Vector2.right * -facingDirection, _playerData.wallCheckDistance, _playerData.groundLayer);
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
        private void Flip()
        {
            facingDirection *= -1;
            transform.Rotate(0.0f, 180f, 0f);
        }
        public Vector2 DetermineCornerPosition()
        {
            //исправить
            RaycastHit2D xHit = Physics2D.Raycast(_wallChecker.position, Vector2.right * facingDirection, _playerData.wallCheckDistance, _playerData.groundLayer);
            float xDist = xHit.distance;
            workspace.Set((xDist+0.015f) * facingDirection, 0f);
            RaycastHit2D yHit = Physics2D.Raycast(_ledgeChecker.position+(Vector3)(workspace), Vector2.down, _ledgeChecker.position.y-_wallChecker.position.y+0.015f, _playerData.groundLayer);
            float yDist = yHit.distance;
            workspace.Set(_wallChecker.position.x + (xDist * facingDirection), _ledgeChecker.position.y - yDist);
            return workspace;
        }


        //private void AnimationTrigger() => stateMachine.currentState.AnimationTrigger();
        //private void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

        #endregion
    }
}