using System.Collections;
using System.Collections.Generic;
using PaintIn3D;
using UnityEngine;

public class GetPixxelUvColor : MonoBehaviour
{
    private P3dPaintableTexture p3dTexture;
    // Start is called before the first frame update
    void Start()
    {
        p3dTexture = GetComponent<P3dPaintableTexture>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Renderer rend = hit.transform.GetComponent<Renderer>();
            Texture2D texture = rend.materials[0].mainTexture as Texture2D; // 첫 번째 머테리얼

            Vector2 pixelUV = hit.textureCoord;
            pixelUV.x *= texture.width;
            pixelUV.y *= texture.height;

            Color color = texture.GetPixel((int)pixelUV.x, (int)pixelUV.y);

            // 원하는 효과를 실행하세요.
        }
    }
    
}
