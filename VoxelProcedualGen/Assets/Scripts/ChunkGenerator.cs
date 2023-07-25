using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public static class ChunkGenerator
{
    public static BlockTypes[,,] GenerateChunk(float noiseScale)
    {
        int MaxH = ChunkData.borderHeight;
        int MaxS = ChunkData.borderSize;

        List<int> bounds = new List<int>() { MaxH - 1, MaxS - 1, 0 };
        BlockTypes[,,] chunkLayers = new BlockTypes[MaxH, MaxS, MaxS];

        for (int pilar = 0; pilar < MaxH; pilar++)
        {
            for (int line = 0; line < MaxS; line++)
            {
                for (int colon = 0; colon < MaxS; colon++)
                { 
                    if (bounds.Contains(pilar)) chunkLayers[pilar, line, colon] = BlockTypes.None;
                    else if (bounds.Contains(line)) chunkLayers[pilar, line, colon] = BlockTypes.None;
                    else if (bounds.Contains(colon)) chunkLayers[pilar, line, colon] = BlockTypes.None;
                    
                    else chunkLayers[pilar, line, colon] = noise.snoise(new float3((float)line, (float)pilar, (float)colon) / noiseScale) >= 0 ? BlockTypes.Solid : BlockTypes.Air;
                }
            }
        }

        return chunkLayers;
    }
}
