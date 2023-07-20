using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public static class ChunkGenerator
{
    private static BlockTypes[,,] chunkLayers = new BlockTypes[ChunkData.chunkHeight, ChunkData.chunkSize, ChunkData.chunkSize];

    public static BlockTypes[,,] GenerateChunk(float noiseScale)
    {
        for (int pilar = 0; pilar < ChunkData.chunkHeight; pilar++)
        {
            for (int line = 0; line < ChunkData.chunkSize; line++)
            {
                for (int colon = 0; colon < ChunkData.chunkSize; colon++)
                {
                    chunkLayers[pilar, line, colon] = noise.snoise(new float3((float)line, (float)pilar, (float)colon) / noiseScale) >= 0 ? BlockTypes.Solid : BlockTypes.Air;
                }
            }
        }

        //chunkLayers[3, 5, 9] = BlockTypes.Solid;
        return chunkLayers;
    }
}
