using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bullet : MonoBehaviour
{
    public float speed = 10f;
    public float fadeDuration = 1f;
    private ParticleSystem particleSystem;
    private ParticleCollisionEvent[] collisionEvents;
    

    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        collisionEvents = new ParticleCollisionEvent[16];
       
    }


    private void OnParticleCollision(GameObject other)
    {
        int numCollisions = particleSystem.GetCollisionEvents(other, collisionEvents);
        for (int i = 0; i < numCollisions; i++)
        {
            Vector3 collisionPosition = collisionEvents[i].intersection;
            Vector3 collisionNormal = collisionEvents[i].normal;
        }
    }
 

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, speed * Time.deltaTime, 0);
        Destroy(this.gameObject, 5);

        //if(collision.gameObject\.tag == "Player")
        //{
        //
        //}
    }
}
