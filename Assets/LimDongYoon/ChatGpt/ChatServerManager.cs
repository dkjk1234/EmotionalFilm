using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.Networking;
using System.Threading.Tasks;

/*[Description("GPT 3.5 Turbo")] ChatGPT3,
[Description("GPT 4")] ChatGPT4,
[Description("GPT 4 32K")] ChatGPT432K,
[Description("GPT 3.5 16K")] ChatGPT316K,*/
public static class ChatServerManager
{
    public static string url = "http://localhost:3000/chat";
    
    public static async Task<string> ChatPostRequest(List<_ChatCompletionMessage> messages)
    {
        
        RequestBody testbody = new RequestBody
        {
            messages = messages,
            /*model = "gpt-4"*/
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
            ResponseData responseData = JsonUtility.FromJson<ResponseData>(request.downloadHandler.text);

            return responseData.data;
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
public class ResponseData
{
    public string data;
    public string message;
    public int status;
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

[Serializable]
public class CurrentCondition
{
    
}





