// 기본 BossShoot 클래스

using UnityEngine;

public class BossShoot : MonoBehaviour
{
    public ParticlePoolingManager poolingManager;
    public float fireRate = 1f;
    protected float nextFire = 0f;
    public Vector3 direction = Vector3.forward;  // 발사 방향
    public float speed = 5f;  // 발사 속도
    public GameObject skillPrefab;

    // Update is called once per frame
    protected virtual void Update()
    {
        if(Time.time > nextFire)
        {
            ShootParticle(transform.position, direction, speed);
            nextFire = Time.time + fireRate;
        }
    }

    public void ShootParticle(Vector3 position, Vector3 direction, float speed)
    {
        // Create an instance from the object pool
        GameObject particle = poolingManager.GetPooledParticle();
        
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
    
    
    protected void ShootParticle(Vector3 position, float speed)
    {
        // Create an instance from the object pool
        GameObject particle = poolingManager.GetPooledParticle();
        
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