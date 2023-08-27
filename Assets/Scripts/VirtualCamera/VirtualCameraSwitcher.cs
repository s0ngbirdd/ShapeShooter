using System;
using Cinemachine;
using Shape;
using UnityEngine;

namespace VirtualCamera
{
    [RequireComponent(typeof(CinemachineBrain))]
    public class VirtualCameraSwitcher : MonoBehaviour
    {
        public static event Action OnVirtualCamera1Activation; 

        [SerializeField] private GameObject _virtualCamera1;
        [SerializeField] private GameObject _virtualCamera2;
        [SerializeField] private Transform _playerTransform;

        private CinemachineBrain _cinemachineBrain;

        private void Awake()
        {
            _cinemachineBrain = GetComponent<CinemachineBrain>();
        }

        private void OnEnable()
        {
            ShapeCreator.OnShapeCreate += ActivateVirtualCamera2;
            _cinemachineBrain.m_CameraActivatedEvent.AddListener(WaitBeforeActivateVirtualCamera1);
        }

        private void OnDisable()
        {
            ShapeCreator.OnShapeCreate += ActivateVirtualCamera2;
            _cinemachineBrain.m_CameraActivatedEvent.RemoveListener(WaitBeforeActivateVirtualCamera1);
        }

        private void ActivateVirtualCamera1()
        {
            _playerTransform.rotation = _virtualCamera2.transform.rotation;

            _virtualCamera2.SetActive(false);
            _virtualCamera1.SetActive(true);
            
            OnVirtualCamera1Activation?.Invoke();
        }
        
        private void ActivateVirtualCamera2()
        {
            _virtualCamera1.SetActive(false);
            _virtualCamera2.SetActive(true);

        }

        private void WaitBeforeActivateVirtualCamera1(ICinemachineCamera arg0, ICinemachineCamera arg1)
        {
            if (!_virtualCamera1.activeSelf)
            {
                Invoke(nameof(ActivateVirtualCamera1), _cinemachineBrain.m_DefaultBlend.m_Time);

            }
            else
            {
                ActivateVirtualCamera1();
            }
        }
    }
}
