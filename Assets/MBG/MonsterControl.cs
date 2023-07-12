using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterControl : MonoBehaviour
{
    public GameObject MonsterPos0;
    public GameObject MonsterPos1;
    public GameObject MonsterPos2;
    public GameObject MonsterPos3;
    public GameObject MonsterPos4;
    
    public GameObject MonsterPos5;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
