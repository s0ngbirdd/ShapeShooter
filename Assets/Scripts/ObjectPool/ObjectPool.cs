using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool Instance;
        
        [SerializeField] private GameObject _cubePrefab;
        [SerializeField] private GameObject _spherePrefab;
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private int _poolNumber;

        private List<GameObject> _cubePooledObjects;
        private List<GameObject> _spherePooledObjects;
        private List<GameObject> _bulletPooledObjects;
    
        private GameObject tempObject;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        private void Start()
        {
            _cubePooledObjects = new List<GameObject>();
            _spherePooledObjects = new List<GameObject>();
            _bulletPooledObjects = new List<GameObject>();
        
            InstantiateObjects(_poolNumber, _cubePrefab, _cubePooledObjects);
            InstantiateObjects(_poolNumber, _spherePrefab, _spherePooledObjects);
            InstantiateObjects(_poolNumber, _bulletPrefab, _bulletPooledObjects);
        }

        public GameObject GetRandomShapeObject()
        {
            int randomIndex = Random.Range(0, 2);

            return randomIndex == 0 ? GetCubePooledObject() : GetSpherePooledObject();
        }
    
        public GameObject GetCubePooledObject()
        {
            for (int i = 0; i < _cubePooledObjects.Count; i++)
            {
                if (!_cubePooledObjects[i].activeInHierarchy)
                {
                    return _cubePooledObjects[i];
                }
            }

            return InstantiateObject(_cubePrefab, _cubePooledObjects);
        }
        
        public GameObject GetSpherePooledObject()
        {
            for (int i = 0; i < _spherePooledObjects.Count; i++)
            {
                if (!_spherePooledObjects[i].activeInHierarchy)
                {
                    return _spherePooledObjects[i];
                }
            }

            return InstantiateObject(_spherePrefab, _spherePooledObjects);
        }
        
        public GameObject GetBulletPooledObject()
        {
            for (int i = 0; i < _bulletPooledObjects.Count; i++)
            {
                if (!_bulletPooledObjects[i].activeInHierarchy)
                {
                    return _bulletPooledObjects[i];
                }
            }

            return InstantiateObject(_bulletPrefab, _bulletPooledObjects);
        }

        private void InstantiateObjects(int instantiateNumber, GameObject fruit, List<GameObject> fruitPool)
        {
            for (int i = 0; i < instantiateNumber; i++)
            {
                tempObject = Instantiate(fruit, transform);
                tempObject.SetActive(false);
                fruitPool.Add(tempObject);
            }
        }
    
        private GameObject InstantiateObject(GameObject fruit, List<GameObject> fruitPool)
        {
            tempObject = Instantiate(fruit, transform);
            tempObject.SetActive(false);
            fruitPool.Add(tempObject);
        
            return tempObject;
        }
    }
}
