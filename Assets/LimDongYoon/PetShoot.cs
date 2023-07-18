// 기본 BossShoot 클래스

using System;
using UnityEngine;

public class PetShoot : MonoBehaviour
{
    

    public ParticlePoolingManager poolingManager;
    public float fireRate = 1f;
    protected float nextFire = 0f;
    public Vector3 direction = Vector3.forward;  // 발사 방향
    public float speed = 5f;  // 발사 속도
    public GameObject skillPrefab;
    public Transform target;
    public bool isAttack = false;

    public void Update()
    {
        if(target && isAttack)
        if(Time.time > nextFire)
        { 
            direction = transform.position - target.position;
            ShootParticle(transform.position, direction, speed);
            nextFire = Time.time + fireRate;
        }
    }

    public void ShootParticle(Vector3 position, Vector3 direction, float speed)
    {
        // Create an instance from the object pool
        GameObject particle = skillPrefab;
        
        if(particle != null)
        {
            // Set position and rotation
            particle.transform.position = position;
            particle.transform.rotation = Quaternion.LookRotation(direction);
            
            // Apply speed
            Rigidbody rb = particle.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.velocity = speed * direction.normalized;
            }

            // Activate it
            particle.SetActive(true);
        }
    }
    
    
    
}