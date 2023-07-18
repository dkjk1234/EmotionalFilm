using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoiceButton : MonoBehaviour
{
    public GameObject characterPrefab;

    void Start()
    {
        
    }

    public void SceneChange()
    {
        GameObject character = Instantiate(characterPrefab);
        DontDestroyOnLoad(character);
        SceneManager.LoadScene("YSJCANDYMAP");
    }
}
