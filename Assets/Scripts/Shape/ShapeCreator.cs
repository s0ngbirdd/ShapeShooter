using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShapeCreator : MonoBehaviour
{
    [SerializeField] private List<GameObject> _buildingBlockPrefabs;
    [SerializeField] private int _dimensionLength = 5;
    [SerializeField] private Transform _objectToLookAt;

    private List<string> _hexColorStrings;
    private JsonReader _jsonReader;

    private void Start()
    {
        _jsonReader = FindObjectOfType<JsonReader>();
        SetHexColorStrings(_jsonReader.HexColorStrings);
        CreateShape();
    }

    private void CreateShape()
    {
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
        
        transform.LookAt(_objectToLookAt);
    }

    private void CreateSphere()
    {
        Vector3 center = new Vector3(_dimensionLength - 1, _dimensionLength - 1, _dimensionLength - 1);

        for (int x = 0; x < _dimensionLength * 2; x++)
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
                        buildingBlock.GetComponent<MeshRenderer>().material.color = GetRandomColor();
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
                    Vector3 spawnPosition = new Vector3(x, y, z);
                    GameObject buildingBlock = Instantiate(_buildingBlockPrefabs[Random.Range(0, _buildingBlockPrefabs.Count)], spawnPosition, Quaternion.identity);
                    buildingBlock.transform.SetParent(transform);
                    buildingBlock.transform.localPosition = spawnPosition;
                    buildingBlock.GetComponent<MeshRenderer>().material.color = GetRandomColor();
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
                    buildingBlock.GetComponent<MeshRenderer>().material.color = GetRandomColor();
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
                    Vector3 spawnPosition = new Vector3(x, y, z);
                    GameObject buildingBlock = Instantiate(_buildingBlockPrefabs[Random.Range(0, _buildingBlockPrefabs.Count)], spawnPosition, Quaternion.identity);
                    buildingBlock.transform.SetParent(transform);
                    buildingBlock.transform.localPosition = spawnPosition;
                    buildingBlock.GetComponent<MeshRenderer>().material.color = GetRandomColor();
                }
            }
        }
    }

    private void SetHexColorStrings(List<string> hexColorStrings)
    {
        _hexColorStrings = new List<string>(hexColorStrings);
    }
    
    private Color GetRandomColor()
    {
        return HexadecimalStringToColorСonverter.GetColorFromString(_hexColorStrings[Random.Range(0, _hexColorStrings.Count)]);
    }
}
