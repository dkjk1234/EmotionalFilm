using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MouthControll : MonoBehaviour
{
    public GameObject character;
    public List<GameObject> mouthPrefabs;

    private int currentMouthIndex = 0;
    private GameObject currentMouthPrefab;

    private void Start()
    {

    }

    public void PreviousMouth()
    {

        if (currentMouthPrefab != null)
        {
            currentMouthPrefab.SetActive(false);
            if (currentMouthPrefab.name == "Eye01")
            {
                currentMouthPrefab.SetActive(false);
            }
        }

        currentMouthIndex--;
        if (currentMouthIndex < 0)
        {
            currentMouthIndex = mouthPrefabs.Count - 1;
        }

        currentMouthPrefab = mouthPrefabs[currentMouthIndex];
        currentMouthPrefab.SetActive(true);
    }

    public void NextMouth()
    {

        if (currentMouthPrefab != null)
        {
            currentMouthPrefab.SetActive(false);
            if (currentMouthPrefab.name == "Eye01")
            {
                currentMouthPrefab.SetActive(false);
            }
        }

        currentMouthIndex++;
        if (currentMouthIndex >= mouthPrefabs.Count)
        {
            currentMouthIndex = 0;
        }

        currentMouthPrefab = mouthPrefabs[currentMouthIndex];
        currentMouthPrefab.SetActive(true);
    }
}
