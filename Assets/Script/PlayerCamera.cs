using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    public GameObject Target;               

    public float offsetX = 0.0f;            
    public float offsetY = 4.82f;           
    public float offsetZ = -5.99f;          

    public float angleX = 23.55f;
    public float angleY = 0.0f;
    public float angleZ = 0.0f;

    public float CameraSpeed = 10.0f;       
    Vector3 TargetPos;                      

    // Update is called once per frame
    void FixedUpdate()
    {
        TargetPos = new Vector3(
            Target.transform.position.x + offsetX,
            Target.transform.position.y + offsetY,
            Target.transform.position.z + offsetZ
            );

        transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * CameraSpeed);
        transform.rotation = Quaternion.Euler(angleX, angleY, angleZ);

    }

}
