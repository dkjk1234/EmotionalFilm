using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ClotheControll : MonoBehaviour
{
    public GameObject character;
    public List<GameObject> clothePrefabs;

    private int currentClotheIndex = 0;
    private GameObject currentClothePrefab;

    private void Start()
    {

    }

    public void PreviousClothe()
    {
        if (currentClothePrefab != null)
        {
            currentClothePrefab.SetActive(false);
        }

        currentClotheIndex--;
        if (currentClotheIndex < 0)
        {
            currentClotheIndex = clothePrefabs.Count - 1;
        }

        currentClothePrefab = clothePrefabs[currentClotheIndex];
        currentClothePrefab.SetActive(true);
    }

    public void NextClothe()
    {

        if (currentClothePrefab != null)
        {
            currentClothePrefab.SetActive(false);
        }

        currentClotheIndex++;
        if (currentClotheIndex >= clothePrefabs.Count)
        {
            currentClotheIndex = 0;
        }

        currentClothePrefab = clothePrefabs[currentClotheIndex];
        currentClothePrefab.SetActive(true);
    }
}