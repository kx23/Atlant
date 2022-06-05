using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atlant
{
    public class CollisionSenses : CoreComponent
    {

        #region Check Transform Variables
        [SerializeField] private Transform _groundChecker;
        [SerializeField] private Transform _wallChecker;
        [SerializeField] private Transform _ledgeChecker;
        [SerializeField] private Transform _ceilingChecker;

        public Transform groundChecker { get => _groundChecker; private set => _groundChecker = value; }
        public Transform wallChecker { get => _wallChecker; private set => _wallChecker=value; }
        public Transform ledgeChecker { get => _ledgeChecker; private set =>_ledgeChecker=value; }
        public Transform ceilingChecker { get => ceilingChecker; private set=>_ceilingChecker=value; }




        [SerializeField] private float _groundCheckRadius;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _wallCheckDistance;

        public float groundCheckRadius { get => _groundCheckRadius; private set => _groundCheckRadius = value; }
        public LayerMask groundLayer { get => _groundLayer; private set => _groundLayer = value; }
        public float wallCheckDistance { get => _wallCheckDistance; private set => _wallCheckDistance = value; }

        #endregion


        #region Check Properties
        public bool ceiling
        {
            //Debug.DrawLine(_groundChecker.position, _groundChecker.position+ (Vector3.down*_playerData.groundCheckRadius),Color.red, 1f);
            get => Physics2D.OverlapCircle(_ceilingChecker.position, _groundCheckRadius, _groundLayer);
        }

        public bool ground
        {
            //Debug.DrawLine(_groundChecker.position, _groundChecker.position+ (Vector3.down*_playerData.groundCheckRadius),Color.red, 1f);
            get => Physics2D.OverlapCircle(_groundChecker.position, _groundCheckRadius, _groundLayer);
        }

        public bool wallFront
        {
            get => Physics2D.Raycast(_wallChecker.position, Vector2.right * _core.movement.facingDirection, _wallCheckDistance, _groundLayer);
        }

        public bool ledge
        {
            get => Physics2D.Raycast(_ledgeChecker.position, Vector2.right * _core.movement.facingDirection, _wallCheckDistance, _groundLayer);
        }

        public bool wallBack
        {
            get => Physics2D.Raycast(_wallChecker.position, Vector2.right * -_core.movement.facingDirection, _wallCheckDistance, _groundLayer);
        }
        #endregion

    }
}