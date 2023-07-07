using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public GameObject particlePrefab;
    public int poolSize = 5;
    public float delay = 3;
    private GameObject[] particlePool;
    public float moveRange = 20f;
    private float timer = 0f;
    private Animator anim;
    private NavMeshAgent nav;
    public bool isAttack;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
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
        if (timer >= delay)
        {
            Move();
            timer = 0;
        }
    }

    private void FixedUpdate()
    {
        
        Targeting();
        Move();
        
    }

    private void Targeting()
    {
        float targetRadius = 30f;
        float targetRange = 3f;

        Collider[] colliders = Physics.OverlapSphere(transform.position, targetRadius, LayerMask.GetMask("Player"));
        foreach (Collider collider in colliders)
        {
            isAttack = true;
            Attack();
        }
    }

    private void OnDrawGizmosSelected()
    {
        float targetRadius = 1.5f;
        Gizmos.DrawWireSphere(transform.position, 1.5f);
        Gizmos.color = Color.yellow;
        
    }

    private void Move()
    {
        if (!nav.pathPending && nav.remainingDistance < 0.5f)
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
        int ranNumber = Random.Range(0, 3);
        anim.SetFloat("Attack", ranNumber);
        anim.SetTrigger("doAttack");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 directionToPlayer = player.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

        FireBullet(directionToPlayer);
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
    }
}