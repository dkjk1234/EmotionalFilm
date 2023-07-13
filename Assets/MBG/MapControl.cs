using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapControl : MonoBehaviour
{
    public GameObject MonsterPos0;
    public GameObject MonsterPos1;
    public GameObject MonsterPos2;
    public GameObject MonsterPos3;
    public GameObject MonsterPos4;
    
    public GameObject MonsterPos5;

    public GameObject Door;

    public GameObject Target;

    bool MonsterTag = true;

    bool BossTag = true;
    
    // Start is called before the first frame update
    void Start()
    {
        Target.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        MonsterTag = GameObject.FindGameObjectWithTag("Monster");
        
        if(MonsterTag == false)
        {
            GameObject.Destroy(Door);
        }

        BossTag = GameObject.FindGameObjectWithTag("Boss");
        
        if(BossTag == false)
        {
            Target.SetActive(true);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.name=="MonsterPos0")
        {
            GameObject.Destroy(MonsterPos0);
        }
        else if(collision.collider.gameObject.name=="MonsterPos1")
        {
            GameObject.Destroy(MonsterPos1);
        }
        else if(collision.collider.gameObject.name=="MonsterPos2")
        {
            GameObject.Destroy(MonsterPos2);
        }
        else if(collision.collider.gameObject.name=="MonsterPos3")
        {
            GameObject.Destroy(MonsterPos3);
        }
        else if(collision.collider.gameObject.name=="MonsterPos4")
        {
            GameObject.Destroy(MonsterPos4);
        }
        else if(collision.collider.gameObject.name=="MonsterPos5")
        {
            GameObject.Destroy(MonsterPos5);
        }

    }
}
