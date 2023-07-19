using System;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using PaintIn3D;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Random = UnityEngine.Random;

public class Boss : MonoBehaviour
{
    [Range(0, 1f)] public float hpRate = 0.8f;
    
    Animator animator;
    BossShoot bossShoot;

    private P3dChangeCounter p3dChangeCounter;

    Transform player;
    Transform target;
    public float rotationTime = 0.2f; // The time it takes for the rotation to complete
    public float rotationAngle = 30f; // The angle to rotate
    private Quaternion startRotation; // The initial rotation
    private Quaternion targetRotation; // The target rotation

    public float nextAttack = 1f;
    public float attackInterval = 2f;

    //move
    public float speed = 1.0f; // The speed of movement
    public float minDistance = 0.1f; // The minimum distance to target before stopping
    public float returnMoveDistance = 5f;
    public BossState state;

    public Transform shootOrigin;
    public bool isAttack = false;

    float targetDistance = 0f;

    public enum BossState
    {
        Idle,
        Move,
        Attack,

    }

    private void Awake()
    {
       
    }

    public void LookPlayer(bool immediately)
    {
        Vector3 directionToPlayer = player.position - transform.position; // The direction to the player
        Quaternion rotationToPlayer = Quaternion.LookRotation(directionToPlayer); // The rotation to look at the player
        if (immediately) transform.rotation = rotationToPlayer;
        else transform.rotation = Quaternion.Slerp(transform.rotation, rotationToPlayer, Time.deltaTime * 10f); // Smoothly rotate towards the player
    }

    public void FollowTarget(float minDistance)
    {
        if (player != null)
        {

            LookPlayer(false);


            // Only move if the distance is greater than the minimum distance
            if (targetDistance > minDistance)
            {
                // Calculate the direction to the target
                Vector3 direction = (player.position - transform.position).normalized;

                // Move towards the target
                transform.position = transform.position + direction * speed * Time.deltaTime;
            }
            else
            {
                state = BossState.Attack;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        p3dChangeCounter = GetComponentInChildren<P3dChangeCounter>();
        state = BossState.Idle;
        startRotation = transform.rotation;
        animator = GetComponent<Animator>();
        bossShoot = GetComponent<BossShoot>();


        if (player != null)
        {
            state = BossState.Move;
        }

    }
    void Update()
    {
        HealthBarUdpate();
        targetDistance = Vector3.Distance(transform.position, player.position);

        switch (state)
        {
            case BossState.Idle:
                break;
            case BossState.Move:
                FollowTarget(minDistance);
                break;
            case BossState.Attack:
                if (returnMoveDistance < targetDistance) state = BossState.Move;

                if (Time.time > nextAttack && !isAttack)
                {
                    isAttack = true;
                    nextAttack = Time.time + attackInterval;
                    LookPlayer(true);
                    SelectAttack();

                }
                else if (!isAttack)
                {
                    LookPlayer(true);
                }
                break;
        }
    }

    public void HealthBarUdpate()
    {
        var a = 100096 * hpRate;
        GameManager.Instance.uIScript.bossHealthSlider.maxValue = a;
        GameManager.Instance.uIScript.bossHealthSlider.value = (100096 * hpRate) - p3dChangeCounter.Count;
        if ( a - p3dChangeCounter.Count < 0)
        {
            FindObjectOfType<ClearMap>()._ClearMap();
            StartCoroutine(Death());
        }
    }

    IEnumerator Death()
    {
        animator.SetTrigger("Die");
        yield return new WaitForSeconds(3f);
        FindObjectOfType<ClearMap>().bossHealthBar.SetActive(false);
        Destroy(this.gameObject);
    }
   
    public void SelectAttack()
    {

        var numState = Random.Range(0, 3);
        if (numState == 0) Attack();
        else if (numState == 1) FarAttack();
        else if (numState == 2) SpinAttack();


    }
    public void Attack()
    {
        Debug.Log("Attack");
        //StartCoroutine(RotateOverTime(rotationTime))

        animator.SetTrigger("ParticleAttack");
        Vector3 directionToPlayer = transform.position - player.position;
        bossShoot.ShootParticle(transform.position, directionToPlayer, bossShoot.speed);

        // Calculate the target rotation
        isAttack = false;
        animator.SetTrigger("EndAttack");
        // Start the rotation coroutine

    }
    public void FarAttack()
    {
        Debug.Log("FarAttack");
        animator.SetTrigger("FarAttack");
        // Calculate the target rotation
        targetRotation = Quaternion.Euler(rotationAngle - 10, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        isAttack = false;
        animator.SetTrigger("EndAttack");
        // Start the rotation coroutine
        // StartCoroutine(RotateOverTime(rotationTime));
    }
    public void SpinAttack()
    {
        animator.SetTrigger("SpinAttack");
        StartCoroutine(SprialShoot());

    }
    IEnumerator SetCoolTime(bool _isAttack, float sec)
    {
        yield return new WaitForSeconds(sec);
        isAttack = _isAttack;
        if(!isAttack)
        animator.SetTrigger("EndAttack");
    }
    IEnumerator SprialShoot()
    {
        StartCoroutine(SetCoolTime(false, 1f));
        while (isAttack)
        {

            bossShoot.SprialAttack();
            yield return new WaitForSeconds(0.1f);
        }



       

    } }
