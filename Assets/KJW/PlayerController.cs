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
        // �޺� ���� �ƴ� ��� �� �ܻ� ����Ʈ�� ��
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
        // �̰� �� ���ָ� ��� ����ź ������ ��� �ݺ���.
        anim.SetBool("IsWaterBalloon", false);
        yield return new WaitForSeconds(0.4f);
        waterBalloonCool = false;
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
        combo = anim.GetInteger("SwingCombo");

        // comboBool�� ������ �޺��� ����ٰ� �Ǵ��ϰ� ���� �޺��� 0���� �ٲ�
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
        // ���� �߿��� ���� �Ұ����ϰ� ���� �ڵ� �߰��� ��
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
    }
}