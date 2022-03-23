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



        [SerializeField]
        private PlayerData _playerData;
        #endregion

        #region Components
        public Animator animator { get; private set; }
        public PlayerInputHandler inputHandler { get; private set; }
        public Rigidbody2D rb { get; private set; }
        #endregion

        #region Check Transform Variables
        [SerializeField]
        private Transform _groundChecker;
        [SerializeField]
        private Transform _wallChecker;


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
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            stateMachine.Initialize(idleState);
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
        public void SetVelocity(float velocity, Vector2 angle, int direction)
        {
            angle.Normalize();
            workspace.Set(angle.x * velocity * direction, angle.y * velocity);
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

        public bool CheckIfGrounded()
        {
            //Debug.DrawLine(_groundChecker.position, _groundChecker.position+ (Vector3.down*_playerData.groundCheckRadius),Color.red, 1f);
            return Physics2D.OverlapCircle(_groundChecker.position, _playerData.groundCheckRadius, _playerData.groundLayer);
        }

        public bool CheckIfTouchingWall()
        {
            return Physics2D.Raycast(_wallChecker.position, Vector2.right * facingDirection, _playerData.wallCheckDistance, _playerData.groundLayer);
        }
        public bool CheckIfTouchingWallBack()
        {
            return Physics2D.Raycast(_wallChecker.position, Vector2.right * -facingDirection, _playerData.wallCheckDistance, _playerData.groundLayer);
        }

        #endregion

        #region Other Functions
        private void Flip()
        {
            facingDirection *= -1;
            transform.Rotate(0.0f, 180f, 0f);
        }

        //private void AnimationTrigger() => stateMachine.currentState.AnimationTrigger();
        //private void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

        #endregion
    }
}