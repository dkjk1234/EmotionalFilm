using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public GameObject particlePrefab;
    public int poolSize = 5;
    public float attackDelay = 3f;
    public float attackTime = 0f;
    public float delay = 3;
    private GameObject[] particlePool;
    public float moveRange = 20f;
    private float timer = 0f;
    private Animator anim;
    private NavMeshAgent nav;
    public bool isAttack;
    public bool isMove;
    public int curHealth;
    public int maxHealth;

    public GameObject hpBarPrefab;
    public Vector3 hpBarOffset = new Vector3(0, 2.2f, 0);

    private Canvas uiCanvas;
    private Image hpBarImage;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        SetHpBar();
        particlePool = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            particlePool[i] = Instantiate(particlePrefab);
            particlePool[i].SetActive(false);
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        attackTime += Time.deltaTime;
        if (timer >= attackDelay && !isAttack)
        {
            Move();
            timer = 0f;
        }
    }

    private void FixedUpdate()
    {
        Targeting();
        Move();
    }

    void SetHpBar()
    {
        uiCanvas = GameObject.Find("UI Canvas").GetComponent<Canvas>();
        GameObject hpBar = Instantiate<GameObject>(hpBarPrefab, uiCanvas.transform);
        hpBarImage = hpBar.GetComponentsInChildren<Image>()[1];

        var _hpbar = hpBar.GetComponent<EnemyHpBar>();
        _hpbar.targetTr = this.gameObject.transform;
        _hpbar.offset = hpBarOffset;
    }

    private void Targeting()
    {
        float targetRadius = 5f;

        Collider[] colliders = Physics.OverlapSphere(transform.position, targetRadius, LayerMask.GetMask("Player"));
        if (colliders.Length > 0 && !isAttack)
        {
            Attack();
        }
    }

    private void OnDrawGizmosSelected()
    {
        float targetRadius = 5f;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, targetRadius);
    }

    private void Move()
    {
        if (!isAttack && !nav.pathPending && nav.remainingDistance < 0.5f)
        {
            Vector3 randomPoint = GetRandomPoint();
            nav.SetDestination(randomPoint);
        }
    }

    private Vector3 GetRandomPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * moveRange;
        randomDirection += transform.position;
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(randomDirection, out navMeshHit, moveRange, NavMesh.AllAreas);
        return navMeshHit.position;
    }

    private void Attack()
    {
        if (!isAttack && attackDelay <= attackTime)
        {
            isAttack = true;
            nav.isStopped = true; // NavMeshAgent 이동 중지

            int ranNumber = Random.Range(0, 3);
            anim.SetFloat("Attack", ranNumber);
            anim.SetTrigger("doAttack");
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Vector3 directionToPlayer = player.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);
            //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            transform.rotation = targetRotation;
            FireBullet(directionToPlayer);
        }
    }

    private void FireBullet(Vector3 direction)
    {
        for (int i = 0; i < poolSize; i++)
        {
            if (!particlePool[i].activeInHierarchy)
            {
                particlePool[i].SetActive(true);
                particlePool[i].transform.position = transform.position;

                particlePool[i].GetComponent<Bullet>().Initialized(direction);
                StartCoroutine(DisableParticle(particlePool[i]));
                break;
            }
        }
    }

    private IEnumerator DisableParticle(GameObject particleObject)
    {
        yield return new WaitForSeconds(delay);
        particleObject.SetActive(false);
        nav.isStopped = false; // NavMeshAgent 이동 재개
        isAttack = false;
        attackTime = 0f;
    }
}
