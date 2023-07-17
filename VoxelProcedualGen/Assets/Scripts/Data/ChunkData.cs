using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkData
{
    public VoxelConf[] voxelList; // list cointaining all blocks in a chunk
    
    //chunk dimentions (similar to older versions of minecraft)
    public int size = 16; 
    public int height = 100;
    
    // reference to the world for calculation purposes
    public Map mapRef;
    public Vector3Int mapPos; //position of a chunk in the Map

    public bool playerMod = false; // if it has been modified by the player

    public ChunkData(int size, int height, Map mapReference, Vector3Int mapPosition)
    {
        this.size = size;
        this.height = height;
        this.mapRef = mapReference;
        this.mapPos = mapPosition;

        voxelList = new VoxelConf[size * size * height];
    }
}
