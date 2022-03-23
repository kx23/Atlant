using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : RaycastController
{

    [SerializeField]
    private LayerMask passengerMask;


    [SerializeField]
    private float speed;    
    [SerializeField]
    private float waitTime;
    [SerializeField]
    private bool cyclic;


    private int fromWaypointIndex=0;

    private float percentBetweenWaypoints;
    private float nextMoveTime;

    [SerializeField]
    [Range(0,2)]
    private float easeAmount;

    [SerializeField]
    private Vector3[] localWaypoints;
    [SerializeField]
    private Vector3[] globalWaypoints;


    private List<PassengerMovement> passengerMovement;
    Dictionary<Transform, Controller> passengerDictionary = new Dictionary<Transform, Controller>();




    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        globalWaypoints = new Vector3[localWaypoints.Length];
        for (int i = 0; i < localWaypoints.Length; i++)
        {
            globalWaypoints[i] = localWaypoints[i] + transform.position;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateRaycastOrigins();
        Vector3 velocity = CalculatePlatformMovement();
        CalculatePassengerMovement(velocity);
        MovePassengers(true);
        transform.Translate(velocity);
        MovePassengers(false);
    }

    Vector3 CalculatePlatformMovement()
    {
        if (Time.time < nextMoveTime)
            return Vector3.zero;

        fromWaypointIndex %= globalWaypoints.Length;
        int toWaypointIndex = (fromWaypointIndex + 1)%globalWaypoints.Length;
        float distanceBetweenWaypoints = Vector3.Distance(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex]);
        percentBetweenWaypoints += Time.deltaTime * speed/distanceBetweenWaypoints;
        percentBetweenWaypoints = Mathf.Clamp01(percentBetweenWaypoints);

        float easedPercentBetweenWaypoints = Ease(percentBetweenWaypoints);

        Vector3 newPosition = Vector3.Lerp(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex], easedPercentBetweenWaypoints);

        if (percentBetweenWaypoints >= 1)
        {
            percentBetweenWaypoints = 0;
            fromWaypointIndex++;
            if (!cyclic)
            {
                if (fromWaypointIndex >= globalWaypoints.Length - 1)
                {
                    fromWaypointIndex = 0;
                    System.Array.Reverse(globalWaypoints);
                }
            }
            nextMoveTime = Time.time + waitTime;
        }


        return newPosition - transform.position;
    }

    void MovePassengers(bool beforeMovePlatform)
    {
        /*for (int i = 0; i < passengerMovement.Count; i++)
        {
            if (passengerMovement[i].MoveBeforePlatform == beforeMovePlatform)
            {
                passengerMovement[i].PassengerTransform.GetComponent<Controller>().Move(passengerMovement[i].Velocity,);
            }
        }*/
        foreach (PassengerMovement passenger in passengerMovement) 
        {
            if (!passengerDictionary.ContainsKey(passenger.PassengerTransform))
            {
                passengerDictionary.Add(passenger.PassengerTransform, passenger.PassengerTransform.GetComponent<Controller>());
            }
            if (passenger.MoveBeforePlatform == beforeMovePlatform)
                passengerDictionary[passenger.PassengerTransform].Move(passenger.Velocity, passenger.StandingOnPlatform);
        }
            
            
    }

    float Ease(float x)
    {
        float a = easeAmount+ 1;
        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
    }

    void CalculatePassengerMovement(Vector3 velocity)
    {
        HashSet<Transform> movedPassengers = new HashSet<Transform>();
        passengerMovement = new List<PassengerMovement>();

        float directionX = Mathf.Sign(velocity.x);
        float directionY = Mathf.Sign(velocity.y);

        //vertical movement
        if (velocity.y != 0)
        {
            float rayLength = Mathf.Abs(velocity.y) + skinWidth;
            for (int i = 0; i < VerticalRayCount; i++)
            {
                Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.BottomLeft : raycastOrigins.TopLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, passengerMask);
                Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);
                if (hit&&hit.distance!=0)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = (directionY == 1) ? velocity.x : 0;
                        float pushY = velocity.y - (hit.distance - skinWidth) * directionY;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), directionY == 1, true));
                    }

                }
            }

        }

        //horizontal movement
        if (velocity.x != 0)
        {
            float rayLength = Mathf.Abs(velocity.x) + skinWidth;
            for (int i = 0; i < HorizontalRayCount; i++)
            {
                Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.BottomLeft : raycastOrigins.BottomRight;
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, passengerMask);

                if (hit && hit.distance != 0)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = velocity.x - (hit.distance - skinWidth) * directionX;
                        float pushY = -skinWidth;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), false, true));
                    }

                }

            }
        }

        // Passengers on top of a horizontally or downward platform
        if (directionY == -1 || velocity.y == 0 && velocity.x != 0)
        {
            float rayLength =  skinWidth*2;
            for (int i = 0; i < VerticalRayCount; i++)
            {
                Vector2 rayOrigin = raycastOrigins.TopLeft+ Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask);
                Debug.DrawRay(rayOrigin, Vector2.up * rayLength, Color.red);
                if (hit && hit.distance != 0)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = velocity.x;
                        float pushY = velocity.y;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), true, false));
                    }

                }
            }
        }



    }


    private void OnDrawGizmos()
    {
        if (localWaypoints != null)
        {
            Gizmos.color = Color.red;
            float size = .3f;
            for (int i = 0; i < localWaypoints.Length; ++i)
            {
                Vector3 globalWaypointsPosition=(Application.isPlaying)? globalWaypoints[i]:localWaypoints[i]+transform.position;
                Gizmos.DrawLine(globalWaypointsPosition - Vector3.up * size, globalWaypointsPosition + Vector3.up * size);
                Gizmos.DrawLine(globalWaypointsPosition - Vector3.left * size, globalWaypointsPosition + Vector3.left * size);
            }
        }
    }

    struct PassengerMovement
    {
        public Transform PassengerTransform;
        public Vector3 Velocity;
        public bool StandingOnPlatform;
        public bool MoveBeforePlatform;

        public PassengerMovement(Transform _transform, Vector3 _velocity, bool _standingOnPlatform, bool _moveBeforePlatform)
        {
            PassengerTransform = _transform;
            Velocity = _velocity;
            StandingOnPlatform = _standingOnPlatform;
            MoveBeforePlatform = _moveBeforePlatform;

        }
    }
}
