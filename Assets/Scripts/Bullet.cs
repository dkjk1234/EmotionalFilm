using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    private Vector3 direction;
    private bool isActive = false;

    public void Initialized(Vector3 dir)
    {
        direction = dir.normalized;
        isActive = true;
    }

    private void Update()
    {
        if (isActive)
        {
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    private void OnBecameInvisible()
    {
        // 총알이 화면 밖으로 나가면 비활성화
        isActive = false;
        gameObject.SetActive(false);
    }
}
