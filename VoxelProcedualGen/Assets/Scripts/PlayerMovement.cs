using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 15;

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        if (Input.GetKey(KeyCode.Space))
            direction.y = 1;
        else if (Input.GetKey(KeyCode.LeftShift))
            direction.y = -1;

        direction = direction.normalized;
        transform.position += direction * speed * Time.deltaTime;
    }
}
