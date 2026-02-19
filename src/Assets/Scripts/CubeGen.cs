using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class CubeGen : MonoBehaviour
{
    [SerializeField] GameObject cubePrefab;
    [SerializeField] int radius;
    
    
    

    void Start()
    {


        //StartCoroutine(generateShape());
        generateShape();
    }
    
    void generateShape()
    {
        Vector3 center = Vector3.zero;
        for (int y = -radius; y < radius; y++)
        {
            for (int x = -radius; x < radius; x++)
            {
                for (int z = -radius; z < radius; z++)
                {
                    Vector3 currentPos = new Vector3(y, x, z);
                    float distance = Vector3.Distance(currentPos, center);
                    cubePrefab.transform.position = new Vector3(x, y, z);
                    if (distance < radius)
                        Instantiate(cubePrefab, this.transform);
                    //yield return new WaitForSeconds(0f);
                }
            }
        }
    }    

}
