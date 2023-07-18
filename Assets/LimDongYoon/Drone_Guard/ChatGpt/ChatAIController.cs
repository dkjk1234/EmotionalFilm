using LeastSquares.Spark;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatAIController : MonoBehaviour
{
    public enum ChatAIState
    {
        Idle,
        Chatting,
        Loading
      
    }
    public ChatAIState chatAIstate = ChatAIState.Idle;
    public string currentStatePrompt;
    public PetAIDialog Dialogue;
    public TMP_InputField input;
    public Image imageToFade;
    public GameObject pannel;
    public float fadeDuration = 1.0f;


  

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (chatAIstate)
            {
                case ChatAIState.Idle: 
                    OpenChatWindow();
                    break;
                case ChatAIState.Chatting:
                    Talk();
                    break;
            }
            
        }
    }
    public void OpenChatWindow() 
    {
        chatAIstate = ChatAIState.Chatting;
        input.gameObject.SetActive(true);
        
        input.ActivateInputField();
    }
    public void Talk()
    {
        chatAIstate = ChatAIState.Loading;
        input.gameObject.SetActive(false);
        Dialogue.Talk(input.text);
    }
    public void EndTalk() 
    {
        chatAIstate = ChatAIState.Idle;
    }

    public void FadeIn()
    {
        StartCoroutine(FadeImage(true));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeImage(false));
    }
    IEnumerator FadeImage(bool fadeIn)
    {
        float startTime = Time.time;
        Color color = imageToFade.color;
        while (Time.time - startTime < fadeDuration)
        {
            float t = (Time.time - startTime) / fadeDuration;
            color.a = fadeIn ? t : 1.0f - t;
            imageToFade.color = color;
            yield return null;
        }

        // Ensure the fade is completely finished
        color.a = fadeIn ? 1.0f : 0.0f;
        imageToFade.color = color;
    }
}
