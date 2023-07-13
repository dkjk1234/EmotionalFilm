using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    private Vector3 direction;

    public void Initialized(Vector3 dir)
    {
        direction = dir.normalized;
    }

    private void Update()
    {
            transform.position += direction * speed * Time.deltaTime;
    }
}
