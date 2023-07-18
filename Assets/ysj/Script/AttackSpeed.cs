using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackSpeed : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform player;
    public Transform firePoint;
    public Animator animator;
    public float attackRange = 3f;
    public bool shouldAttak = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if(distanceToPlayer <= attackRange ) 
        {
            transform.LookAt(player);
            Attack();
            shouldAttak = true;
        
        }
        else
        {
            shouldAttak = false;
        }
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");
        GameObject bullet = Instantiate(bulletPrefab, transform);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * 40, ForceMode.Impulse);
    }
}
