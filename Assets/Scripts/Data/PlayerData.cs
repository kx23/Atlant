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

        [Header("Jump state")]
        public float jumpVelocity = 15f;
        public int amountOfJumps = 1;

        [Header("Wall Jump state")]
        public float wallJumpVelocity=20;
        public Vector2 wallJumpAngle=new Vector2(1,2);
        public float wallJumpTime = 0.3f;

        [Header("In Air state")]
        public float coyoteTime = 0.2f;
        public float variableJumpHeightMultiplayer = 0.5f;

        [Header("Wall Slide state")]
        public float wallSlideVelocity = 3f;

        [Header("Wall Climb state")]
        public float wallClimbVelocity = 2f;

        [Header("Check variables")]
        public float groundCheckRadius = 0.1f;
        public float wallCheckDistance = 0.5f;
        public LayerMask groundLayer;
    }
}
