using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxDataMannager : MonoBehaviour
{
    public static float textureOffset = 0.001f;
    public static float tileSizeX, tileSizeY;
    public static Dictionary<VoxelConf, TextureData> blockTexDataDictionary =
        new Dictionary<VoxelConf, TextureData>();
    public VoxelDataSO texData;

    private void Awake()
    {
        foreach (var item in texData.texDataList)
        {
            if (blockTexDataDictionary.ContainsKey(item.voxType) == false)
            {
                blockTexDataDictionary.Add(item.voxType, item);
            }
            tileSizeX = texData.textureSizeX;
            tileSizeY = texData.textureSizeX;
        }
    }
}
