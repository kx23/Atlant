using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Controller : RaycastController
{


    [SerializeField]
    private float maxClimbAngle = 85;
    [SerializeField]
    private float maxDescendAngle = 75;

    private Vector2 playerInput;
    public Vector2 PlayerInput
    {
        get { return playerInput; }
    }


    public CollisionInfo Collisions;

    // Start is called before the first frame update

    protected override void Start()
    {
        base.Start();
        Collisions.FaceDir = 1;
    }

    //херня, поменять во это говно потом
    public void Move(Vector2 moveAmount, bool standingOnPlatform = false)
    {
        Move(moveAmount, Vector2.zero, standingOnPlatform);
    }
    public void Move(Vector2 moveAmount, Vector2 input, bool standingOnPlatform=false)
    {
        UpdateRaycastOrigins();
        Collisions.Reset();

        Collisions.OldVelocity = moveAmount;

        playerInput = input;

        if (moveAmount.x != 0)
        {
            Collisions.FaceDir = (int)Mathf.Sign(moveAmount.x);
        }

        if (moveAmount.y < 0)
        {
            DescendSlope(ref moveAmount);
        }

        //if (velocity.x!=0) если убрать комметарий, то чек по горизонтали будет происходить только если двигаться 
            HorizontalCollisions(ref moveAmount);
        if(moveAmount.y!=0)
            VerticalCollisions(ref moveAmount);
        transform.Translate(moveAmount);
        if (standingOnPlatform)
            Collisions.Below = true;

    }
    private void HorizontalCollisions(ref Vector2 moveAmount)
    {
        float directionX = Collisions.FaceDir;
        float rayLength = Mathf.Abs(moveAmount.x) + skinWidth;


        if (Mathf.Abs(moveAmount.x) < skinWidth)
            rayLength = 2 * skinWidth;
        for (int i = 0; i < HorizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.BottomLeft : raycastOrigins.BottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);
            if (hit)
            {

                if (hit.distance==0)
                {
                    continue;
                }

                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (i == 0 &&slopeAngle<=maxClimbAngle)
                {
                    if (Collisions.DescendingSlope)
                    {
                        Collisions.DescendingSlope = false;
                        moveAmount = Collisions.OldVelocity;
                    }
                    float distanceToSlopeStart = 0;
                    if (slopeAngle != Collisions.OldSlopeAngle)
                    {
                        distanceToSlopeStart = hit.distance - skinWidth;
                        moveAmount.x -= distanceToSlopeStart * directionX;
                    }
                    ClimbSlope(ref moveAmount, slopeAngle);
                    moveAmount.x += distanceToSlopeStart * directionX;
                }
                if(!Collisions.ClimbingSlope||slopeAngle>maxClimbAngle)
                {
                    moveAmount.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

                    if (Collisions.ClimbingSlope)
                    {
                        moveAmount.y = Mathf.Tan(Collisions.CurrentSlopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x);
                    }

                    Collisions.Left = directionX == -1;
                    Collisions.Right = directionX == 1;
                }

            }



        }

    }

    private void VerticalCollisions(ref Vector2 moveAmount)
    {
        float directionY = Mathf.Sign(moveAmount.y);
        float rayLength = Mathf.Abs(moveAmount.y) + skinWidth;
        for (int i = 0; i < VerticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.BottomLeft : raycastOrigins.TopLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + moveAmount.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

            if (hit)
            {
                if (hit.collider.CompareTag("Through"))
                {
                    if (directionY == 1||hit.distance==0)
                    {
                        continue;
                    }
                    if (Collisions.FallingThroughPlatform)
                    {
                        continue;
                    }
                    if (playerInput.y == -1)
                    {
                        Collisions.FallingThroughPlatform = true;
                        Invoke("ResetFallingThroughPlatform", 0.5f);
                        continue;
                    }
                }
                moveAmount.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                if (Collisions.ClimbingSlope)
                {
                    moveAmount.x = moveAmount.y / Mathf.Tan(Collisions.CurrentSlopeAngle * Mathf.Deg2Rad) * Mathf.Sign(moveAmount.x);
                }

                Collisions.Below = directionY == -1;
                Collisions.Above = directionY == 1;

            }



        }
        if (Collisions.ClimbingSlope)
        {
            float directionX = Mathf.Sign(moveAmount.x);
            rayLength = Mathf.Abs(moveAmount.x) + skinWidth;
            Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.BottomLeft : raycastOrigins.BottomRight) + Vector2.up * moveAmount.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != Collisions.CurrentSlopeAngle)
                {
                    moveAmount.x = (hit.distance - skinWidth) * directionX;
                    Collisions.CurrentSlopeAngle = slopeAngle;
                }

            }
        }

    }


    private void ClimbSlope(ref Vector2 moveAmount, float slopeAngle)
    {
        //добавить регуляцию скорости (чем больше угол тем меньше скорость)
        float moveDistance = Mathf.Abs(moveAmount.x);
        float climbVelocityY= Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
        if (moveAmount.y <= climbVelocityY)
        {
            moveAmount.y = climbVelocityY;
            moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
            Collisions.Below = true;
            Collisions.ClimbingSlope = true;
            Collisions.CurrentSlopeAngle = slopeAngle;
        }
    }

    private void DescendSlope(ref Vector2 moveAmount)
    {
        float directionX = Mathf.Sign(moveAmount.x);
        Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.BottomRight : raycastOrigins.BottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);
        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if (slopeAngle != 0 && slopeAngle <= maxDescendAngle)
            {
                if (Mathf.Sign(hit.normal.x) == directionX)
                {
                    if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x))
                    {
                        float moveDistance = Mathf.Abs(moveAmount.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
                        moveAmount.y = -descendVelocityY;

                        Collisions.CurrentSlopeAngle = slopeAngle;
                        Collisions.DescendingSlope = true;
                        Collisions.Below = true;
                    }
                        
                }
            }
        }
    }

    private void ResetFallingThroughPlatform()
    {
        Collisions.FallingThroughPlatform = false;
    }

    public struct CollisionInfo 
    {
        public bool Above, Below;
        public bool Left, Right;

        public bool ClimbingSlope;
        public float CurrentSlopeAngle, OldSlopeAngle;
        public bool DescendingSlope;

        public Vector2 OldVelocity;

        public int FaceDir;
        public bool FallingThroughPlatform;

        public void Reset()
        {
            DescendingSlope= ClimbingSlope=Above = Below = Left = Right = false;
            OldSlopeAngle = CurrentSlopeAngle;
            CurrentSlopeAngle = 0;
        }
    }
}
