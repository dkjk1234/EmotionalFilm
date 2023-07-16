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

    /// <summary>
    /// Initiates a conversation with the AI or NPC character.
    /// </summary>
    /// <param name="prompt">The initial prompt or message from the player.</param>
    /// 
    public void Start()
    {
        _messages.Add(CreateStartingPrompt());
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
        SetText(result);
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
        var prompt = "You are acting as an AI or NPC inside a game, a player might talk to you and you will have a pleasant cddonversation. The following are the instructions for your character:\n";
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
}
