using System;
using UnityEngine;

namespace _Scripts
{
    public class GroundCheck : MonoBehaviour
    {
        [SerializeField] private LayerMask _layer;
        [SerializeField] private float _checkDistance = 0.1f;

        [SerializeField] private Collider _collider;
        [SerializeField] private GameObject _player;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        private void Update()
        {
            transform.position = _player.transform.position;
        }

        public bool IsGrounded()
        {
            RaycastHit hitInfo;
            bool ray = Physics.BoxCast(
                _collider.bounds.center,
                _collider.bounds.extents,
                Vector3.down,
                out hitInfo,
                Quaternion.identity,
                _checkDistance,
                _layer
            );
            if (ray)
                Debug.Log(hitInfo.collider.name);

            return ray;
        }

        private void OnDrawGizmos()
        {
            if (_collider == null)
            {
                _collider = GetComponent<BoxCollider>();
            }

            Gizmos.color = Color.red;
            Vector3 boxCenter = _collider.bounds.center;
            Vector3 boxExtents = _collider.bounds.extents;
            Quaternion boxOrientation = Quaternion.identity;

            Gizmos.DrawWireCube(
                boxCenter + Vector3.down * (_checkDistance * 0.5f),
                new Vector3(boxExtents.x * 2, _checkDistance, boxExtents.z * 2)
            );
        }
    }
}