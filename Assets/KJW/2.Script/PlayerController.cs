using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine; //시네머신을 using 해줘야 활용이 시네머신을 스크립트로 가져올 수 있다.
using static Cinemachine.CinemachineFreeLook;
using PaintIn3D;

public class PlayerController : MonoBehaviour
{
    private Animator anim;

    public Camera cam;

    public bool isGround = true;

    public float speed = 5f;
    public float mouseSensitivity = 100.0f;
    public float gravity = -9.81f;
    public float jumpHeight = 2.0f;

    private Vector3 velocity;

    private CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        // Player movement - WASD
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if (x != 0 || z != 0)
        {
            anim.SetBool("Move", true);
        }
        else
        {
            anim.SetBool("Move", false);
        }

        characterController.Move(move * speed * Time.deltaTime);


        // Jump

        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            anim.SetBool("Jump", true);
            StartCoroutine(WaitJump());
            isGround = false;
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftShift) && isGround)
        {
            anim.SetTrigger("Roll");
        }

        // Mouse look - rotate player and camera
        transform.rotation = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0);

    }


    IEnumerator WaitJump()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Jump", false);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
            isGround = true;
    }
}