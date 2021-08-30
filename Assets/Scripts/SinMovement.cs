using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinMovement : MonoBehaviour
{
    public float size;
    public float speed;
    private float x_pos;
    private float y_pos;
    private float z_pos;
    public enum e_MoveableAxes
    {
        x,
        y,
        z,
        xy,
        xz,
        yz,
        xyz
    };
    public e_MoveableAxes m_MoveableAxes;
    // Start is called before the first frame update
    void Start()
    {
        x_pos = transform.position.x;
        y_pos = transform.position.y;
        z_pos = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_MoveableAxes)
        {
            case e_MoveableAxes.x:
                transform.position = new Vector3(Mathf.Sin(Time.time * speed) * size, y_pos, z_pos);
                break;
            case e_MoveableAxes.y:
                transform.position = new Vector3(x_pos, Mathf.Sin(Time.time * speed) * size, z_pos);
                break;
            case e_MoveableAxes.z:
                transform.position = new Vector3(x_pos, y_pos, Mathf.Sin(Time.time * speed) * size);
                break;
            case e_MoveableAxes.xy:
                transform.position = new Vector3(Mathf.Sin(Time.time * speed) * size, Mathf.Sin(Time.time * speed) * size, z_pos);
                break;
            case e_MoveableAxes.xz:
                transform.position = new Vector3(Mathf.Sin(Time.time * speed) * size, y_pos, Mathf.Sin(Time.time * speed) * size);
                break;
            case e_MoveableAxes.yz:
                transform.position = new Vector3(x_pos, Mathf.Sin(Time.time * speed) * size, Mathf.Sin(Time.time * speed) * size);
                break;
            case e_MoveableAxes.xyz:
                transform.position = new Vector3(Mathf.Sin(Time.time * speed) * size, Mathf.Sin(Time.time * speed) * size, Mathf.Sin(Time.time * speed) * size);
                break;
        }
    }
}
