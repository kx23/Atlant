using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atlant
{
    public class Movement : CoreComponent
    {
        public Rigidbody2D rb { get; private set; }

        public int facingDirection { get; private set; }

        public Vector2 currentVelocity { get; private set; }
        private Vector2 _workspace;

        protected override void Awake()
        {
            base.Awake();

            rb = GetComponentInParent<Rigidbody2D>();

            if (_core == null)
            {
                Debug.LogError("There is no Core on the parent.");
            }

            facingDirection = 1;
        }

        public void LogicUpdate()
        {
            currentVelocity = rb.velocity;
        }

        #region Set Functions
        public void SetVelocityZero()
        {
            rb.velocity = Vector2.zero;
            currentVelocity = Vector2.zero;
        }

        public void SetVelocity(float velocity, Vector2 angle, int direction)
        {
            angle.Normalize();
            _workspace.Set(angle.x * velocity * direction, angle.y * velocity);
            rb.velocity = _workspace;
            currentVelocity = _workspace;
        }
        public void SetVelocity(float velocity, Vector2 direction)
        {
            _workspace = direction * velocity;
            rb.velocity = _workspace;
            currentVelocity = _workspace;
        }

        public void SetVelocityX(float velocity)
        {
            _workspace.Set(velocity, currentVelocity.y);
            rb.velocity = _workspace;
            currentVelocity = _workspace;
        }

        public void SetVelocityY(float velocity)
        {
            _workspace.Set(currentVelocity.x, velocity);
            rb.velocity = _workspace;
            currentVelocity = _workspace;
        }
        #endregion

        #region Other Functions

        private void Flip()
        {
            facingDirection *= -1;
            rb.transform.Rotate(0.0f, 180f, 0f);
        }
        public void CheckIfShoudFlip(int xInput)
        {
            if (xInput != 0 && xInput != facingDirection)
            {
                Flip();
            }
        }

        #endregion
    }
}