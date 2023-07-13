using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; //�ó׸ӽ��� using ����� Ȱ���� �ó׸ӽ��� ��ũ��Ʈ�� ������ �� �ִ�.
using static Cinemachine.CinemachineFreeLook;
using PaintIn3D;

public class WeaponScript : MonoBehaviour
{
    public enum Weapon
    {
        Spray,
        Brush,
        PaintGun,
        WaterBalloon
    }
    public Weapon weapon = Weapon.Spray;

    private Animator anim;

    public CinemachineFreeLook cineFreeLook;
    public CinemachineCameraOffset cineCameraOffset;
    public CinemachineFollowZoom cineFollowZoom;

    Vector3[] trajectoryPoints;
    public LineRenderer lineRenderer;

    public ParticleSystem PS;

    public List<GameObject> prefab;
    public List<GameObject> selectWeapon;
    public List<GameObject> aim;
    public List<GameObject> effect;
    public List<P3dPaintSphere> SBScript; //Spray, Brush
    public List<P3dPaintDecal> PWScript; //PaintGun, WaterBalloon


    float sprayConsumption = 0.03f;
    float ARpaintGunConsumption = 1f;
    float SRpaintGunConsumption = 3f;
    float waterBalloonConsumption = 5f;

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

    // Start is called before the first frame update
    void Start()
    {
        ScreenCenter = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
        anim = GetComponent<Animator>();

        trajectoryPoints = new Vector3[30];
        lineRenderer.positionCount = 30;
    }

    // Update is called once per frame
    void Update()
    {
        Spray();
        Brush();
        PaintGun();
        WaterBalloon();
    }

    void FixedUpdate()
    {
        if (weapon == Weapon.WaterBalloon && Input.GetMouseButton(0) && GameManager.Instance.player.paintValue >= waterBalloonConsumption)
        {
            CalculateTrajectory();
        }
    }

