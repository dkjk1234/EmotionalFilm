using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove2 : MonoBehaviour
{
    Animator animator;

    public float Speed = 10.0f;
    public float jumpHeight = 2.0f;

    float h, v;
    // Start is called before the first frame update
    private Vector3 velocity;
    private Rigidbody rigid;
    public bool isGround = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(h, 0, v);
        transform.position += new Vector3(h, 0, v) * Speed * Time.deltaTime;

        if (h != 0 || v != 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10.0f);
            animator.SetBool("move", true);
        }
        else
        {
            animator.SetBool("move", false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround==true)
        {
            rigid.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            animator.SetTrigger("Jump");
            isGround = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetTrigger("Roll");
        }

        if (Input.GetMouseButton(0))
        {
            animator.SetTrigger("attack");
        }
    }

      

    void OnCollisionEnter(Collision collsion)
    {
        if(collsion.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }

    }
}
