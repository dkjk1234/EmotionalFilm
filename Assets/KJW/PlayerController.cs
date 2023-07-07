using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // 김정우 코드
    public enum Weapon
    {
        Spray,
        Brush,
        PaintGun,
        WaterBalloon
    }
    public Weapon weapon = Weapon.Spray;

    private Animator anim;

    public List<GameObject> prefab;
    public List<GameObject> selectWeapon;
    public List<GameObject> aim;
    public List<GameObject> effect;

    public Slider paintBar;
    public GameObject fillArea;

    public Camera cam;

    bool FPS = false;
    bool swingBool = true;
    bool comboBool = false;
    bool bulletCool = false;
    bool waterBalloonCool = false;

    int combo;

    public float run = 2f;
    Vector3 ScreenCenter;

    // 윤수지 코드
    private Rigidbody rigid;
    public bool isGround = true;


    // 원래 있었던 코드
    public float speed = 5.0f;
    public float mouseSensitivity = 100.0f;
    public float gravity = -9.81f;
    public float jumpHeight = 2.0f;

    private float xRotation = 0f;
    private Vector3 velocity;

    private CharacterController characterController;
    private Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        // 원래 있던 코드
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;

        // 김정우 코드
        ScreenCenter = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
        anim = GetComponent<Animator>();

        // 윤수지 코드
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Spray();
        Brush();
        PaintGun();
        WaterBalloon();
        PaintBar();

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
            anim.SetTrigger("Jump");
            isGround = false;
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            anim.SetTrigger("Roll");
        }

        // Mouse look - rotate player and camera
        transform.rotation = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0);
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

        // comboBool이 켜지면 콤보가 끊겼다고 판단하고 스윙 콤보를 0으로 바꿈
        if (comboBool)
        {
            combo = 0;
            anim.SetInteger("SwingCombo", combo);
        }

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

        if (weapon != Weapon.Spray)
            return;

        prefab[2].transform.localRotation = Quaternion.Euler(cam.transform.eulerAngles.x, 0f, 0f);

        if (Input.GetMouseButton(0))
        {
            if (paintBar.value > 0)
            {
                prefab[2].SetActive(true);
                paintBar.value -= 0.001f;
            }

            else
            {
                prefab[2].SetActive(false);
            }
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

        if (weapon != Weapon.Brush)
            return;

        // 스윙 콤보수를 받음
        combo = anim.GetInteger("SwingCombo");

        // Swing
        if (Input.GetMouseButtonDown(0))
        {
            if (swingBool)
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

        if (weapon != Weapon.PaintGun)
        {
            aim[0].SetActive(false);
            return;
        }
        aim[0].SetActive(true);

        if (Input.GetMouseButtonDown(0))
        {
            if (!bulletCool)
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

        if (Input.GetMouseButtonDown(1))
        {
            aim[1].SetActive(true);
            FPS = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            FPS = false;
            aim[1].SetActive(true);
            //cameraTransform.localPosition = new Vector3(0,2,-10);
        }

        if (FPS)
        {
            cameraTransform.localPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.8f, gameObject.transform.position.z + 0.4f);
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

        if (weapon != Weapon.WaterBalloon)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (!waterBalloonCool)
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

    void PaintBar()
    {
        if (paintBar.value <= 0)
            fillArea.SetActive(false);
        else
            fillArea.SetActive(true);
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

        FPS = false;
        aim[1].SetActive(true);
        //cameraTransform.localPosition = new Vector3(0, 2, -10);
    }

    void OnCollisionEnter(Collision collsion)
    {
        if (collsion.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
    }
}