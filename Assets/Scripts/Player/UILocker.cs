using System.Collections.Generic;
using Shape;
using UnityEngine;
using UnityEngine.UI;
using VirtualCamera;

namespace Player
{
    public class UILocker : MonoBehaviour
    {
        [SerializeField] private List<Image> _images;

        private void OnEnable()
        {
            VirtualCameraSwitcher.OnVirtualCamera1Activation += UnlockImages;
            CollisionDetector.OnCollisionDetection += LockImages;
        }

        private void OnDisable()
        {
            VirtualCameraSwitcher.OnVirtualCamera1Activation -= UnlockImages;
            CollisionDetector.OnCollisionDetection -= LockImages;
        }

        private void Start()
        {
            LockImages();
        }

        private void LockImages()
        {
            foreach (Image image in _images)
            {
                image.color = Color.grey;
            }
        }

        private void UnlockImages()
        {
            foreach (Image image in _images)
            {
                image.color = Color.white;
            }
        }
    }
}
