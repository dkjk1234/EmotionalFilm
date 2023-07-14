using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine; //시네머신을 using 해줘야 활용이 시네머신을 스크립트로 가져올 수 있다.
using static Cinemachine.CinemachineFreeLook;
using PaintIn3D;

public class PlayerController : MonoBehaviour
{
    // 김정우 코드
    private Animator anim;

    public Image fillArea;

    public Text percentText;

    public Camera cam;

    public float paintValue = 100;

    public bool paintRecovery = true;

    // 윤수지 코드
    private Rigidbody rigid;
    public bool isGround = true;


    // 원래 있었던 코드
    public float speed = 5f;
    public float mouseSensitivity = 100.0f;
    public float gravity = -9.81f;
    public float jumpHeight = 2.0f;

    private Vector3 velocity;

    private CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        // 원래 있던 코드
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        // 김정우 코드
        anim = GetComponent<Animator>();

        // 윤수지 코드
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        PaintBar();

        paintValue = Mathf.Clamp(paintValue, 0, 100);

        if (paintRecovery)
        {
            paintValue += 0.02f;
        }
        int percentPaintValue = (int)paintValue;
        percentText.text = percentPaintValue.ToString() + "%";

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

    void PaintBar()
    {
        fillArea.rectTransform.sizeDelta = new Vector2(840, 70 + paintValue * 7);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
            isGround = true;
    }
}