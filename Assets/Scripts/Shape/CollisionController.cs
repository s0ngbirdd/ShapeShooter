using System;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    public static event Action OnCollisionDetection;
    
    private void OnCollisionEnter(Collision other)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.AddComponent<Rigidbody>().AddExplosionForce(200, transform.position, 100);
            //Destroy(child.gameObject, 2);
            //GetComponent<Rigidbody>().AddExplosionForce(500, transform.position, 500);
        }
        
        OnCollisionDetection?.Invoke();
    }
}
