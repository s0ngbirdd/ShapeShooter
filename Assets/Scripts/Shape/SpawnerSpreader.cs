using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerSpreader : MonoBehaviour
{
    public static event Action<Vector3> OnSetSpawnPoint;
    
    //[SerializeField] private GameObject _objectToSpawn;
    [SerializeField] private float _objectXSpread = 10;
    [SerializeField] private float _objectYSpread = 0;
    [SerializeField] private float _objectZSpread = 10;
    [SerializeField] private int _objectToSpreadNumber = 1;
    
    //[SerializeField] private float _objectToSpawnYOffset = 0.5f;
    [SerializeField] private float _raycastDistance = 100;
    [SerializeField] private float _overlapBoxSize = 0.5f;
    [SerializeField] private LayerMask _spawnedObjectLayer;

    private void OnEnable()
    {
        CollisionController.OnCollisionDetection += WaitToSpreadObject;
    }

    private void OnDisable()
    {
        CollisionController.OnCollisionDetection -= WaitToSpreadObject;
    }

    private void Start()
    {
        for (int i = 0; i < _objectToSpreadNumber; i++)
        {
            Invoke(nameof(SpreadObject), 1);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(_objectXSpread * 2, _objectYSpread * 2, _objectZSpread * 2));
    }

    private void SpreadObject()
    {
        Vector3 randomPosition = new Vector3(Random.Range(-_objectXSpread, _objectXSpread), Random.Range(-_objectYSpread, _objectYSpread), Random.Range(-_objectZSpread, _objectZSpread)) + transform.position;
        
        ObjectOverlappingCheck(randomPosition);
    }
    
    private void ObjectOverlappingCheck(Vector3 rayStartPosition)
    {
        if (Physics.Raycast(rayStartPosition, Vector3.down, out RaycastHit hit, _raycastDistance))
        {
            Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            Vector3 overlapTestBoxScale = new Vector3(_overlapBoxSize, _overlapBoxSize, _overlapBoxSize);
            Collider[] collidersInsideOverlapBox = new Collider[1];
            int numberOfCollidersFound = Physics.OverlapBoxNonAlloc(hit.point, overlapTestBoxScale, collidersInsideOverlapBox, spawnRotation, _spawnedObjectLayer);

            if (numberOfCollidersFound == 0)
            {
                SpawnObject(hit.point, spawnRotation);
            }
            else
            {
                SpreadObject();
            }
        }
    }

    private void SpawnObject(Vector3 positionToSpawn, Quaternion rotationToSpawn)
    {
        OnSetSpawnPoint?.Invoke(positionToSpawn);
        //GameObject obj = Instantiate(_objectToSpawn, positionToSpawn + new Vector3(0, _objectToSpawnYOffset, 0), rotationToSpawn);
    }

    private void WaitToSpreadObject()
    {
        Invoke(nameof(SpreadObject), 3);
    }
}