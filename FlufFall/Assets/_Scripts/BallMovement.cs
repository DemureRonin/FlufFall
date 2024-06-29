using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using TouchPhase = UnityEngine.TouchPhase;

namespace _Scripts
{
    public class BallMovement : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        [SerializeField] private float _speed;
        [SerializeField] private float _rbSpeed;
        [SerializeField] private float _smoothTime = 0.2f;

        [SerializeField] private Rigidbody _rigidbody;
        private float _initialY;
        [SerializeField] private GroundCheck _groundCheck;
        [SerializeField] private UnityEvent _onDie;
        [SerializeField] private UnityEvent _onStart;
        private bool _dead;
        private bool _phoneControls;
        private bool _isDragging;
        private Vector3 _lastTouchPosition;
        private Vector3 _velocity = Vector3.zero;
        private Vector3 _targetPosition;
        private Vector3 _vector;

        private void Start()
        {
            _initialY = transform.position.y;
        }

        private void OnStart()
        {
            _onStart?.Invoke();
        }


        private void Update()
        {
            if (Input.touches.Length > 0)
            {
                _phoneControls = true;
            }

            if (!ProcLevelCreator.Started) return;
            if (_dead) return;
            if (_groundCheck.IsGrounded())
            {
                _onDie?.Invoke();
                _dead = true;
            }
        }

        void FixedUpdate()
        {
            if (!ProcLevelCreator.Started) return;
            if (_dead) return;

            if (!_phoneControls)
            {
                _vector = Input.mousePosition;
                _vector.z = 10;
                _vector = _camera.ScreenToWorldPoint(_vector);


                Vector3 velocity = (_vector.normalized) * _rbSpeed;
                velocity = new Vector3(velocity.x, 0, velocity.z);
                _rigidbody.velocity = velocity;
            }
            else
            {
                if (Input.touches.Length > 0)
                {
                    var direction = Gamepad.current.rightStick.ReadValue();
                    var velocity = new Vector3(direction.x, 0, direction.y) * _rbSpeed;
                    _rigidbody.velocity = velocity;
                }
            }
        }

        private void OnEnable()
        {
            ProcLevelCreator.OnStart += OnStart;
        }


        private void OnDisable()
        {
            ProcLevelCreator.OnStart -= OnStart;
        }
    }
}