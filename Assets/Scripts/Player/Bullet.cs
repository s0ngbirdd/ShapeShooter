using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _speed = 20;
        [SerializeField] private float _timeBeforeDeactivate = 2;

        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            Invoke(nameof(DeactivateSelf), _timeBeforeDeactivate);
        }

        private void FixedUpdate()
        {
            _rigidbody.MovePosition(transform.position + transform.forward * (_speed * Time.deltaTime));
        }

        private void OnCollisionEnter(Collision collision)
        {
            DeactivateSelf();
        }

        private void DeactivateSelf()
        {
            _rigidbody.velocity = Vector3.zero;
            gameObject.SetActive(false);
        }
    }
}
