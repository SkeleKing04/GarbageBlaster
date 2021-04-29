using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinMovement : MonoBehaviour
{
    public int m_Mode;
    public bool m_LockSpeedSize = false;
    private float x_pos;
    public float x_size;
    public float x_speed;
    private float y_pos;
    public float y_size;
    public float y_speed;
    private float z_pos;
    public float z_size;
    public float z_speed;
    // Start is called before the first frame update
    void Start()
    {
        switch (m_LockSpeedSize)
        {
            case true:
                y_speed = x_speed;
                z_speed = x_speed;
                y_size = x_size;
                z_size = x_size;
                break;
            default:
                break;
        }
        x_pos = transform.position.x;
        y_pos = transform.position.y;
        z_pos = transform.position.z;
        Debug.Log(x_pos);
        Debug.Log(y_pos);
        Debug.Log(z_pos);
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_Mode)
        {
            case 0: //x
                transform.position = new Vector3(x_pos + Mathf.Sin(Time.time * x_speed) * x_size, y_pos, z_pos);
                break;
            case 1: //y
                transform.position = new Vector3(x_pos, y_pos + Mathf.Sin(Time.time * y_speed) * y_size, z_pos);
                break;
            case 2: //z
                transform.position = new Vector3(x_pos, y_pos, z_pos + Mathf.Sin(Time.time * z_speed) * z_size);
                break;
            case 3: //xy
                transform.position = new Vector3(x_pos + Mathf.Sin(Time.time * x_speed) * x_size, y_pos + Mathf.Sin(Time.time * y_speed) * y_size, z_pos);
                break;
            case 4: //xz
                transform.position = new Vector3(x_pos + Mathf.Sin(Time.time * x_speed) * x_size, y_pos, z_pos + Mathf.Sin(Time.time * z_speed) * z_size);
                break;
            case 5: //yz
                transform.position = new Vector3(x_pos, y_pos + Mathf.Sin(Time.time * y_speed) * y_size, z_pos + Mathf.Sin(Time.time * z_speed) * z_size);
                break;
            case 6: //xyz
                transform.position = new Vector3(x_pos + Mathf.Sin(Time.time * x_speed) * x_size, y_pos + Mathf.Sin(Time.time * y_speed) * y_size, z_pos + Mathf.Sin(Time.time * z_speed) * z_size);
                break;
            default:
                transform.position = new Vector3(x_pos, y_pos, z_pos);
                break;
        }
    }
}
