using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EyeControll : MonoBehaviour
{
    public GameObject character;
    public List<GameObject> eyePrefabs;

    private int currentEyeIndex = 0;
    private GameObject currentEyePrefab;

    private void Start()
    {
    }

    public void PreviousEyes()
    {

        if (currentEyePrefab != null)
        {
            currentEyePrefab.SetActive(false);
            if (currentEyePrefab.name == "Eye01")
            {
                currentEyePrefab.SetActive(false);
            }
        }

        currentEyeIndex--;
        if (currentEyeIndex < 0)
        {
            currentEyeIndex = eyePrefabs.Count - 1;
        }

        currentEyePrefab = eyePrefabs[currentEyeIndex];
        currentEyePrefab.SetActive(true);
    }

    public void NextEyes()
    {

        if (currentEyePrefab != null)
        {
            currentEyePrefab.SetActive(false);
            if (currentEyePrefab.name == "Eye01")
            {
                currentEyePrefab.SetActive(false);
            }
        }

        currentEyeIndex++;
        if (currentEyeIndex >= eyePrefabs.Count)
        {
            currentEyeIndex = 0;
        }

        currentEyePrefab = eyePrefabs[currentEyeIndex];
        currentEyePrefab.SetActive(true);
    }
}
