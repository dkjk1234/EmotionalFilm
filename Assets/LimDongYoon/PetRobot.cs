using System.Collections.Generic;
using UnityEngine;

public class PetRobot : MonoBehaviour
{
    

    [SerializeField]
    [Range(0, 3f)] private float lerpSpeed = 1f;

    private PetShoot petShoot;
    [Header("FollowPlayer")]
    private Vector3 initialOffset;
    private GameObject nearestObject;
    private GameObject lastNearestObject;
    [SerializeField]
    private float lookAtTimer = 2f;
    
    
    public Transform playerTransform;
    public Transform targetObject;
    
    public GameObject projectilePrefab;
    
    public float detectionRange;
    public float defenseRange;
    
    public State state;
    public List<GameObject> detectedObjects = new List<GameObject>();
    public GameObject spray;

    void Start()
    {
        petShoot = GetComponent<PetShoot>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        initialOffset = transform.position - playerTransform.position;
        spray = GetComponentInChildren<ParticleSystem>(true).gameObject;

    }
    public void MoveToTarget(Vector3 targetPosition)
    {
        targetPosition += initialOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime);

        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange);
        float shortestDistance = Mathf.Infinity;
        nearestObject = null;

        foreach (Collider collider in colliders)
        {
            if (!collider.gameObject.CompareTag("Ground") && !collider.gameObject.CompareTag("Player"))
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearestObject = collider.gameObject;
                }
            }
        }
    }
    void Update()
    {
        if (state != State.Attack) petShoot.isAttack = false;
        if(state != State.Health) spray.SetActive(false);
        Collider[] enemies = Physics.OverlapSphere(transform.position, detectionRange, LayerMask.GetMask("Enemy"));
        switch (state)
        {
                
            case State.FollowPlayer:
                Vector3 targetPosition = playerTransform.position + initialOffset;
                MoveToTarget(playerTransform.position);

               
                break;

            case State.Attack:
                
                
                if (enemies.Length > 0)
                {
                    petShoot.isAttack = true;
                    petShoot.target = enemies[0].transform;
                 //   GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                    MoveToTarget(enemies[0].transform.position);
                        //projectile.GetComponent<Projectile>().target = enemies[0].transform;
                }
                else
                {
                    MoveToTarget(playerTransform.position);
                }
                break;

            case State.Defense:

                Collider[] incomingProjectiles = Physics.OverlapSphere(transform.position, defenseRange, LayerMask.GetMask("EnemyProjectile"));
                if (incomingProjectiles.Length > 0)
                {
                    Destroy(incomingProjectiles[0].gameObject);
                }
                else
                {
                    MoveToTarget(playerTransform.position);
                }
                break;

            case State.Guide:
                // TODO: 플레이어에게 이동 경로를 표시하거나 안내
                
                enemies = Physics.OverlapSphere(transform.position, detectionRange, LayerMask.GetMask("Enemy"));

                if (enemies.Length > 0)
                {
                    MoveToTarget(enemies[0].transform.position);
                }
                else
                {
                    MoveToTarget(playerTransform.position);
                }

                targetObject = nearestObject.transform;
                transform.position = Vector3.Lerp(transform.position, targetObject.position, Time.deltaTime);
                break;

            case State.Speak:
                // TODO: 음성 또는 텍스트 기반 대화를 구현
                break;
            case State.Health:
                spray.SetActive(true);
                break;
            case State.Explore:
                Collider[] objects = Physics.OverlapSphere(transform.position, detectionRange, LayerMask.GetMask("Object"));
                detectedObjects = new List<GameObject>();
                foreach (var obj in objects)
                {
                    detectedObjects.Add(obj.gameObject);
                }
                
                break;
        }
    }
}
public enum State
{
    Health,
    FollowPlayer,
    Attack,
    Defense,
    Guide,
    Speak,
    Explore
}