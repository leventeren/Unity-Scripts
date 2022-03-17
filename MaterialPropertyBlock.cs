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
