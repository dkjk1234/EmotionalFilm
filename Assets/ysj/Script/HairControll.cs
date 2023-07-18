using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HairControll : MonoBehaviour
{
    public GameObject character; 
    public List<GameObject> hairPrefabs; 

    private static int currentHairIndex = 0; 
    private GameObject currentHairPrefab;

    private void Start()
    {

        if (PlayerPrefs.HasKey("CurrentHairIndex"))
        {
            currentHairIndex = PlayerPrefs.GetInt("CurrentHairIndex");
        }
        else
        {
            currentHairIndex = 0; // 초기값을 설정하거나 적절한 초기 인덱스로 설정해야 합니다.
        }
    }

    public static void SaveCurrentHairIndex()
    {
        // 현재 헤어 선택 정보 저장
        PlayerPrefs.SetInt("CurrentHairIndex", currentHairIndex);
    }

    public void PreviousHair()
    {
        if (currentHairPrefab != null)
        {
            currentHairPrefab.SetActive(false);
        }

        currentHairIndex--;
        if (currentHairIndex < 0)
        {
            currentHairIndex = hairPrefabs.Count - 1;
        }

        currentHairPrefab = hairPrefabs[currentHairIndex];
        currentHairPrefab.SetActive(true);
    }

    public void NextHair()
    {

        if (currentHairPrefab != null)
        {
            currentHairPrefab.SetActive(false);
        }

        currentHairIndex++;
        if (currentHairIndex >= hairPrefabs.Count)
        {
            currentHairIndex = 0;
        }

        currentHairPrefab = hairPrefabs[currentHairIndex];
        currentHairPrefab.SetActive(true);
    }
}