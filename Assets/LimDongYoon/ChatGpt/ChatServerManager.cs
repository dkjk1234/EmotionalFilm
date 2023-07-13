using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Unity.VisualScripting;
using UnityEngine.Networking;

public class RequestTest
{
    public List<ChatCompletionMessage2> messages;
}

public class ChatServerManager : MonoBehaviour
{
    public void Start()
    {
        List<ChatCompletionMessage2> messages = new List<ChatCompletionMessage2>();
        
        ChatCompletionMessage2 a = new ChatCompletionMessage2
        {
            role = "system",
            content = "너는 나무꾼이야 말끝마다 나무를 붙여"
        };
        ChatCompletionMessage2 b = new ChatCompletionMessage2
        {
            role = "user",
            content = "뭐하고있어?"
        };
        messages.Add(a);
        messages.Add(b);
        Debug.Log(messages[0]);
        RequestTest testbody = new RequestTest
        {
            messages = messages
        };
        string json = JsonUtility.ToJson((object)testbody,true);
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


