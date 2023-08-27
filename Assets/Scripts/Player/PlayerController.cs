using HexadecimalColor;
using Shape;
using UnityEngine;
using UnityEngine.InputSystem;
using VirtualCamera;
using Pool;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("ROTATION")]
        [SerializeField] private Transform _playerBody;
        [SerializeField] private float _sensitivity = 70;
        [SerializeField] private float _maxVerticalViewAngle = 45;
    
        [Header("FIRE")]
        [SerializeField] private Transform _bulletSpawnPoint;
        [SerializeField] private JsonReader _jsonReader;

        private PlayerInput _playerInput;
        private float _xRotation;
        private bool _isInputActive;

        private void Awake()
        {
            _playerInput = new PlayerInput();
        }

        private void OnEnable()
        {
            _playerInput.Enable();
            _playerInput.Player.Fire.performed += Fire;

            VirtualCameraSwitcher.OnVirtualCamera1Activation += ActivateInput;
            CollisionDetector.OnCollisionDetection += DeactivateInput;
        }

        private void OnDisable()
        {
            _playerInput.Disable();
            _playerInput.Player.Fire.performed -= Fire;
        
            VirtualCameraSwitcher.OnVirtualCamera1Activation -= ActivateInput;
            CollisionDetector.OnCollisionDetection -= DeactivateInput;
        }

        private void Update()
        {
            if (_isInputActive)
            {
                Rotate();
            }
        }

        private void Rotate()
        {
            Vector2 delta = _playerInput.Player.Rotate.ReadValue<Vector2>();
            float joystickX = delta.x * _sensitivity * Time.deltaTime;
            float joystickY = delta.y * _sensitivity * Time.deltaTime;

            _xRotation -= joystickY;
            _xRotation = Mathf.Clamp(_xRotation, -_maxVerticalViewAngle, _maxVerticalViewAngle);

            transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
            _playerBody.Rotate(Vector3.up * joystickX);
        }

        private void Fire(InputAction.CallbackContext obj)
        {
            if (_isInputActive)
            {
                GameObject bullet = ObjectPool.Instance.GetBulletPooledObject();//Instantiate(_bulletPrefab, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation);
                bullet.transform.position = _bulletSpawnPoint.position;
                bullet.transform.rotation = _bulletSpawnPoint.rotation;
                bullet.GetComponent<MeshRenderer>().material.color = _jsonReader.GetRandomColor();
                bullet.SetActive(true);
            }
        }

        private void ActivateInput()
        {
            _xRotation = 0;
            _isInputActive = true;
        }

        private void DeactivateInput()
        {
            _isInputActive = false;
        }
    }
}
