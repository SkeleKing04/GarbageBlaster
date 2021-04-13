using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinMovement : MonoBehaviour
{
    public float size;
    public float speed;
    private float x_pos;
    // Start is called before the first frame update
    void Start()
    {
        x_pos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(x_pos, 1, Mathf.Sin(Time.time * speed) * size);
    }
}
