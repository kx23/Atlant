using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour
{

    public LayerMask collisionMask;

    protected const float skinWidth = 0.015f;
    public const float dstBetweenRays= .25f;

    [HideInInspector] public int HorizontalRayCount;
    [HideInInspector] public int VerticalRayCount;

    protected float horizontalRaySpacing;
    protected float verticalRaySpacing;

    protected RaycastOrigins raycastOrigins;
    protected BoxCollider2D boxCollider2D;
    public BoxCollider2D BoxCollider2D
    {
        get { return boxCollider2D; }
    }


    protected virtual void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        
    }

    protected virtual void Start() 
    {
        CalculateRaySpacing();
    }




    protected void UpdateRaycastOrigins()
    {
        Bounds bounds = boxCollider2D.bounds;
        bounds.Expand(skinWidth * -2);
        raycastOrigins.BottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.BottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.TopLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.TopRight = new Vector2(bounds.max.x, bounds.max.y);
    }
    protected void CalculateRaySpacing()
    {
        Bounds bounds = boxCollider2D.bounds;
        bounds.Expand(skinWidth * -2);

        float boundsWidth = bounds.size.x;
        float boundsHeight = bounds.size.y;
        HorizontalRayCount = Mathf.RoundToInt(boundsHeight / dstBetweenRays);
        VerticalRayCount = Mathf.RoundToInt(boundsWidth / dstBetweenRays);

        horizontalRaySpacing = bounds.size.y / (HorizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (VerticalRayCount - 1);
    }
    protected struct RaycastOrigins
    {
        public Vector2 TopLeft, TopRight;
        public Vector2 BottomLeft, BottomRight;
    }

}
