using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum Weapon
    {
        Spray,
        Brush,
        PaintGun,
        WaterBalloon
    }
    public Weapon weapon = Weapon.Spray;

    public List<GameObject> prefab;
    public List<GameObject> selectWeapon;
    public GameObject aim;
    public List<GameObject> effect;

    bool swingBool = true;
    bool comboBool = false;
    bool bulletCool = false;
    bool waterBalloonCool = false;

    int combo;

    public float run = 2f;
    Vector3 ScreenCenter;


    public float speed = 5.0f;
    public float mouseSensitivity = 100.0f;
    public float gravity = -9.81f;
    public float jumpHeight = 2.0f;

    public float xRotation = 0f;
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

        ScreenCenter = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Spray();
        Brush();
        PaintGun();
        WaterBalloon();

        // Player movement - WASD
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if (Input.GetKey(KeyCode.LeftShift))
            characterController.Move(move * 2 * speed * Time.deltaTime);

        else
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
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }


    IEnumerator WaitCombo()
    {
        // 콤보 끊기는데 걸리는 시간

        // 콤보가 최대치일 경우 다시 처음 콤보로 돌아감
        if (combo == 5)
        {
            combo = 0;
            anim.SetInteger("SwingCombo", combo);
            swingBool = false;
            yield return new WaitForSeconds(1f);
            swingBool = true;
        }

        else
            yield return new WaitForSeconds(0.8f);
        comboBool = true;
        // 콤보 중이 아닐 경우 검 잔상 이펙트를 끔
        effect[0].SetActive(false);
    }
    IEnumerator BulletCooldown()
    {
        bulletCool = true;
        yield return new WaitForSeconds(0.3f);
        bulletCool = false;
    }

    IEnumerator WaitWaterBalloon()
    {
        waterBalloonCool = true;
        yield return new WaitForSeconds(0.1f);
        // 이거 안 해주면 계속 수류탄 던지는 모션 반복함.
        anim.SetBool("IsWaterBalloon", false);
        yield return new WaitForSeconds(0.4f);
        waterBalloonCool = false;
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
        combo = anim.GetInteger("SwingCombo");

        // comboBool이 켜지면 콤보가 끊겼다고 판단하고 스윙 콤보를 0으로 바꿈
        if (comboBool)
        {
            combo = 0;
            anim.SetInteger("SwingCombo", combo);
        }

        // Swing
        if (Input.GetMouseButtonDown(0))
        {
            if (weapon == Weapon.Brush && swingBool)
            {
                effect[0].SetActive(true);
                // 공격시 콤보 확인 코루틴 스탑
                StopCoroutine("WaitCombo");
                anim.SetInteger("SwingCombo", combo + 1);
                comboBool = false;

                // 공격이 끝나면 콤보가 끊겼는지 확인하는 코루틴 진행
                StartCoroutine("WaitCombo");
            }
        }
    }

    void PaintGun()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ConnectWeapon(2, "PaintGun");
            weapon = Weapon.PaintGun;
        }
        selectWeapon[0].transform.localRotation = Quaternion.Euler(xRotation, 90f, 0f);

        if(weapon == Weapon.PaintGun)
            aim.SetActive(true);
        else
            aim.SetActive(false);

        if (Input.GetMouseButtonDown(0))
        {
            if (weapon == Weapon.PaintGun && !bulletCool)
            {
                var ray = Camera.main.ScreenPointToRay(ScreenCenter);
                var rotation = Quaternion.LookRotation(ray.direction);

                var clone = Instantiate(prefab[0], prefab[0].transform.position, rotation);

                clone.SetActive(true);

                // Throw with velocity?
                var cloneRigidbody = clone.GetComponent<Rigidbody>();

                if (cloneRigidbody != null)
                {
                    cloneRigidbody.velocity = clone.transform.forward * 100;
                }

                StartCoroutine(BulletCooldown());
            }
        }
    }

    void WaterBalloon()
    {
        // 동작 중에는 변경 불가능하게 막는 코드 추가할 것
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ConnectWeapon(3, "WaterBalloon");
            weapon = Weapon.WaterBalloon;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (weapon == Weapon.WaterBalloon && !waterBalloonCool)
            {
                anim.SetBool("IsWaterBalloon", true);

                var ray = Camera.main.ScreenPointToRay(ScreenCenter);
                var rotation = Quaternion.LookRotation(ray.direction);

                var clone = Instantiate(prefab[1], prefab[1].transform.position, rotation);

                clone.SetActive(true);

                // Throw with velocity?
                var cloneRigidbody = clone.GetComponent<Rigidbody>();

                if (cloneRigidbody != null)
                {
                    cloneRigidbody.velocity = clone.transform.forward * 15 + clone.transform.up * 5;
                }

                StartCoroutine(WaitWaterBalloon());
            }
        }
    }

    // 현재 무기를 활성화해주는 함수
    void ConnectWeapon(int weaponNum, string weaponName)
    {
        selectWeapon[0].SetActive(false);
        selectWeapon[1].SetActive(false);
        selectWeapon[2].SetActive(false);
        selectWeapon[3].SetActive(false);
        selectWeapon[weaponNum].SetActive(true);

        anim.SetBool("Spray", false);
        anim.SetBool("Brush", false);
        anim.SetBool("PaintGun", false);
        anim.SetBool("WaterBalloon", false);
        anim.SetBool(weaponName, true);
    }
}