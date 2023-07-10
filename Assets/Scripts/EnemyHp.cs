using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Transform hpBar;
    public Renderer hpBarRenderer;

    private void Start()
    {
        currentHealth = maxHealth;
        hpBar.localScale = new Vector3(1f, 1f, 1f); // 초기 HP 바 크기
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // HP 바 크기 조정
        float hpRatio = (float)currentHealth / maxHealth;
        hpBar.localScale = new Vector3(hpRatio, 1f, 1f);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // 적 사망 처리
        Destroy(gameObject);
    }
}
