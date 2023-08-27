using Cinemachine;
using Unity.Plastic.Newtonsoft.Json.Serialization;
using UnityEngine;

namespace Joystick
{
    [RequireComponent(typeof(CinemachineBrain))]
    public class VirtualCameraSwitcher : MonoBehaviour
    {
        public static event Action OnCamera1Activation; 

        [SerializeField] private GameObject _camera1;
        [SerializeField] private GameObject _camera2;

        [SerializeField] private Transform _playerTransform;
        [SerializeField] private Transform _playerCameraTargeTransform;

        private CinemachineBrain _cinemachineBrain;

        private void Awake()
        {
            _cinemachineBrain = GetComponent<CinemachineBrain>();
        }

        private void OnEnable()
        {
            ShapeCreator.OnShapeCreated += ActivateCamera2;
            _cinemachineBrain.m_CameraActivatedEvent.AddListener(WaitBeforeActivateCamera1);
        }

        private void OnDisable()
        {
            ShapeCreator.OnShapeCreated += ActivateCamera2;
            _cinemachineBrain.m_CameraActivatedEvent.RemoveListener(WaitBeforeActivateCamera1);
        }

        private void Update()
        {
            //Debug.Log(_cinemachineBrain.ActiveBlend);
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ActivateCamera2();
            }
            /*else if(Input.GetKeyDown(KeyCode.Q))
            {
                ActivateCamera2();
            }*/
        }

        private void ActivateCamera1()
        {
            _playerTransform.rotation = _camera2.transform.rotation;
            
            /*Vector3 _eulerAngles = new Vector3(_camera2.transform.rotation.x, 0, _camera2.transform.rotation.z);
            _playerTransform.rotation = Quaternion.Euler(_eulerAngles);*/
            
            _camera2.SetActive(false);
            //Camera.main.transform.rotation = _camera2.transform.rotation;
            _camera1.SetActive(true);
            
            OnCamera1Activation?.Invoke();
        }
        
        private void ActivateCamera2()
        {
            _camera1.SetActive(false);
            //Camera.main.transform.rotation = _camera1.transform.rotation;
            _camera2.SetActive(true);
            //_camera1.transform.rotation = new Quaternion(_playerTransform.rotation.x, _playerCameraTargeTransform.rotation.y, 0, 0);
            //_camera1.transform.rotation = _camera2.transform.rotation;
            
        }

        private void WaitBeforeActivateCamera1(ICinemachineCamera arg0, ICinemachineCamera arg1)
        {
            /*Invoke(nameof(ActivateCamera1), _cinemachineBrain.m_DefaultBlend.m_Time);
            Debug.Log("WAIT");*/
            
            if (!_camera1.activeSelf)
            {
                Invoke(nameof(ActivateCamera1), _cinemachineBrain.m_DefaultBlend.m_Time);

            }
            else
            {
                ActivateCamera1();
            }
            
        }
    }
}
