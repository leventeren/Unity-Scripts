using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialPropertyBlock : MonoBehaviour
{
  [SerializedField] private Color mainColor = Color.black;
  private Renderer _renderer = null;
  private MaterialPropertyBlock _materialPropertyBlock = null;

  private void Start(){
    _renderer = GetComponent<Renderer>();
    _materialPropertyBlock = new MaterialPropertyBlock();
  }

  private void Update(){
    _materialPropertyBlock.SetColor("_Color", mainColor);
    _renderer.SetPropertyBlock(_materialPropertyBlock);
  }
}

// Add new shader -> add to head [PerRendererData] tag to property
// eg. [PerRendererData] _Color ("Color", Color) = (1,1,1,1)

// Sample Case:
/*
public class MeshBall : MonoBehaviour
{
    static int baseColorId = Shader.PropertyToID("_BaseColor");
    [SerializeField]
    Mesh ballmesh = default;
    [SerializeField]
    Material instancedMaterial = default;

    Matrix4x4[] matrices = new Matrix4x4[1023];

    Vector4[] colors = new Vector4[1023];

    MaterialPropertyBlock materialBlock;

    // Update is called once per frame
    void Awake()
    {
        for (int i = 0; i < matrices.Length; i++)
        {
            matrices[i] = Matrix4x4.TRS(Random.insideUnitSphere * 50f, Quaternion.identity, Vector3.one);
            colors[i] = new Vector4(Random.value, Random.value, Random.value, Random.Range(0.5f, 1f));
        }
    }


    void Update()
    {
        if (materialBlock == null)
        {
            materialBlock = new MaterialPropertyBlock();
            materialBlock.SetVectorArray(baseColorId, colors);
        }

        Graphics.DrawMeshInstanced(ballmesh, 0, instancedMaterial, matrices, 1023, materialBlock);
    }
}
*/
