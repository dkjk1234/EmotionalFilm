using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoiceButton : MonoBehaviour
{

    void Start()
    {
        
    }

    public void SceneChange()
    {

        SceneManager.LoadScene("Emotional");
    }
}
