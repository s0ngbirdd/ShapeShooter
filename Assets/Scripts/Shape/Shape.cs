using System;
using UnityEngine;

public class Shape : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        //Destroy(gameObject);
        Destroy(gameObject, 2);
    }
    
    
}
