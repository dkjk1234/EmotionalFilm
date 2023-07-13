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
        hpBar.localScale = new Vector3(1f, 1f, 1f); // �ʱ� HP �� ũ��
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // HP �� ũ�� ����
        float hpRatio = (float)currentHealth / maxHealth;
        hpBar.localScale = new Vector3(hpRatio, 1f, 1f);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // �� ��� ó��
        Destroy(gameObject);
    }
}
