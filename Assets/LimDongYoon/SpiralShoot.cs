// SpiralShoot 클래스

using UnityEngine;

public class SpiralShoot : BossShoot
{
    public float spiralSpeed = 5f;  // 나선형 발사 시 회전 속도
    private float currentAngle = 0f;  // 나선형 발사 시 현재 각도


    protected override void Update()
    {
        if(Time.time > nextFire)
        {
            // Calculate the direction of spiral shooting
            float rad = currentAngle * Mathf.Deg2Rad;
            Vector3 spiralDirection = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad));
            ShootParticle(transform.position, spiralDirection, speed);
            // Update the angle
            currentAngle += spiralSpeed;

            nextFire = Time.time + fireRate;
        }
    }
 
}