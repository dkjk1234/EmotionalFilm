using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBlindfolded : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Animator animator;
    public string[] fireAnimationNames;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool shouldAttack = false;
        foreach(string animationName in fireAnimationNames) 
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
            {
                shouldAttack = true;
                break;
            }
        }
        if(shouldAttack) 
        {
            Attack();
        }
     
    }

    private void Attack()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * 30, ForceMode.Impulse);

    }

}
