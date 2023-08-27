using System;
using HexadecimalColor;
using Pool;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Shape
{
    public class ShapeCreator : MonoBehaviour
    {
        public static event Action OnShapeCreate;
        
        [SerializeField] private int _dimensionLength = 5;
        [SerializeField] private Transform _objectToLookAt;
        [SerializeField] private JsonReader _jsonReader;

        private void OnEnable()
        {
            SpawnPointWithoutOverlapSpreader.OnSpawnPoint += CreateShape;
        }

        private void OnDisable()
        {
            SpawnPointWithoutOverlapSpreader.OnSpawnPoint -= CreateShape;
        }

        private void CreateShape(Vector3 positionToSpawn)
        {
            transform.rotation = Quaternion.identity;

            int randomIndex = Random.Range(0, 4);

            switch (randomIndex)
            {
                case 0:
                    CreateSphere();
                    break;
                case 1:
                    CreateRectangle();
                    break;
                case 2:
                    CreatePyramid();
                    break;
                case 3:
                    CreateCube();
                    break;
            }
        
            transform.position = new Vector3(positionToSpawn.x, transform.position.y, positionToSpawn.z);
            transform.LookAt(_objectToLookAt);
            Vector3 eulerAngles = transform.rotation.eulerAngles;
            eulerAngles = new Vector3(0f, eulerAngles.y, 0f);
            transform.rotation = Quaternion.Euler(eulerAngles);
        
            OnShapeCreate?.Invoke();
        }

        private void CreateSphere()
        {
            Vector3 center = new Vector3(0, _dimensionLength - 1, 0);

            for (int x = -_dimensionLength; x < _dimensionLength; x++)
            {
                for (int y = 0; y < _dimensionLength * 2; y++)
                {
                    for (int z = -_dimensionLength; z < _dimensionLength; z++)
                    {
                        Vector3 spawnPosition = new Vector3(x, y, z);
                        float distance = Vector3.Distance(spawnPosition, center);
                        if (distance < _dimensionLength)
                        {
                            CreateBuildingBlock(spawnPosition);
                        }
                    }
                }
            }
        }

        private void CreateRectangle()
        {
            for (int x = 0; x < _dimensionLength * 2; x++)
            {
                for (int y = 0; y < _dimensionLength; y++)
                {
                    for (int z = 0; z < _dimensionLength; z++)
                    {
                        Vector3 spawnPosition = new Vector3(x - _dimensionLength, y, z - _dimensionLength * 0.5f);
                        CreateBuildingBlock(spawnPosition);
                    }
                }
            }
        }

        private void CreatePyramid()
        {
            for (int y = 0; y < _dimensionLength; y++)
            {
                int buildingBlocksInLayer = _dimensionLength - y;

                for (int x = 0; x < buildingBlocksInLayer; x++)
                {
                    for (int z = 0; z < buildingBlocksInLayer; z++)
                    {
                        Vector3 spawnPosition = new Vector3(x - buildingBlocksInLayer * 0.5f, y, z - buildingBlocksInLayer * 0.5f);
                        CreateBuildingBlock(spawnPosition);
                    }
                }
            }
        }

        private void CreateCube()
        {
            for (int x = 0; x < _dimensionLength; x++)
            {
                for (int y = 0; y < _dimensionLength; y++)
                {
                    for (int z = 0; z < _dimensionLength; z++)
                    {
                        Vector3 spawnPosition = new Vector3(x - _dimensionLength * 0.5f, y, z - _dimensionLength * 0.5f);
                        CreateBuildingBlock(spawnPosition);
                    }
                }
            }
        }

        private void CreateBuildingBlock(Vector3 spawnPosition)
        {
            GameObject buildingBlock = ObjectPool.Instance.GetRandomShapeObject();
            buildingBlock.transform.position = spawnPosition;
            buildingBlock.transform.rotation = Quaternion.identity;
            buildingBlock.transform.SetParent(transform);
            buildingBlock.transform.localPosition = spawnPosition;
            buildingBlock.GetComponent<MeshRenderer>().material.color = _jsonReader.GetRandomColor();
            buildingBlock.SetActive(true);
        }
    }
}
