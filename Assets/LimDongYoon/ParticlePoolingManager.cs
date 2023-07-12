using System;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePoolingManager : MonoBehaviour
{
    public static ParticlePoolingManager instance;
    public GameObject particlePrefab;
    public Dictionary<string, Stack<GameObject>> poolingDic = new Dictionary<string, Stack<GameObject>>();
    public int poolSize = 20;
    private List<GameObject> particlePool;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        particlePool = new List<GameObject>();
        
        // Instantiate particles and add them to the pool
        for(int i = 0; i < poolSize; i++)
        {
            GameObject particle = Instantiate(particlePrefab, Vector3.zero, Quaternion.identity);
            particle.SetActive(false);
            particlePool.Add(particle);
        }
    }

    public GameObject GetPooledParticle()
    {
        foreach(GameObject particle in particlePool)
        {
            if(!particle.activeInHierarchy)
            {
                return particle;
            }
        }

        // If there is no available object in the pool, create a new one
        GameObject newParticle = Instantiate(particlePrefab, Vector3.zero, Quaternion.identity);
        newParticle.SetActive(false);
        particlePool.Add(newParticle);

        return newParticle;
    }
}