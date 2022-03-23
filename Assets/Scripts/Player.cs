using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller))]
public class Player : MonoBehaviour
{



    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    private float wallSlidingSpeedMax=3;

    [SerializeField]
    private float timeToJumpApex = 0.4f;
    [SerializeField]
    private float maxJumpHeight = 4f; 
    [SerializeField]
    private float minJumpHeight = 1f;
    [SerializeField]
    private float moveSpeed = 6f;

    private float acclerationTimeAirborn=.15f;
    private float acclerationTimeGrounded = .1f;


    private float gravity = -20f;
    private float maxJumpVelocity;
    private float minJumpVelocity;
    private Vector3 velocity;


    private Controller controller;
    private float velocityXSmoothing;

    private Vector2 _directionalInput;
    public Vector2 directionalInput { get { return _directionalInput; } set { _directionalInput = value; } }

    [SerializeField]
    private float wallStickTime = 0.25f;
    private float timeToWallUnstick;
    private bool wallSliding;
    private float wallDirX;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Controller>();
        gravity = -(2 * maxJumpHeight) / (timeToJumpApex * timeToJumpApex);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }
    private void Update()
    {

        CalculateVelocity();
        HandleWallSliding();
        //поменять на простой апдейт или убрать все тайм дельта таймы
        controller.Move(velocity* Time.deltaTime,_directionalInput);
        if (controller.Collisions.Below||controller.Collisions.Above)
            velocity.y = 0;

    }

    private void HandleWallSliding()
    {
        //поменять на простой апдейт или убрать все тайм дельта таймы
        wallDirX = (controller.Collisions.Left) ? -1 : 1;
        wallSliding = false;
        if ((controller.Collisions.Left || controller.Collisions.Right) && !controller.Collisions.Below && velocity.y < 0)
        {
            wallSliding = true;
            if (velocity.y < -wallSlidingSpeedMax)
            {
                velocity.y = -wallSlidingSpeedMax;
            }
            if (timeToWallUnstick > 0)
            {
                velocityXSmoothing = 0;
                velocity.x = 0;
                if (_directionalInput.x != wallDirX && _directionalInput.x != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }

            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }

        }
    }
    private void CalculateVelocity()
    {
        float targetVelocityX = _directionalInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.Collisions.Below) ? acclerationTimeGrounded : acclerationTimeAirborn);
        velocity.y += gravity * Time.deltaTime;
    }

    public void OnJumpInputDown() 
    {
        if (wallSliding)
        {
            if (wallDirX == _directionalInput.x)
            {
                velocity.x = -wallDirX * wallJumpClimb.x;
                velocity.y = wallJumpClimb.y;
            }
            else if (_directionalInput.x == 0)
            {
                velocity.x = -wallDirX * wallJumpOff.x;
                velocity.y = wallJumpOff.y;
            }
            else
            {
                velocity.x = -wallDirX * wallLeap.x;
                velocity.y = wallLeap.y;
            }
        }
        if (controller.Collisions.Below)
            velocity.y = maxJumpVelocity;
    }
    public void OnJumpInputUp()
    {
        if (velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }
    }

}
