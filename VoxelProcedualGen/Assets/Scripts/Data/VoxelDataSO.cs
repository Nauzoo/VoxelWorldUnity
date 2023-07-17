using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Block Data", menuName = "Data/Block Data")]
public class VoxelDataSO : ScriptableObject
{
    public float textureSizeX, textureSizeY;
    public List<TextureData> texDataList;
}
[Serializable]
public class TextureData {
    public VoxelConf voxType;
    public Vector2 up, down, side;
    public bool isSolid = true;
    public bool isCollideable = true;
}

