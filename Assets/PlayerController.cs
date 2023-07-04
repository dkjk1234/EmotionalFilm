using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum Weapon
    {
        Melee,
        Spray,
        Grenade
    }
    Weapon weapon = Weapon.Melee;

    bool comboBool = false;

    public float speed = 5.0f;
    public float mouseSensitivity = 100.0f;
    public float gravity = -9.81f;
    public float jumpHeight = 2.0f;

    private float xRotation = 0f;
    private Vector3 velocity;

    private CharacterController characterController;
    private Transform cameraTransform;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        int Combo = anim.GetInteger("SwingCombo");
        if (comboBool)
        {
            print("SDSDS");
            Combo = 0;
            anim.SetInteger("SwingCombo", Combo);
        }
        if (Combo == 5)
        {
            Combo = 0;
            anim.SetInteger("SwingCombo", Combo);
        }

        // Swing
        if (Input.GetMouseButtonDown(0))
        {
            if (weapon == Weapon.Melee)
            {
                StopCoroutine("WaitCombo");

                anim.SetBool("IsSwing", true);
                anim.SetInteger("SwingCombo", Combo + 1);
                comboBool = false;

                StartCoroutine("WaitCombo");
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (weapon == Weapon.Melee)
            {
                anim.SetBool("IsSwing", false);

            }
        }

        // Player movement - WASD
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        characterController.Move(move * speed * Time.deltaTime);

        // Jump
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        // Mouse look - rotate player and camera
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        //float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(/*xRotation*/15f, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }


    IEnumerator WaitCombo()
    {
        {
            yield return new WaitForSeconds(1f);
            comboBool = true;

        }
    }
}