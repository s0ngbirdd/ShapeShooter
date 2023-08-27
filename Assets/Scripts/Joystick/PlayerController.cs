using System.Collections.Generic;
using Cinemachine;
using Joystick;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _playerBody;
    [SerializeField] private float _sensitivity = 1;
    [SerializeField] private float _maxVerticalViewAngle = 45;
    
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _transformToSpawn;
    
    //[SerializeField] private CinemachineBrain _cinemachineBrain;

    private PlayerInput _playerInput;
    private float _xRotation;
    
    private List<string> _hexColorStrings;
    private JsonReader _jsonReader;

    private bool _isJoysctickActive = true;

    private void Awake()
    {
        _playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Player.Fire.performed += Fire;

        VirtualCameraSwitcher.OnCamera1Activation += ActivateJoystick;
        //_cinemachineBrain.m_CameraActivatedEvent.AddListener(WaitBeforeJoystickActivation);
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        _playerInput.Player.Fire.performed -= Fire;
        
        VirtualCameraSwitcher.OnCamera1Activation -= ActivateJoystick;
        //_cinemachineBrain.m_CameraActivatedEvent.RemoveListener(WaitBeforeJoystickActivation);
    }

    private void Start()
    {
        _jsonReader = FindObjectOfType<JsonReader>();
    }

    private void Update()
    {
        if (_isJoysctickActive)
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
        if (_isJoysctickActive)
        {
            _isJoysctickActive = false;
            GameObject newBullet = Instantiate(_bulletPrefab, _transformToSpawn.position, _transformToSpawn.rotation);
            newBullet.GetComponent<MeshRenderer>().material.color = _jsonReader.GetRandomColor();
        }
    }

    /*private void WaitBeforeJoystickActivation(ICinemachineCamera arg0, ICinemachineCamera arg1)
    {
        _isJoysctickActive = false;
        
        Invoke(nameof(ActivateJoystick), _cinemachineBrain.m_DefaultBlend.m_Time);
    }*/

    private void ActivateJoystick()
    {
        ResetXRotation();
        
        _isJoysctickActive = true;
    }

    private void ResetXRotation()
    {
        _xRotation = 0;
    }
}
