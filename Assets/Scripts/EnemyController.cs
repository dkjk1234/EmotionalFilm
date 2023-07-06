using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public GameObject bulletPrefab;
    public int poolSize = 5;
    private GameObject[] bulletPool;

    public float fireRate = 3f;

    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        bulletPool = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            bulletPool[i] = Instantiate(bulletPrefab);
            bulletPool[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= fireRate)
        {
            FireBullet();
            timer = 0f;
        }    
    }

    private void FireBullet()
    {
        for(int i = 0; i < poolSize; i++)
        {
            if(!bulletPool[i].activeInHierarchy)
            {
                bulletPool[i].SetActive(true);
                bulletPool[i].transform.position = transform.position;
                Vector3 direction = Vector3.forward;
                bulletPool[i].GetComponent<Bullet>().Initialized(direction);
                break;
            }
        }
    }
}