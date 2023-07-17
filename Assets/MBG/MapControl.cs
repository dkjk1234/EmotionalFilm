using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapControl : MonoBehaviour
{
    //임시로 MonsterPos물체 받아서 닿았을 시 사라진 후 MonsterTag 수 줄어듬.
    //MonsterTag 수가 0이 되면 문이 열림
    public GameObject MonsterPos0;
    public GameObject MonsterPos1;
    public GameObject MonsterPos2;
    public GameObject MonsterPos3;
    public GameObject MonsterPos4;
    public GameObject MonsterPos5;
    public GameObject Door;
    public GameObject portal;
    public GameObject GameEndingSphere;
    int MonsterTag = 6;
    public GameObject BossTag;
    // Start is called before the first frame update
    void Start()
    {
        portal.SetActive(false);
        BossTag.SetActive(true);
        GameEndingSphere.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(MonsterTag == 0)
        {
            GameObject.Destroy(Door);
        }
        BossTag = GameObject.Find("Boss");
        print(BossTag);
        if(BossTag.activeSelf == false){
            portal.SetActive(true);
            GameEndingSphere.SetActive(true);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.name=="MonsterPos0")
        {
            GameObject.Destroy(MonsterPos0);
            MonsterTag--;
        }
        else if(collision.collider.gameObject.name=="MonsterPos1")
        {
            GameObject.Destroy(MonsterPos1);
            MonsterTag--;
        }
        else if(collision.collider.gameObject.name=="MonsterPos2")
        {
            GameObject.Destroy(MonsterPos2);
            MonsterTag--;
        }
        else if(collision.collider.gameObject.name=="MonsterPos3")
        {
            GameObject.Destroy(MonsterPos3);
            MonsterTag--;
        }
        else if(collision.collider.gameObject.name=="MonsterPos4")
        {
            GameObject.Destroy(MonsterPos4);
            MonsterTag--;
        }
        else if(collision.collider.gameObject.name=="MonsterPos5")
        {
            GameObject.Destroy(MonsterPos5);
            MonsterTag--;
        }
    }
}
