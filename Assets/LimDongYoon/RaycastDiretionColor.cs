using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaintIn3D;
using Unity.VisualScripting;

public class RaycastDiretionColor : MonoBehaviour
{
    

    public float raycastDistance = 100f;
    public LayerMask raycastLayerMask;
    private P3dReadColor readColor;

    private void Start()
    {
        readColor = GetComponent<P3dReadColor>();
    }

    void Update()
    {
        
    }
    public float PlayerDownRaycastColor(Transform player)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, raycastDistance, raycastLayerMask))
            // ㅇ.ㅇ .....................................................ㅇ ㅓㄷ ㅓㄱㅎ ㅔㅔㅔㅔㅔ 
        {
            // Call your function with the hit details.
            P3dHit p3dHit = new P3dHit(hit);

            readColor.HandleHitCoord(false, 0, 0, 0, p3dHit, Quaternion.identity);
            Color color = readColor.Color;

//            Debug.Log(color + " alpha: " + color.a);
            return color.a;
        }
        return 0f;
    }
    public void HandleHitCoord(bool preview, int priority, float pressure, int seed, P3dHit hit, Quaternion rotation)
    {
        // Your function body here...
    }
    

}
