using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum Weapon
    {
        Spray,
        Brush,
        WaterBalloon
    }
    public Weapon weapon = Weapon.Spray;

    public List<GameObject> selectWeapon;

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
        Spray();
        Brush();
        WaterBalloon();

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
            // �޺� ����µ� �ɸ��� �ð�
            yield return new WaitForSeconds(0.7f);
            comboBool = true;
        }
    }

    void Spray()
    {
        // ���� �߿��� ���� �Ұ����ϰ� ���� �ڵ� �߰��� ��
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ConnectWeapon(0, "Spray");
            weapon = Weapon.Spray;
        }
    }

    void Brush()
    {
        // ���� �߿��� ���� �Ұ����ϰ� ���� �ڵ� �߰��� ��
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ConnectWeapon(1, "Brush");
            weapon = Weapon.Brush;
        }

        // ���� �޺����� ����
        int Combo = anim.GetInteger("SwingCombo");

        // comboBool�� ������ �޺��� ����ٰ� �Ǵ��ϰ� ���� �޺��� 0���� �ٲ�
        if (comboBool)
        {
            Combo = 0;
            anim.SetInteger("SwingCombo", Combo);
        }

        // �޺��� �ִ�ġ�� ��� 0���� �ٲ�
        if (Combo == 5)
        {
            Combo = 0;
            anim.SetInteger("SwingCombo", Combo);
        }

        // Swing
        if (Input.GetMouseButtonDown(0))
        {
            if (weapon == Weapon.Brush)
            {
                // ���ݽ� �޺� Ȯ�� �ڷ�ƾ ��ž
                StopCoroutine("WaitCombo");
                anim.SetInteger("SwingCombo", Combo + 1);
                comboBool = false;

                // ������ ������ �޺��� ������� Ȯ���ϴ� �ڷ�ƾ ����
                StartCoroutine("WaitCombo");
            }
        }
    }
    void WaterBalloon()
    {
        // ���� �߿��� ���� �Ұ����ϰ� ���� �ڵ� �߰��� ��
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ConnectWeapon(2, "WaterBalloon");
            weapon = Weapon.WaterBalloon;
        }
    }

    // ���� ���⸦ Ȱ��ȭ���ִ� �Լ�
    void ConnectWeapon(int weaponNum, string weaponName)
    {
        selectWeapon[0].SetActive(false);
        selectWeapon[1].SetActive(false);
        selectWeapon[2].SetActive(false);
        selectWeapon[weaponNum].SetActive(true);

        anim.SetBool("Spray", false);
        anim.SetBool("Brush", false);
        anim.SetBool("WaterBalloon", false);
        anim.SetBool(weaponName, true);
    }
}