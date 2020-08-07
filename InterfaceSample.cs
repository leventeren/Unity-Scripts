/* IController.cs file */
namespace MyGame
{
    public interface IController
    {
        event System.Action OnStart;
        event System.Action OnStop;
    }
}

/* PlayerController.cs file */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class PlayerController : MonoBehaviour, IController
    {
        public event Action OnStart;
        public event Action OnStop;

        private bool _isTouched = false;

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                OnStart?.Invoke();
            }
            if (Input.GetMouseButtonUp(0))
            {
                OnStop?.Invoke();
            }
            return;
#endif
            if (Input.touches.Length > 0)
            {
                if (!_isTouched)
                {
                    _isTouched = true;
                    OnStart?.Invoke();
                }
            }
            else
            {
                if (_isTouched)
                {
                    _isTouched = false;
                    OnStop?.Invoke();
                }
            }
        }
    }
}

/* Player.cs file */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class Player : MonoBehaviour
    {

        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private GameObject _controlObject;
        [SerializeField] private float _speedMax;

        private float speed = 0;

        public IController _controller;

        private bool _isControled = false;

        private Quaternion _defaultRotation;
        private RigidbodyConstraints _constraints;

        public bool IsControled
        {
            get => _isControled;
            set => _isControled = value;
        }

        // Start is called before the first frame update
        void Start()
        {
            _constraints = _rigidbody.constraints;
            _defaultRotation = transform.rotation;

            _controller = _controlObject.GetComponent<IController>();
            _controller.OnStart += StartMovement;
            _controller.OnStop += StopMovement;
        }

        private void StopMovement()
        {
            speed = 0;
        }

        private void StartMovement()
        {
            speed = _speedMax;
        }

        // Update is called once per frame
        void Update()
        {
            if (_isControled)
            {
                _rigidbody.velocity = Vector3.forward * speed;
            }
        }
    }
}
