using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomNoise
{
    public static float[,] GenerateRandomMap(int mapSize)
    {
        float[,] noisemap = new float[mapSize, mapSize];
        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                noisemap[x, y] = Random.Range(1, 9) / 10f;
            }
        }

        return noisemap;
    }
}