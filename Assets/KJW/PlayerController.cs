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
            // 콤보 끊기는데 걸리는 시간
            yield return new WaitForSeconds(0.7f);
            comboBool = true;
        }
    }

    void Spray()
    {
        // 동작 중에는 변경 불가능하게 막는 코드 추가할 것
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ConnectWeapon(0, "Spray");
            weapon = Weapon.Spray;
        }
    }

    void Brush()
    {
        // 동작 중에는 변경 불가능하게 막는 코드 추가할 것
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ConnectWeapon(1, "Brush");
            weapon = Weapon.Brush;
        }

        // 스윙 콤보수를 받음
        int Combo = anim.GetInteger("SwingCombo");

        // comboBool이 켜지면 콤보가 끊겼다고 판단하고 스윙 콤보를 0으로 바꿈
        if (comboBool)
        {
            Combo = 0;
            anim.SetInteger("SwingCombo", Combo);
        }

        // 콤보가 최대치일 경우 0으로 바꿈
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
                // 공격시 콤보 확인 코루틴 스탑
                StopCoroutine("WaitCombo");
                anim.SetInteger("SwingCombo", Combo + 1);
                comboBool = false;

                // 공격이 끝나면 콤보가 끊겼는지 확인하는 코루틴 진행
                StartCoroutine("WaitCombo");
            }
        }
    }
    void WaterBalloon()
    {
        // 동작 중에는 변경 불가능하게 막는 코드 추가할 것
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ConnectWeapon(2, "WaterBalloon");
            weapon = Weapon.WaterBalloon;
        }
    }

    // 현재 무기를 활성화해주는 함수
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