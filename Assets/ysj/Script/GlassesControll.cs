using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GlassesControll : MonoBehaviour
{
    public GameObject character;
    public List<GameObject> glassesPrefabs;

    private int currentGlassesIndex = 0;
    private GameObject currentGlassesPrefab;
 

    public void PreviousHat()
    {
        if (currentGlassesPrefab != null)
        {
            currentGlassesPrefab.SetActive(false);
        }

        currentGlassesIndex--;
        if (currentGlassesIndex < 0)
        {
            currentGlassesIndex = glassesPrefabs.Count - 1;
        }

        currentGlassesPrefab = glassesPrefabs[currentGlassesIndex];
        currentGlassesPrefab.SetActive(true);
    }

    public void NextHat()
    {

        if (currentGlassesPrefab != null)
        {
            currentGlassesPrefab.SetActive(false);
        }

        currentGlassesIndex++;
        if (currentGlassesIndex >= glassesPrefabs.Count)
        {
            currentGlassesIndex = 0;
        }

        currentGlassesPrefab = glassesPrefabs[currentGlassesIndex];
        currentGlassesPrefab.SetActive(true);
    }
}