using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 1.0f;
    
    private void Update()
    {
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
