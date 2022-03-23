using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private float lookAheadDistanceX;      
    [SerializeField]
    private float lookSmoothTimeX;    
    [SerializeField]
    private float verticalSmoothTime;   
    
    [SerializeField]
    private float verticalOffset;
    [SerializeField]
    public Controller target;
    [SerializeField]
    private Vector2 focusAreaSize;
    FocusArea focusArea;


    float currentLookAheadX;
    float targetLookAheadX;
    float lookAheadDirectionX;
    float smoothLookVelocityX;
    float smoothVelocityY;

    bool lookAheadStopped;

    private void Start()
    {
        focusArea = new FocusArea(target.BoxCollider2D.bounds, focusAreaSize);
    }

    private void LateUpdate()
    {
        focusArea.Update(target.BoxCollider2D.bounds);

        Vector2 focusPosition = focusArea.Centre + Vector2.up * verticalOffset;

        if (focusArea.velocity.x != 0)
        {
            lookAheadDirectionX = Mathf.Sign(focusArea.velocity.x);
            if (Mathf.Sign(target.PlayerInput.x) == Mathf.Sign(focusArea.velocity.x) && target.PlayerInput.x != 0)
            {
                lookAheadStopped = false;
                targetLookAheadX = lookAheadDirectionX * lookAheadDistanceX;
            }
            else
            {
                if (!lookAheadStopped)
                {
                    lookAheadStopped = true;
                    targetLookAheadX = currentLookAheadX + (lookAheadDirectionX * lookAheadDistanceX - currentLookAheadX) / 4f;
                }
                
            }
        }
        
        currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX);

        focusPosition.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref smoothVelocityY, verticalSmoothTime);
        focusPosition += Vector2.right * currentLookAheadX;
        transform.position = (Vector3)focusPosition + Vector3.forward * -10;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(focusArea.Centre, focusAreaSize);
    }

    struct FocusArea
    {
        public Vector2 Centre;
        public Vector2 velocity;
        float left, right, top, bottom;

        public FocusArea(Bounds targretBounds, Vector2 size)
        {
            left = targretBounds.center.x - size.x / 2;
            right = targretBounds.center.x + size.x / 2;
            bottom = targretBounds.min.y;
            top = targretBounds.min.y+size.y;

            velocity = Vector2.zero;
            Centre = new Vector2((left + right) / 2, (top + bottom) / 2);


        }
        public void Update(Bounds targetBounds)
        {
            float shiftX = 0;
            if (targetBounds.min.x < left)
            {
                shiftX = targetBounds.min.x - left;
            }
            else if(targetBounds.max.x>right)
            {
                shiftX = targetBounds.max.x - right;
            }
            left += shiftX;
            right += shiftX;

            float shiftY = 0;
            if (targetBounds.min.y < bottom)
            {
                shiftY = targetBounds.min.y - bottom;
            }
            else if (targetBounds.max.y > top)
            {
                shiftY = targetBounds.max.y - top;
            }
            top += shiftY;
            bottom += shiftY;

            Centre = new Vector2((left + right) / 2, (top + bottom) / 2);
            velocity = new Vector2(shiftX, shiftY);
        }
    }

}
