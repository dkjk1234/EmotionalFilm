using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClearMap : MonoBehaviour
{
    public GameObject spreadSphere;
    public GameObject boss;
    public int totalMonster;
    public int catchedMonster;
    public GameObject bossHealthBar;
    public GameObject clearItemPicture;
    public GameObject portal;

    public bool isClear = false;
    public int numState = 0;
    public bool isTest = false;

    void Start()
    {
        if (isTest)
        {
            spreadSphere.SetActive(false);
            portal.SetActive(false);
            boss.SetActive(false);
            bossHealthBar.SetActive(false);
            clearItemPicture.SetActive(false);
            
        }
    }
    public void _ClearMap()
    {
        isClear = true;
        spreadSphere.SetActive(true);
        portal.SetActive(true);
        clearItemPicture.SetActive(true);
        clearItemPicture.transform.position = boss.transform.position;
        Cursor.lockState = CursorLockMode.None;
        Debug.Log("Clear!");
    }

    public void CompleteMonster()
    {
        boss.SetActive(true);
        bossHealthBar.SetActive(true);
        var monsters = GameObject.FindGameObjectsWithTag("Monster");
        if (monsters.Length > 0)
        {
            foreach (var m in monsters)
            {
                m.SetActive(false);
            }
        }
        

    }
    
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        if(numState == 0)
        if (catchedMonster >= totalMonster)
        {
            numState = 1;
            CompleteMonster();
            
        }
    }
}
