using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
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
        //Portal에 닿으면 End 신으로 넘어감
        if(collision.collider.gameObject.CompareTag("Portal"))
        {
            SceneManager.LoadScene("End");
        }
    }
}
