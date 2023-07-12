using System;
using System.Collections;
using System.Collections.Generic;
using PaintIn3D;
using UnityEngine;
using UnityEngine.UI;

public class SpreadPaintingWorld : MonoBehaviour
{
    public Transform targetOffSet;
    private Texture temp;
    public Texture black;
    public float startRadious = 0f;
    public float maxRadious = 200f;
    public float spreadSpeed = 5f;
    public P3dPaintSphere paintSphere;
    
    public float paintRadious;

    public int numState = 0;
    // Start is called before the first frame update
    void Start()
    {
        if(targetOffSet)
        transform.position = targetOffSet.position;

        paintSphere = GetComponent<P3dPaintSphere>();
        paintRadious = paintSphere.Radius;
    }

    public void OnEnable()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        paintSphere.Radius += spreadSpeed * Time.deltaTime;
        if (paintSphere.Radius >  maxRadious)
        {
            paintSphere.Radius = 0f;
            if (numState == 0)
            {
                paintSphere.Color = Color.white;
                
                
            }
            if (numState == 1)
            {
                paintSphere.Color = Color.red;
                numState = -1;
            }
            numState++;
        }
    }
}
