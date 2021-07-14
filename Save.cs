using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    public List<Vector3Serializable> targetPositions = new List<Vector3Serializable>();
    public List<QuaternionSerializable> targetRotation = new List<QuaternionSerializable>();
}
[System.Serializable]
public struct Vector3Serializable
{
    public float x;
    public float y;
    public float z;
}
[System.Serializable]
public struct QuaternionSerializable
{
    public float x;
    public float y;
    public float z;
    public float w;
}
