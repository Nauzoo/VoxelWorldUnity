using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public static class ChunkGenerator
{
    private static int chunkVBoundries = ChunkData.chunkHeight;
    private static int chunkHBoundries = ChunkData.chunkHeight;

    private static List<int> bounds = new List<int>() { chunkVBoundries-1, chunkHBoundries-1, 0 };
    private static BlockTypes[,,] chunkLayers = new BlockTypes[chunkVBoundries, chunkHBoundries, chunkHBoundries];

    public static BlockTypes[,,] GenerateChunk(float noiseScale)
    {        
        for (int pilar = 0; pilar < chunkVBoundries; pilar++)
        {
            for (int line = 0; line < chunkHBoundries; line++)
            {
                for (int colon = 0; colon < chunkHBoundries; colon++)
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
