using PaintIn3D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BossHp : MonoBehaviour
{
    public P3dChangeCounter p3DChangeCounter;
    // Start is called before the first frame update
    void Start()
    {
        p3DChangeCounter.GetComponent<P3dChangeCounter>();
    }

    // Update is called once per frame
    void Update()
    {
        GameManager.Instance.uIScript.bossHealthSlider.value = 100096 - p3DChangeCounter.Count;
    }
}
