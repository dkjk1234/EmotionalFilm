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
            currentHairIndex = 0; // �ʱⰪ�� �����ϰų� ������ �ʱ� �ε����� �����ؾ� �մϴ�.
        }
    }

    public static void SaveCurrentHairIndex()
    {
        // ���� ��� ���� ���� ����
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