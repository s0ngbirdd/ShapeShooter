using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Shape
{
    public class SpawnPointWithoutOverlapSpreader : MonoBehaviour
    {
        public static event Action<Vector3> OnSpawnPoint;
    
        [Header("SPREAD")]
        [SerializeField] private float _pointXSpread = 20;
        [SerializeField] private float _pointYSpread = 0;
        [SerializeField] private float _pointZSpread = 20;
        [SerializeField] private int _pointToSpreadNumber = 1;
        [SerializeField] private float _timeBeforeSpreadPoint = 3;
    
        [Header("OVERLAP")]
        [SerializeField] private float _raycastDistance = 100;
        [SerializeField] private float _overlapBoxSize = 10;
        [SerializeField] private LayerMask _overlapLayer;

        private void OnEnable()
        {
            CollisionDetector.OnCollisionDetection += WaitToSpreadPoint;
        }

        private void OnDisable()
        {
            CollisionDetector.OnCollisionDetection -= WaitToSpreadPoint;
        }

        private void Start()
        {
            for (int i = 0; i < _pointToSpreadNumber; i++)
            {
                Invoke(nameof(SpreadPoint), 1);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(_pointXSpread * 2, _pointYSpread * 2, _pointZSpread * 2));
        }

        private void SpreadPoint()
        {
            Vector3 randomPosition = new Vector3(Random.Range(-_pointXSpread, _pointXSpread), Random.Range(-_pointYSpread, _pointYSpread), Random.Range(-_pointZSpread, _pointZSpread)) + transform.position;
        
            OverlappingCheck(randomPosition);
        }
    
        private void OverlappingCheck(Vector3 rayStartPosition)
        {
            if (Physics.Raycast(rayStartPosition, Vector3.down, out RaycastHit hit, _raycastDistance))
            {
                Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                Vector3 overlapBoxScale = new Vector3(_overlapBoxSize, _overlapBoxSize, _overlapBoxSize);
                Collider[] collidersInsideOverlapBox = new Collider[1];
                int numberOfCollidersFound = Physics.OverlapBoxNonAlloc(hit.point, overlapBoxScale, collidersInsideOverlapBox, spawnRotation, _overlapLayer);

                if (numberOfCollidersFound == 0)
                {
                    SpawnPoint(hit.point);
                }
                else
                {
                    SpreadPoint();
                }
            }
        }

        private void SpawnPoint(Vector3 positionToSpawn)
        {
            OnSpawnPoint?.Invoke(positionToSpawn);
        }

        private void WaitToSpreadPoint()
        {
            Invoke(nameof(SpreadPoint), _timeBeforeSpreadPoint);
        }
    }
}