using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float Speed = 10.0f;
    public float jumpHeight = 2.0f;

    float h, v;
    // Start is called before the first frame update
    private Vector3 velocity;
    private Rigidbody rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        transform.position += new Vector3(h, 0, v) * Speed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigid.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);

        }

    }
}
