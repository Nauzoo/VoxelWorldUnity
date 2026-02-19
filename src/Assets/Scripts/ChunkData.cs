using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ChunkData
{
    public readonly static int chunkSize = 16;
    public readonly static int chunkHeight = 128;    

    public readonly static int borderSize = chunkSize + 2;
    public readonly static int borderHeight = chunkHeight + 2;

    public static readonly List<int> chunkBounds = new List<int>() { ChunkData.borderSize - 1, 0 };

    public static int maxTerrainHight = 0;
    public static int minTerrainHight = 0;
}
