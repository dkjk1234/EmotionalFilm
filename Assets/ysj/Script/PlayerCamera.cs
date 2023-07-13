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
    public float angleY = 180.0f;
    public float angleZ = 0.0f;
    private float xRotate, yRotate, xRotateMove, yRotateMove;
    public float rotateSpeed = 500.0f;

    public float CameraSpeed = 10.0f;
    Vector3 TargetPos;

    void Start()
    {
        transform.eulerAngles = new Vector3(angleX, angleY, angleZ);
    }

    void Update()
    {
        xRotateMove = -Input.GetAxis("Mouse Y") * Time.deltaTime * rotateSpeed;
        yRotateMove = Input.GetAxis("Mouse X") * Time.deltaTime * rotateSpeed;

        yRotate = Mathf.Clamp(yRotate + yRotateMove, angleY - 90f, angleY + 90f);
        xRotate = xRotate + xRotateMove;
        xRotate = Mathf.Clamp(xRotate, -90f, 90f);

        transform.eulerAngles = new Vector3(xRotate, yRotate, 0);


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TargetPos = new Vector3(
            Target.transform.position.x + offsetX,
            Target.transform.position.y + offsetY,
            Target.transform.position.z + offsetZ
            );

        transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * CameraSpeed);
    }

}