    IEnumerator WaitCombo()
    {
        // �޺� ����µ� �ɸ��� �ð�

        // �޺��� �ִ�ġ�� ��� �ٽ� ó�� �޺��� ���ư�
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

        // comboBool�� ������ �޺��� ����ٰ� �Ǵ��ϰ� ���� �޺��� 0���� �ٲ�
        if (comboBool)
        {
            combo = 0;
            anim.SetInteger("SwingCombo", combo);
        }

        // �޺� ���� �ƴ� ��� �� �ܻ� ����Ʈ�� ��
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
        // �̰� �� ���ָ� ��� ����ź ������ ��� �ݺ���.
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
        for (int i = 0; i < 10; i++)
        {
            cineFreeLook.m_YAxis.Value += 0.002f;
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator PaintRecovery()
    {
        GameManager.Instance.player.paintRecovery = false;
        yield return new WaitForSeconds(4f);
        GameManager.Instance.player.paintRecovery = true;
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

        prefab[0].transform.rotation = Quaternion.Euler(
            GameManager.Instance.player.cam.transform.eulerAngles.x, 
            GameManager.Instance.player.cam.transform.eulerAngles.y, 
            GameManager.Instance.player.cam.transform.eulerAngles.z
            );


        if (Input.GetMouseButton(0))
        {
            if (GameManager.Instance.player.paintValue >= sprayConsumption && sprayPaintMin)
            {
                StopCoroutine("PaintRecovery");
                prefab[0].SetActive(true);
                GameManager.Instance.player.paintValue -= sprayConsumption;
                StartCoroutine("PaintRecovery");
            }

            else if (GameManager.Instance.player.paintValue < sprayConsumption)
            {
                prefab[0].SetActive(false);
                sprayPaintMin = false;
            }

        }

        if (GameManager.Instance.player.paintValue > 10f)
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

        // ���� �޺����� ����
        combo = anim.GetInteger("SwingCombo");

        // Swing
        if (Input.GetMouseButtonDown(0))
        {
            if (swingBool)
            {
                effect[0].SetActive(true);
                // ���ݽ� �޺� Ȯ�� �ڷ�ƾ ��ž
                StopCoroutine("WaitCombo");
                anim.SetInteger("SwingCombo", combo + 1);
                comboBool = false;

                // ������ ������ �޺��� ������� Ȯ���ϴ� �ڷ�ƾ ����
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
            if ((!bulletCool && FPS && GameManager.Instance.player.paintValue >= SRpaintGunConsumption) || (!bulletCool && !FPS && GameManager.Instance.player.paintValue >= ARpaintGunConsumption))
            {
                StopCoroutine("PaintRecovery");
                var ray = Camera.main.ScreenPointToRay(ScreenCenter);
                var rotation = Quaternion.LookRotation(ray.direction);

                var clone = Instantiate(prefab[1], prefab[1].transform.position, rotation);

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
                    GameManager.Instance.player.paintValue -= SRpaintGunConsumption;
                }

                if (!FPS)
                {
                    StartCoroutine(BulletCooldown(0.2f));
                    GameManager.Instance.player.paintValue -= ARpaintGunConsumption;
                }
            }
        }


        if (Input.GetMouseButtonDown(1))
        {
            if (GameManager.Instance.player.paintValue > SRpaintGunConsumption)
            {
                FPS = true;
            }
        }


        if (Input.GetMouseButtonUp(1))
        {
            FPS = false;
        }

        // paintValue���� 3���� ������, ������ �Ұ���
        if (GameManager.Instance.player.paintValue < SRpaintGunConsumption)
        {
            FPS = false;
        }

        if (FPS)
        {
            StopCoroutine("PaintRecovery");
            // ���ǵ� ���� �ڵ�
            GameManager.Instance.player.speed = srSpeed;
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

        if (!FPS || !GameManager.Instance.player.isGround)
        {
            // ���ǵ� ���� �ڵ�
            GameManager.Instance.player.speed = originSpeed;
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

        prefab[2].transform.rotation = Quaternion.Euler(
            GameManager.Instance.player.cam.transform.eulerAngles.x, 
            GameManager.Instance.player.cam.transform.eulerAngles.y, 
            GameManager.Instance.player.cam.transform.eulerAngles.z
            );

        if (Input.GetMouseButton(0) && GameManager.Instance.player.paintValue >= waterBalloonConsumption)
        {
            if (!waterBalloonCool)
            {
                StopCoroutine("PaintRecovery");
                lineRenderer.enabled = true;
                //CalculateTrajectory();
                StartCoroutine("PaintRecovery");
            }
        }

        if (Input.GetMouseButtonUp(0) && GameManager.Instance.player.paintValue >= waterBalloonConsumption)
        {
            if (!waterBalloonCool)
            {
                anim.SetBool("IsWaterBalloon", true);

                var ray = Camera.main.ScreenPointToRay(ScreenCenter);
                var rotation = Quaternion.LookRotation(ray.direction);

                var clone = Instantiate(prefab[2], prefab[2].transform.position, rotation);

                GameManager.Instance.player.paintValue -= waterBalloonConsumption;

                clone.SetActive(true);

                var cloneRigidbody = clone.GetComponent<Rigidbody>();

                if (cloneRigidbody != null)
                {
                    cloneRigidbody.velocity = clone.transform.forward * 15 + clone.transform.up * 5;
                }

                for (int i = 0; i < 30; i++)
                {
                    //fixedUpdate�� �� ��� ���� ��ġ���� ���̴� ���װ� ����. ���� ��� ��ġ���� ��������� ��.
                    lineRenderer.SetPosition(i, Vector3.zero);
                }
                lineRenderer.enabled = false;

                StartCoroutine(WaitWaterBalloon());
            }
        }
    }

    // ���� ���⸦ Ȱ��ȭ���ִ� �Լ�
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
        GameManager.Instance.player.speed = originSpeed;
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
            //fixedUpdate�� �� ��� ���� ��ġ���� ���̴� ���װ� ����. ���� ��� ��ġ���� ��������� ��.
            lineRenderer.SetPosition(i, Vector3.zero);
        }
        lineRenderer.enabled = false;
    }

    // ��ǳ�� ���� ���ϴ� �ڵ�(��ӵ� ������ �̿��Ͽ� �Ÿ� ��� s = v0*t + (1/2)at^2) ���⼭ ���ӵ��� �߷°��ӵ�����.
    private void CalculateTrajectory()
    {
        Vector3 startPosition = prefab[2].transform.position;
        Vector3 currentVelocity = prefab[2].transform.forward * 15f + prefab[2].transform.up * 5;

        for (int i = 0; i < 30; i++)
        {
            float time = i * 0.1f;
            float x = startPosition.x + (currentVelocity.x * time);
            float y = startPosition.y + (currentVelocity.y * time) + (0.5f * GameManager.Instance.player.gravity * time * time);
            float z = startPosition.z + (currentVelocity.z * time);
            trajectoryPoints[i] = new Vector3(x, y, z);
            lineRenderer.SetPosition(i, trajectoryPoints[i]);
        }
    }
}
