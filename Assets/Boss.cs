using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    Animator animator;
    SpiralShoot bossShoot;

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

    public bool isAttack = false;

    float targetDistance = 0f;

    public enum BossState
    {
        Idle,
        Move,
        Attack,

    }

    public void AnimationEndEvent()
    {
        // Get the clip from the animator
        AnimationClip clip = animator.runtimeAnimatorController.animationClips[0];

        // Check if the clip is the correct one
        if (clip.name == "Attack")
        {
            // Create a new AnimationEvent
            AnimationEvent animationEvent = new AnimationEvent
            {
                // Set the function to be called when the event is triggered
                functionName = "AnimationEnded",
                // Set the time for the event
                time = clip.length
            };

            // Add the event to the clip
            clip.AddEvent(animationEvent);
        }
    }
    public void AnimationEnded()
    {
        Debug.Log("Animation ended!");
    }
    public void LookPlayer(bool immediately)
    {
        Vector3 directionToPlayer = player.position - transform.position; // The direction to the player
        Quaternion rotationToPlayer = Quaternion.LookRotation(directionToPlayer); // The rotation to look at the player
        if(immediately) transform.rotation = rotationToPlayer;
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

        state = BossState.Idle;
        startRotation = transform.rotation;
        animator = GetComponent<Animator>();
        bossShoot = GetComponent<SpiralShoot>();


        if (player != null)
        {
            state = BossState.Move;
        }

    }
    void Update()
    {
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
                    SelectAttack();
                }
                else if(!isAttack)
                {
                    LookPlayer(true);
                }
                break;
        }
    }

    public void SelectAttack()
    {
        
        var numState = Random.Range(0, 3);
        if (numState == 0) Attack();
        else FarAttack();
        

    }
    public void SprialAttack()
    {
        
    }
    public void Attack()
    {
        Debug.Log("Attack");
        StartCoroutine(RotateOverTime(rotationTime));
        animator.SetTrigger("ParticleAttack");
        bossShoot.ShootParticle(transform.position, player.position - transform.position, 2f);
        // Calculate the target rotation
        targetRotation = Quaternion.Euler(rotationAngle, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        // Start the rotation coroutine
        
    }
    public void FarAttack()
    {
        Debug.Log("FarAttack");
        animator.SetTrigger("FarAttack");
        // Calculate the target rotation
        targetRotation = Quaternion.Euler(rotationAngle - 10, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        // Start the rotation coroutine
        StartCoroutine(RotateOverTime(rotationTime));
    }


    IEnumerator RotateOverTime(float time)
    {


        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            // Calculate the new rotation
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, (elapsedTime / time));
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Ensure the rotation is exactly the target
        transform.rotation = targetRotation;

        // And rotate back
        StartCoroutine(RotateBackOverTime(time));
    }

    IEnumerator RotateBackOverTime(float time)
    {
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            // Calculate the new rotation
            transform.rotation = Quaternion.Lerp(targetRotation, startRotation, (elapsedTime / time));
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Ensure the rotation is exactly the start rotation
        transform.rotation = startRotation;
        isAttack = false;
    }

    // Update is called once per frame

}
