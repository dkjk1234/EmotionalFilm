using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HatControll : MonoBehaviour
{
    public GameObject character;
    public List<GameObject> hatPrefabs;

    private int currentHatIndex = 0;
    private GameObject currentHatPrefab;

    private void Start()
    {

    }

    public void PreviousHat()
    {
        if (currentHatPrefab != null)
        {
            currentHatPrefab.SetActive(false);
        }

        currentHatIndex--;
        if (currentHatIndex < 0)
        {
            currentHatIndex = hatPrefabs.Count - 1;
        }

        currentHatPrefab = hatPrefabs[currentHatIndex];
        currentHatPrefab.SetActive(true);
    }

    public void NextHat()
    {

        if (currentHatPrefab != null)
        {
            currentHatPrefab.SetActive(false);
        }

        currentHatIndex++;
        if (currentHatIndex >= hatPrefabs.Count)
        {
            currentHatIndex = 0;
        }

        currentHatPrefab = hatPrefabs[currentHatIndex];
        currentHatPrefab.SetActive(true);
    }
}