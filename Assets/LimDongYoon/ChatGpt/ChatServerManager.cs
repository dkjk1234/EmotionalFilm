using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.Networking;
using System.Threading.Tasks;

public static class ChatServerManager
{
    public static string url = "http://localhost:3000/chat";
   /* public void Start()
    {
        List<ChatCompletionMessage3> messages = new List<ChatCompletionMessage3>();

        ChatCompletionMessage3 a = new ChatCompletionMessage3
        {
            role = "system",
            content = "너는 나무꾼이야 말끝마다 나무를 붙여"
        };
        ChatCompletionMessage3 b = new ChatCompletionMessage3
        {
            role = "user",
            content = "뭐하고있어?"
        };

        messages.Add(a);
        messages.Add(b);
        Debug.Log(messages[0]);
        
        
        //await PostRequest(messages);
    }
*/
    public static async Task<string> ChatPostRequest(List<_ChatCompletionMessage> messages)
    {
        
        RequestBody testbody = new RequestBody
        {
            messages = messages
        };
        string json = JsonUtility.ToJson((object)testbody, true);
        Debug.Log(json);

        var request = new UnityWebRequest("http://localhost:3000/chat", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        var tcs = new TaskCompletionSource<bool>();
        request.SendWebRequest().completed += _ => tcs.SetResult(true);

        await tcs.Task;

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
            
            return null;
        }
        else
        {
            Debug.Log("POST successful!");
            Debug.Log("Received: " + request.downloadHandler.text);

            /*string[] splitString = originalString.Split('/');
            string result = splitString[0];*/

            return request.downloadHandler.text;
        }
    }

    /*  public static IEnumerator PostRequest(string url, string json, Action<string> result)
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
              string response = request.downloadHandler.text;
              result(response);
              Debug.Log("POST successful! " + "Received: " + response);

          }
      }*/

}

[Serializable]
public class _ChatCompletionMessage
{
    public string role;
    public string content;
}
[Serializable]
public class RequestBody
{
    public List<_ChatCompletionMessage> messages;
    public string model;
}





