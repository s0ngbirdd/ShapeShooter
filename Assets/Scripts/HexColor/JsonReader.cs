using System.Collections.Generic;
using UnityEngine;

public class JsonReader : MonoBehaviour
{
    [SerializeField] private TextAsset _jsonFile;

    private List<string> _hexColorStrings;

    private void Start()
    {
        _hexColorStrings = new List<string>();
        
        HexColors hexColorsInJson = JsonUtility.FromJson<HexColors>(_jsonFile.text);

        foreach (HexColor hexColor in hexColorsInJson.hexColors)
        {
            _hexColorStrings.Add(hexColor.hex);
        }
    }

    public Color GetRandomColor()
    {
        return HexadecimalStringToColorСonverter.GetColorFromString(_hexColorStrings[Random.Range(0, _hexColorStrings.Count)]);
    }
}
