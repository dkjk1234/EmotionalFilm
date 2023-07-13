using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTest : MonoBehaviour
{
    public CinemachineFreeLook thirdPersonCamera;
    public CinemachineVirtualCamera fixCamera;

    private bool isfixMode = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            isfixMode = !isfixMode;

            thirdPersonCamera.gameObject.SetActive(!isfixMode);
            fixCamera.gameObject.SetActive(!isfixMode);
        }
    }
}
