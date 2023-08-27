using System;
using System.Collections;
using UnityEngine;

namespace Shape
{
    public class CollisionDetector : MonoBehaviour
    {
        public static event Action OnCollisionDetection;
    
        [SerializeField] private float _explosionForce = 200;
        [SerializeField] private float _timeBeforeDeactivateChild = 2;
    
        private void OnCollisionEnter(Collision other)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.AddComponent<Rigidbody>().AddExplosionForce(_explosionForce, transform.position, _explosionForce * 0.5f);
                StartCoroutine(DeactivateChild(child.gameObject));
            }
        
            OnCollisionDetection?.Invoke();
        }

        private IEnumerator DeactivateChild(GameObject child)
        {
            yield return new WaitForSeconds(_timeBeforeDeactivateChild);
            
            Destroy(child.GetComponent<Rigidbody>());
            child.SetActive(false);
        }
    }
}
