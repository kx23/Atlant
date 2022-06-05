using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Atlant
{
    [CreateAssetMenu(fileName = "NewPlayerData", menuName = "Data/PlayerData/BaseData")]
    public class PlayerData : ScriptableObject
    {
        [Header("Move state")]
        public float movementVelocity = 10f;
        public float standColliderHeight = 0.45f;

        [Header("Jump state")]
        public float jumpVelocity = 15f;
        public int amountOfJumps = 1;

        [Header("Wall Jump state")]
        public float wallJumpVelocity = 20;
        public Vector2 wallJumpAngle = new Vector2(1, 2);
        public float wallJumpTime = 0.3f;

        [Header("In Air state")]
        public float coyoteTime = 0.2f;
        public float variableJumpHeightMultiplier = 0.5f;

        [Header("Wall Slide state")]
        public float wallSlideVelocity = 3f;

        [Header("Wall Climb state")]
        public float wallClimbVelocity = 2f;

        [Header("Ledge Climb State")]
        public Vector2 startOffset;
        public Vector2 stopOffset;

        [Header("Crouch State")]
        public float crouchMovementVelocity = 5f;
        public float crouchColliderHeight = 0.3f;
        
        [Header("Dash State")]
        public float dashCooldown = 0.5f;
        public float maxHoldTime = 1f;
        public float holdTimeScale = 0.15f;
        public float dashTime = 0.2f;
        public float dashVelocity = 15f;
        public float drag = 10f;
        public float dashEndYMultiplier = 0.2f;
        public float distBetweenAfterImages = 0.1f;

    }
}
