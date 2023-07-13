using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Unity.VisualScripting;
using UnityEngine.Networking;

class RequestTest
{
    public ChatCompletionMessage2[] messages;
}

public class ChatServerManager : MonoBehaviour
{
    public void Start()
    {
        ChatCompletionMessage2[] messages = new ChatCompletionMessage2[2];
        var a = new ChatCompletionMessage2
        {
            role = "system",
            content = "너는 나무꾼이야 말끝마다 나무를 붙여"
        };
        var b = new ChatCompletionMessage2
        {
            role = "user",
            content = "뭐하고있어?"
        };
        messages[0] = a;
        messages[1] = b;
        Debug.Log(messages);
        RequestTest testbody = new RequestTest
        {
            messages = messages
        };
        string json = JsonUtility.ToJson(testbody);
        StartCoroutine(PostRequest("http://localhost:3000/chat", json));
    }

  
    IEnumerator PostRequest(string url, string json)
    {
        Debug.Log(json);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log("POST successful!");
            Debug.Log("Received: " + request.downloadHandler.text);
        }
    }

    /*public enum chatModle
    {
        gpt3,
        gpt3_5
    }
    HTTPRequest request = new HTTPRequest(new Uri("http://lcoalhost:3000/chat"), HTTPMethods.Post, OnRequestFinished);

    public void TestRequest()
    {
        ChatCompletionMessage2[] messages = new ChatCompletionMessage2[2];
        var a = new ChatCompletionMessage2
        {
            role = "system",
            content = "너는 나무꾼이야 말끝마다 나무를 붙여"
        };
        var b = new ChatCompletionMessage2
        {
            role = "user",
            content = "뭐하고있어?"
        };
        messages[0] = a;
        messages[1] = b;
        RequsetChatServer(messages);
    }
    public void RequsetChatServer(ChatCompletionMessage2[] messages)
    {
        request.AddField("messages", JsonUtility.ToJson(messages));
        request.Send();
    }
    static void OnRequestFinished(HTTPRequest request, HTTPResponse response)
    {
        Debug.Log("Request Finished! Text received: " + response.DataAsText);
    }*/
    
    

}


