using System;
using LeastSquares.Spark;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PetAIDialog : MonoBehaviour
{
    public float writeSpeed = 0.04f;

    /// <summary>
    /// The name of the character that the AI or NPC is acting as.
    /// </summary>
    public string characterName;

    /// <summary>
    /// The role that the AI or NPC is playing.
    /// </summary>
    public string actAs;

    /// <summary>
    /// An array of things that the AI or NPC can mention during the conversation.
    /// </summary>
    public string[] thingsToMention;

    /// <summary>
    /// The text object where the dialogue will be displayed.
    /// </summary>
    public TMP_Text dialogueText;

    public List<_ChatCompletionMessage> _messages = new List<_ChatCompletionMessage>();

    public ChatAIController chatAIController;
    public PetAICommand petAICommand;

    /// <summary>
    /// Initiates a conversation with the AI or NPC character.
    /// </summary>
    /// <param name="prompt">The initial prompt or message from the player.</param>
    /// 
    public void Start()
    {
        _messages.Add(CreateStartingPrompt());
        petAICommand = GameObject.FindObjectOfType<PetAICommand>();
    }

    public async void Talk(string prompt)
    {
        //if (!ChatEngine._loaded) return;

        _messages.Add(new _ChatCompletionMessage
        {
            role = "user",
            content = prompt
        });

        SetText("생각하는 척 하는중...");

        //var result = await OpenAIAccessManager.RequestChatCompletion(_messages.ToArray());
        var result = await ChatServerManager.ChatPostRequest(_messages);

        _messages.Add(new _ChatCompletionMessage
        {
            role = "assistant",
            content = result
        });
        _ChatMessageResult messageResult = DivideChatMessage(result);
        SetText(messageResult.message);
        petAICommand.PlayCommand(messageResult);
        
    }

    private IEnumerator WriteText(string textToWrite)
    {
        var total = string.Empty;
        for (var i = 0; i < textToWrite.Length; ++i)
        {
            total += textToWrite[i];
            dialogueText.text = total;
            yield return new WaitForSeconds(writeSpeed);
        }
        EndText();
    }
    protected void SetText(string text)
    {
        StartCoroutine(WriteText(text));
    }
    protected void EndText()
    {
        chatAIController.EndTalk();
    }

    /// <summary>
    /// Creates the starting prompt for the AI or NPC character.
    /// </summary>
    /// <returns>A ChatCompletionMessage object representing the starting prompt.</returns>
    /// 

    public _ChatCompletionMessage CreateStartingPrompt()
    {
        var prompt = "";//"You are acting as an AI or NPC inside a game, a player might talk to you and you will have a pleasant cddonversation. The following are the instructions for your character:\n";
        prompt += characterName != null ? $"Your name is {characterName}. " : "";
        prompt += actAs != null ? $"You are a {actAs}." : "";
        prompt += thingsToMention != null ? $"Try to mention this things during your conversations:\n{string.Join("\n", thingsToMention)} " : "";
        prompt += "Do not break character. Be creative";
        prompt += "Do not talk excessively. Instead encourage the player to ask questions";
        return new _ChatCompletionMessage
        {
            role = "system",
            content = prompt
        };

    }
    _ChatMessageResult DivideChatMessage(string input)
    {
        _ChatMessageResult result = new _ChatMessageResult();

        // '/' 문자로 문자열을 분할
        string[] dividedStrings = input.Split('/');

        if (dividedStrings.Length >= 2)
        {
            // '/' 다음에 오는 문자열을 저장
            result.command = dividedStrings[1];

            // 괄호 사이에 오는 문자열 추출
            int startIndex = result.command.IndexOf('(');
            int endIndex = result.command.IndexOf(')');
            if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
            {
                result.command = result.command.Substring(startIndex + 1, endIndex - startIndex - 1);
            }

            // command 문자열에서 숫자 추출
            string numString = "";
            for (int i = 0; i < result.command.Length; i++)
            {
                if (char.IsDigit(result.command[i]))
                    numString += result.command[i];
                else
                    break;
            }

            if (!string.IsNullOrEmpty(numString))
            {
                // 숫자를 정수로 변환하여 저장
                result.commandNum = int.Parse(numString);
            }
            else
            {
                // 숫자가 없는 경우 0으로 설정
                result.commandNum = 0;
            }

            // '/' 다음에 오는 문자열을 제거하고 나머지 문자열을 합침
            result.message = string.Join("", dividedStrings, 2, dividedStrings.Length - 2);
        }
        else
        {
            // '/'가 없는 경우 모든 값을 초기화
            result.message = input;
            result.command = "";
            result.commandNum = 0;
        }

        return result;
    }

}
public class _ChatMessageResult
{
    public string message;
    public string command;
    public int commandNum;

}


