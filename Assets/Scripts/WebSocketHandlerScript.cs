using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.AspNetCore.SignalR.Client;
using Assets.SignalRModels;

public class WebSocketHandlerScript : MonoBehaviour
{
    [SerializeField]
    private Text _receivedText;
    [SerializeField]
    private InputField _messageInput;
    [SerializeField]
    private Button _sendButton;
    private SignalRConnector connector;

    private string _serverUrl = "http://localhost:5000/chathub";

    public async Task Start()
    {
        connector = new SignalRConnector();
        connector.OnMessageReceived += UpdateReceivedMessages;

        await connector.InitAsync(_serverUrl);
        _sendButton.onClick.AddListener(SendMessage);
    }

    private void UpdateReceivedMessages(SignalRMessage newMessage)
    {
        var lastMessages = _receivedText.text;

        if (string.IsNullOrEmpty(lastMessages) == false)
        {
            lastMessages += "\n";
        }
         
        lastMessages += $"User: {newMessage.PlayerName}, Message:{newMessage.Text}";
        _receivedText.text = lastMessages;
    }

    private async void SendMessage()
    {
        await connector.SendMessageAsync(new SignalRMessage
        {
            PlayerName = "Example",
            Text = _messageInput.text,
        });
    }
}
