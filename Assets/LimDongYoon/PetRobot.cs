using System.Collections.Generic;
using UnityEngine;

public class PetRobot : MonoBehaviour
{
    

    [SerializeField]
    [Range(0, 3f)] private float lerpSpeed = 1f;
    
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

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        initialOffset = transform.position - playerTransform.position;
        

    }
    public void MoveToTarget(Vector3 targetPosition)
    {
        targetPosition = targetPosition + initialOffset;
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
        switch (state)
        {
            case State.FollowPlayer:
                Vector3 targetPosition = playerTransform.position + initialOffset;
                MoveToTarget(playerTransform.position);

               
                break;

            case State.Attack:
                Collider[] enemies = Physics.OverlapSphere(transform.position, detectionRange, LayerMask.GetMask("Enemy"));
                
                if (enemies.Length > 0)
                {
                    GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                    MoveToTarget(enemies[0].transform.position);
                        //projectile.GetComponent<Projectile>().target = enemies[0].transform;
                }
                break;

            case State.Defense:
                Collider[] incomingProjectiles = Physics.OverlapSphere(transform.position, defenseRange, LayerMask.GetMask("EnemyProjectile"));
                if (incomingProjectiles.Length > 0)
                {
                    Destroy(incomingProjectiles[0].gameObject);
                }
                break;

            case State.Guide:
                // TODO: 플레이어에게 이동 경로를 표시하거나 안내
                targetObject = nearestObject.transform;
                transform.position = Vector3.Lerp(transform.position, targetObject.position, Time.deltaTime);
                break;

            case State.Speak:
                // TODO: 음성 또는 텍스트 기반 대화를 구현
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
    FollowPlayer,
    Attack,
    Defense,
    Guide,
    Speak,
    Explore
}