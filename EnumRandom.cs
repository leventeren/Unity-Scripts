//GameManager => 
// [Header("Color Settings")]
// public Color32[] itemColors;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeItem : MonoBehaviour
{
    public enum CubeColor
    {
        Color0 = 0,
        Color1 = 1,
        Color2 = 2,
        Color3 = 3
    }
    [Header("Color Settings")]
    public CubeColor m_CubeColorType = CubeColor.Color1;
    public MeshRenderer m_meshRenderer;

    void Start()
    {
        m_CubeColorType = (CubeColor)Random.Range(0, Enum.GetValues(typeof(CubeColor)).Length);
        Debug.Log(m_CubeColorType);

        switch (m_CubeColorType)
        {
            case CubeColor.Color0:
                m_meshRenderer.material.SetColor("_Color", GameManager.instance.itemColors[0]);
                break;
            case CubeColor.Color1:
                m_meshRenderer.material.SetColor("_Color", GameManager.instance.itemColors[1]);
                break;
            case CubeColor.Color2:
                m_meshRenderer.material.SetColor("_Color", GameManager.instance.itemColors[2]);
                break;
            case CubeColor.Color3:
                m_meshRenderer.material.SetColor("_Color", GameManager.instance.itemColors[3]);
                break;
        }
    }
}
