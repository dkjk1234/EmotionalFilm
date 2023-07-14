using System.Collections.Generic;
using UnityEngine;

public class PetRobot : MonoBehaviour
{
    public enum State
    {
        FollowPlayer,
        Attack,
        Defense,
        Guide,
        Speak,
        Explore
    }

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
        initialOffset = transform.position - playerTransform.position;
    }
    void Update()
    {
        switch (state)
        {
            case State.FollowPlayer:
                Vector3 targetPosition = playerTransform.position + initialOffset;
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime);

                Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange);
                float shortestDistance = Mathf.Infinity;
                nearestObject = null;

                foreach (Collider collider in colliders)
                {
                    if (!collider.gameObject.CompareTag("Ground")&&!collider.gameObject.CompareTag("Player"))
                    {
                        float distance = Vector3.Distance(transform.position, collider.transform.position);
                        if (distance < shortestDistance)
                        {
                            shortestDistance = distance;
                            nearestObject = collider.gameObject;
                        }
                    }
                }

                if (nearestObject != null)
                {
                    if (nearestObject != lastNearestObject)
                    {
                        lookAtTimer = 0f;
                        lastNearestObject = nearestObject;
                    }

                    if (lookAtTimer < 3f) // 3초 동안만 오브젝트를 바라봅니다.
                    {
                        Vector3 directionToNearestObject = (nearestObject.transform.position - transform.position).normalized;
                        Quaternion targetRotation = Quaternion.LookRotation(directionToNearestObject);
                        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, lerpSpeed * Time.deltaTime);
                        lookAtTimer += Time.deltaTime;
                    }
                }
                else
                {
                    lastNearestObject = null;
                    lookAtTimer = 0f;
                    
                }
                break;

            case State.Attack:
                Collider[] enemies = Physics.OverlapSphere(transform.position, detectionRange, LayerMask.GetMask("Enemy"));
                
                if (enemies.Length > 0)
                {
                    GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
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