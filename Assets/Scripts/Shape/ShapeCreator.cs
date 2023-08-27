using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShapeCreator : MonoBehaviour
{
    public static event Action OnShapeCreated;
    
    [SerializeField] private List<GameObject> _buildingBlockPrefabs;
    [SerializeField] private int _dimensionLength = 5;
    [SerializeField] private Transform _objectToLookAt;

    private List<string> _hexColorStrings;
    private JsonReader _jsonReader;
    
    private Vector3 _eulerAngles;

    private void OnEnable()
    {
        //CollisionController.OnCollisionDetection += WaitToCreateShape;
        SpawnerSpreader.OnSetSpawnPoint += CreateShape;
    }

    private void OnDisable()
    {
        //CollisionController.OnCollisionDetection -= WaitToCreateShape;
        SpawnerSpreader.OnSetSpawnPoint -= CreateShape;
    }

    private void Start()
    {
        _jsonReader = FindObjectOfType<JsonReader>();
        //CreateShape();
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
        _eulerAngles = transform.rotation.eulerAngles;
        _eulerAngles = new Vector3(0f, _eulerAngles.y, 0f);
        transform.rotation = Quaternion.Euler(_eulerAngles);
        
        OnShapeCreated?.Invoke();
    }

    private void CreateSphere()
    {
        //Vector3 center = new Vector3(_dimensionLength - 1, _dimensionLength - 1, _dimensionLength - 1);
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
                        GameObject buildingBlock = Instantiate(_buildingBlockPrefabs[Random.Range(0, _buildingBlockPrefabs.Count)], spawnPosition, Quaternion.identity);
                        buildingBlock.transform.SetParent(transform);
                        buildingBlock.transform.localPosition = spawnPosition;
                        buildingBlock.GetComponent<MeshRenderer>().material.color = _jsonReader.GetRandomColor();
                    }
                }
            }
        }
        
        /*for (int x = 0; x < _dimensionLength * 2; x++)
        {
            for (int y = 0; y < _dimensionLength * 2; y++)
            {
                for (int z = 0; z < _dimensionLength * 2; z++)
                {
                    Vector3 spawnPosition = new Vector3(x, y, z);
                    float distance = Vector3.Distance(spawnPosition, center);
                    if (distance < _dimensionLength)
                    {
                        GameObject buildingBlock = Instantiate(_buildingBlockPrefabs[Random.Range(0, _buildingBlockPrefabs.Count)], spawnPosition, Quaternion.identity);
                        buildingBlock.transform.SetParent(transform);
                        buildingBlock.transform.localPosition = spawnPosition;
                        buildingBlock.GetComponent<MeshRenderer>().material.color = _jsonReader.GetRandomColor();
                    }
                }
            }
        }*/
        
        /*int middleLayer = Mathf.CeilToInt(_dimensionLength / 2f);
        Debug.Log(middleLayer);
        int cubesInLayer = 2;
        
        for (int y = 0; y < _dimensionLength; y++)
        {
            if (y < middleLayer)
            {
                cubesInLayer += y;
            }
            else
            {
                cubesInLayer -= y;
            }
            
            float layerOffset = -cubesInLayer * 0.5f;
            
            for (int x = 0; x < cubesInLayer; x++)
            {
                for (int z = 0; z < cubesInLayer; z++)
                {
                    Vector3 spawnPosition = new Vector3(x + layerOffset, y, z + layerOffset);
                    GameObject buildingBlock = Instantiate(_buildingBlockPrefabs[Random.Range(0, _buildingBlockPrefabs.Count)], spawnPosition, Quaternion.identity);
                    buildingBlock.transform.SetParent(transform);
                    buildingBlock.transform.localPosition = spawnPosition;
                    buildingBlock.GetComponent<MeshRenderer>().material.color = _jsonReader.GetRandomColor();
                }
            }
        }*/
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
                    GameObject buildingBlock = Instantiate(_buildingBlockPrefabs[Random.Range(0, _buildingBlockPrefabs.Count)], spawnPosition, Quaternion.identity);
                    buildingBlock.transform.SetParent(transform);
                    buildingBlock.transform.localPosition = spawnPosition;
                    buildingBlock.GetComponent<MeshRenderer>().material.color = _jsonReader.GetRandomColor();
                }
            }
        }
    }

    private void CreatePyramid()
    {
        for (int y = 0; y < _dimensionLength; y++)
        {
            int cubesInLayer = _dimensionLength - y;
            float layerOffset = -cubesInLayer * 0.5f;

            for (int x = 0; x < cubesInLayer; x++)
            {
                for (int z = 0; z < cubesInLayer; z++)
                {
                    Vector3 spawnPosition = new Vector3(x + layerOffset, y, z + layerOffset);
                    GameObject buildingBlock  = Instantiate(_buildingBlockPrefabs[Random.Range(0, _buildingBlockPrefabs.Count)], spawnPosition, Quaternion.identity);
                    buildingBlock .transform.SetParent(transform);
                    buildingBlock.transform.localPosition = spawnPosition;
                    buildingBlock.GetComponent<MeshRenderer>().material.color = _jsonReader.GetRandomColor();
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
                    GameObject buildingBlock = Instantiate(_buildingBlockPrefabs[Random.Range(0, _buildingBlockPrefabs.Count)], spawnPosition, Quaternion.identity);
                    buildingBlock.transform.SetParent(transform);
                    buildingBlock.transform.localPosition = spawnPosition;
                    buildingBlock.GetComponent<MeshRenderer>().material.color = _jsonReader.GetRandomColor();
                }
            }
        }
    }

    /*private void WaitToCreateShape()
    {
        Invoke(nameof(CreateShape), 3);
    }*/
}
