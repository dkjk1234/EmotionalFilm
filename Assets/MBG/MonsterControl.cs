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
        if(collision.collider.gameObject.CompareTag("Monster"))
        {
            GameObject.Destroy(MonsterPos0);
        }
    }
}
