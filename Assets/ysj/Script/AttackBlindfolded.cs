using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackBlindfolded : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform player;
    public Transform firePoint;
    public Animator animator;
    public float attackRange = 3f;
    public bool shouldAttack = false;
    public bool isCollided = false;
    public Image image;
    public float fadeDuration = 1f;
    public float showDuration = 5f;

    private Coroutine fadeCoroutine; 

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            transform.LookAt(player);
            Attack();
            shouldAttack = true;
        }
        else
        {
            shouldAttack = false;
        }
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * 30, ForceMode.Impulse);

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeInAndOutImage());
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            isCollided = true;
        }
    }

    private IEnumerator FadeInAndOutImage()
    {
        yield return StartCoroutine(FadeIn());
        yield return new WaitForSeconds(showDuration);
        yield return StartCoroutine(FadeOut());
        fadeCoroutine = null;
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
}
