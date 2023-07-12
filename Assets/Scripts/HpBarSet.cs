using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarSet : MonoBehaviour
{
    public GameObject hpBarPrefab;
public Vector3 hpBarOffset = new Vector3(0, 2.2f, 0);

Canvas uiCanvas;
Image hpBarImage;
// Start is called before the first frame update
void Start()
{
    SetHpBar();
}

// Update is called once per frame
void Update()
{

}

void SetHpBar()
{
    uiCanvas = GameObject.Find("UI Canvas").GetComponent<Canvas>();
    GameObject hpBar = Instantiate<GameObject>(hpBarPrefab, uiCanvas.transform);
    hpBarImage = hpBar.GetComponentsInChildren<Image>()[1];
    print(hpBarImage);

    var _hpbar = hpBar.GetComponent<EnemyHpBar>();
    _hpbar.targetTr = transform;
    _hpbar.offset = hpBarOffset;
}
}
