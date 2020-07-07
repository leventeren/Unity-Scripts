using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorToPrefab
{
    public Color color;
    public GameObject prefab;
    public float PosY;
}

public class MapGenerator : MonoBehaviour
{
    public List<Texture2D> LevelImages = new List<Texture2D>();

    public List<ColorToPrefab> ColorMaps = new List<ColorToPrefab>();

    public Transform LevelMap;

    private void Start()
    {
        GenerateMap(0);
    }

    private void GenerateMap(int index)
    {
        for (int x = 0; x < LevelImages[index].width; x++)
        {
            for (int z = 0; z < LevelImages[index].height; z++)
            {
                GenerateTile(x, z, index);
            }
        }
    }

    private void GenerateTile(int x, int z, int index)
    {
        Color pixelColor = LevelImages[index].GetPixel(x, z);

        if (pixelColor.a != 0)
        {
            foreach (ColorToPrefab ColorMap in ColorMaps)
            {
                if (ColorMap.color.Equals(pixelColor))
                {
                    Vector3 tilePos = new Vector3(x, ColorMap.PosY, z);
                    GameObject tile = Instantiate(ColorMap.prefab, tilePos, Quaternion.identity, LevelMap);
                    break;
                }
            }
        }
    }
}
