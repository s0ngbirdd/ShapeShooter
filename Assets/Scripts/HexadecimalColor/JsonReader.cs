using System.Collections.Generic;
using UnityEngine;

namespace HexadecimalColor
{
    public class JsonReader : MonoBehaviour
    {
        [SerializeField] private TextAsset _jsonFile;
    
        private List<Color> _colors;

        private void Start()
        {
            _colors = new List<Color>();
        
            HexColors hexColorsInJson = JsonUtility.FromJson<HexColors>(_jsonFile.text);

            foreach (HexColor hexColor in hexColorsInJson.hexColors)
            {
                _colors.Add(HexadecimalStringToColorСonverter.GetColorFromString(hexColor.hex));
            }
        }

        public Color GetRandomColor()
        {
            return _colors[Random.Range(0, _colors.Count)];
        }
    }
}
