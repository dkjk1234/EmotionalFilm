using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Rigidbody playerRD;
    public Camera Cam;

    float MoveSpeed;
    float rotSpeed;
    float currentRot;
    // Start is called before the first frame update
    void Start()
    {
        MoveSpeed = 5.0f;
        rotSpeed = 5.0f;
        currentRot = 0f;

    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        RotCtrl();
    }

    void PlayerMove()
    {
        float xlnput = Input.GetAxis("Horizontal");
        float zlnput = Input.GetAxis("Vertical");

        float xSpeed = xlnput * MoveSpeed;
        float zSpeed = zlnput * MoveSpeed;

        transform.Translate(Vector3.forward * zSpeed * Time.deltaTime);
        transform.Translate(Vector3.right * xSpeed * Time.deltaTime);

    }

    void RotCtrl()
    {
        float rotX = Input.GetAxis("Mouse Y") * rotSpeed;
        float rotY = Input.GetAxis("Mouse X") * rotSpeed;
    
        currentRot -= rotX;
    
        currentRot = Mathf.Clamp(currentRot, -80f, 80f);
    
        this.transform.localRotation *= Quaternion.Euler(0, rotY, 0);
        Cam.transform.localEulerAngles = new Vector3(currentRot, 0f, 0f);
    
    }
}
