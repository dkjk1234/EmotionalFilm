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
    public enum Weapon
    {
        Spray,
        Brush,
        PaintGun,
        WaterBalloon
    }
    public Weapon weapon = Weapon.Spray;

    private Animator anim;

    public Image fillArea;

    public Text percentText;

    public CinemachineFreeLook cineFreeLook;
    public CinemachineCameraOffset cineCameraOffset;
    public CinemachineFollowZoom cineFollowZoom;

    Vector3[] trajectoryPoints;
    public LineRenderer lineRenderer;

    public ParticleSystem PS;

    public Camera cam;
    
    public List<GameObject> prefab;
    public List<GameObject> selectWeapon;
    public List<GameObject> aim;
    public List<GameObject> effect;
    public List<P3dPaintSphere> SBScript; //Spray, Brush
    public List<P3dPaintDecal> PWScript; //PaintGun, WaterBalloon


    float paintValue = 100;

    float sprayConsumption = 0.03f;
    float ARpaintGunConsumption = 1f;
    float SRpaintGunConsumption = 3f;
    float waterBalloonConsumption = 5f;

    bool paintRecovery = true;
    bool sprayPaintMin = true;

    bool FPS = false;
    bool swingBool = true;
    bool comboBool = false;
    bool bulletCool = false;
    bool waterBalloonCool = false;

    int combo;

    public float srSpeed = 3f;
    public float originSpeed = 5f;
    public float runSpeed = 7f;
    Vector3 ScreenCenter;

    // 윤수지 코드
    private Rigidbody rigid;
    public bool isGround = true;


    // 원래 있었던 코드
    float speed = 5f;
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
        ScreenCenter = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
        anim = GetComponent<Animator>();

        trajectoryPoints = new Vector3[30];
        lineRenderer.positionCount = 30;

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

    void FixedUpdate()
    {
        if (weapon == Weapon.WaterBalloon && Input.GetMouseButton(0) && paintValue >= waterBalloonConsumption)
        {
            CalculateTrajectory();
        }
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
    IEnumerator BulletCooldown(float waitSec)
    {
        bulletCool = true;
        yield return new WaitForSeconds(waitSec);
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

    IEnumerator recoil()
    {
        for (int i = 0; i < 10; i++)
        {
            cineFreeLook.m_YAxis.Value -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        for (int i = 0;i < 10; i++)
        {
            cineFreeLook.m_YAxis.Value += 0.002f;
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator WaitJump()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Jump", false);
    }

    IEnumerator PaintRecovery()
    {
        paintRecovery = false;
        yield return new WaitForSeconds(4f);
        paintRecovery = true;
    }

    void Spray()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ConnectWeapon(0, "Spray");
            weapon = Weapon.Spray;
        }

        if (weapon != Weapon.Spray)
            return;

        prefab[2].transform.rotation = Quaternion.Euler(cam.transform.eulerAngles.x, cam.transform.eulerAngles.y, cam.transform.eulerAngles.z);


        if (Input.GetMouseButton(0))
        {
            if (paintValue >= sprayConsumption && sprayPaintMin)
            {
                StopCoroutine("PaintRecovery");
                prefab[2].SetActive(true);
                paintValue -= sprayConsumption;
                StartCoroutine("PaintRecovery");
            }

            else if (paintValue < sprayConsumption)
            {
                prefab[2].SetActive(false);
                sprayPaintMin = false;
            }

        }

        if (paintValue > 10f)
        {
            sprayPaintMin = true;
        }
    }

    void Brush()
    {
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


        if (Input.GetMouseButton(0))
        {
            if ((!bulletCool && FPS && paintValue >= SRpaintGunConsumption) || (!bulletCool && !FPS && paintValue >= ARpaintGunConsumption))
            {
                StopCoroutine("PaintRecovery");
                var ray = Camera.main.ScreenPointToRay(ScreenCenter);
                var rotation = Quaternion.LookRotation(ray.direction);

                var clone = Instantiate(prefab[0], prefab[0].transform.position, rotation);

                clone.SetActive(true);

                var cloneRigidbody = clone.GetComponent<Rigidbody>();

                if (cloneRigidbody != null)
                {
                    cloneRigidbody.velocity = clone.transform.forward * 100;
                }
                StartCoroutine("PaintRecovery");
                if (FPS)
                {
                    StartCoroutine(recoil());
                    StartCoroutine(BulletCooldown(1.5f));
                    paintValue -= SRpaintGunConsumption;
                }

                if (!FPS)
                {
                    StartCoroutine(BulletCooldown(0.2f));
                    paintValue -= ARpaintGunConsumption;
                }
            }
        }


        if (Input.GetMouseButtonDown(1))
        {
            if (paintValue > SRpaintGunConsumption)
            {
                FPS = true;
            }
        }


        if (Input.GetMouseButtonUp(1))
        {
            FPS = false;
        }

        // paintValue값이 3보다 적으면, 조준이 불가능
        if (paintValue < SRpaintGunConsumption)
        {
            FPS = false;
        }

        if (FPS)
        {
            StopCoroutine("PaintRecovery");
            // 스피드 변경 코드
            speed = srSpeed;
            cineFreeLook.m_Orbits = new Orbit[3]
            {
                new Orbit(1f, 2f),
                new Orbit(0.5f, 2f),
                new Orbit(0f, 2f)
            };
            cineFollowZoom.enabled = true;
            cineCameraOffset.enabled = true;
            cineFreeLook.m_XAxis.m_MaxSpeed = 50f;

            PWScript[0].Radius = 2;
            var main = PS.main;
            main.startSize = 3;

            aim[0].transform.localScale = new Vector3(5, 5, 5);
            StartCoroutine("PaintRecovery");
        }

        if (!FPS || !isGround)
        {
            // 스피드 변경 코드
            speed = originSpeed;
            cineFreeLook.m_Orbits = new Orbit[3]
            {
                new Orbit(4.5f, 3f),
                new Orbit(2f, 5f),
                new Orbit(-1.5f, 3f)
            };
            cineFollowZoom.enabled = false;
            cineCameraOffset.enabled = false;
            cineFreeLook.m_XAxis.m_MaxSpeed = 300f;

            PWScript[0].Radius = 1;
            var main = PS.main;
            main.startSize = 1;

            aim[0].transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
    }

    void WaterBalloon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ConnectWeapon(3, "WaterBalloon");
            weapon = Weapon.WaterBalloon;
        }

        if (weapon != Weapon.WaterBalloon)
            return;

        prefab[1].transform.rotation = Quaternion.Euler(cam.transform.eulerAngles.x, cam.transform.eulerAngles.y, cam.transform.eulerAngles.z);

        if (Input.GetMouseButton(0) && paintValue >= waterBalloonConsumption)
        {
            if (!waterBalloonCool)
            {
                StopCoroutine("PaintRecovery");
                lineRenderer.enabled = true;
                //CalculateTrajectory();
                StartCoroutine("PaintRecovery");
            }
        }
        
        if (Input.GetMouseButtonUp(0) && paintValue >= waterBalloonConsumption)
        {
            if (!waterBalloonCool)
            {
                anim.SetBool("IsWaterBalloon", true);

                var ray = Camera.main.ScreenPointToRay(ScreenCenter);
                var rotation = Quaternion.LookRotation(ray.direction);

                var clone = Instantiate(prefab[1], prefab[1].transform.position, rotation);
                
                paintValue -= waterBalloonConsumption;
                
                clone.SetActive(true);

                var cloneRigidbody = clone.GetComponent<Rigidbody>();

                if (cloneRigidbody != null)
                {
                    cloneRigidbody.velocity = clone.transform.forward * 15 + clone.transform.up * 5;
                }

                for (int i = 0; i < 30; i++)
                {
                    //fixedUpdate로 할 경우 예전 위치값이 보이는 버그가 존재. 따라서 계속 위치값을 제거해줘야 함.
                    lineRenderer.SetPosition(i, Vector3.zero);
                }
                lineRenderer.enabled = false;

                StartCoroutine(WaitWaterBalloon());
            }
        }
    }

    void PaintBar()
    {
        fillArea.rectTransform.sizeDelta = new Vector2(840, 70 + paintValue * 7);
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
        speed = originSpeed;
        cineFreeLook.m_Orbits = new Orbit[3]
        {
                new Orbit(4.5f, 3f),
                new Orbit(2f, 5f),
                new Orbit(-1.5f, 3f)
        };
        cineFollowZoom.enabled = false;
        cineCameraOffset.enabled = false;
        cineFreeLook.m_XAxis.m_MaxSpeed = 300f;

        for (int i = 0; i < 30; i++)
        {
            //fixedUpdate로 할 경우 예전 위치값이 보이는 버그가 존재. 따라서 계속 위치값을 제거해줘야 함.
            lineRenderer.SetPosition(i, Vector3.zero);
        }
        lineRenderer.enabled = false;
    }

    // 물풍선 궤적 구하는 코드(등가속도 공식을 이용하여 거리 계산 s = v0*t + (1/2)at^2) 여기서 가속도는 중력가속도뿐임.
    private void CalculateTrajectory()
    {
        Vector3 startPosition = prefab[1].transform.position;
        Vector3 currentVelocity = prefab[1].transform.forward * 15f + prefab[1].transform.up * 5;

        for (int i = 0; i < 30; i++)
        {
            float time = i * 0.1f;
            float x = startPosition.x + (currentVelocity.x * time);
            float y = startPosition.y + (currentVelocity.y * time) + (0.5f * gravity * time * time);
            float z = startPosition.z + (currentVelocity.z * time);
            trajectoryPoints[i] = new Vector3(x, y, z);
            lineRenderer.SetPosition(i, trajectoryPoints[i]);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
            isGround = true;
    }
}