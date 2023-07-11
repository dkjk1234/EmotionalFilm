using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bullet : MonoBehaviour
{
    public float speed = 10f;
    public Image image;
    public float fadeDuration = 1f;
    private ParticleSystem particleSystem;
    private ParticleCollisionEvent[] collisionEvents;
    public bool isCollided = false;

    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        collisionEvents = new ParticleCollisionEvent[16];
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FadeInAndOutImage());
            if(!isCollided)
            {
                isCollided = true;
            }
        }
    }

    private IEnumerator FadeInAndOutImage()
    {
        yield return StartCoroutine(FadeIn());
        yield return new WaitForSeconds(fadeDuration);
        yield return StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);

    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
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
