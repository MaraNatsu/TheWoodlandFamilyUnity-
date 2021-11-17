using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.AspNetCore.SignalR.Client;
using Assets.SignalRModels;
using Assets.SignalRServices;
using UnityEngine.SceneManagement;

public class WebSocketHandlerScript : MonoBehaviour
{
    //[SerializeField]
    //private Text _receivedText;
    //[SerializeField]
    //private InputField _messageInput;
    //[SerializeField]
    //private Button _sendButton;

    [SerializeField]
    private InputField _playerName;
    [SerializeField]
    private InputField _wordKeyOnCreation;
    [SerializeField]
    private InputField _wordKeyOnJoining;

    private string _wordKey;
    private SignalRConnector _connector;
    private string _serverUrl = "http://localhost:5000/chathub";

    //public async void Start()
    //{
    //    await StartConnection();

    //    Debug.Log("StartConnection()");
    //}

    async Task Start()
    {
        _connector = SignalRConnector.GetInstance();
        _connector.OnPlayerJoined += SavePlayerData;
        _connector.OnAllPlayersConnected += LoadGameScene;
        //_connector.OnMessageReceived += UpdateReceivedMessages;

        await _connector.InitAsync(_serverUrl);
        //_sendButton.onClick.AddListener(SendMessage);
    }

    //private void UpdateReceivedMessages(SignalRMessage newMessage)
    //{
    //    var lastMessages = _receivedText.text;

    //    if (string.IsNullOrEmpty(lastMessages) == false)
    //    {
    //        lastMessages += "\n";
    //    }

    //    lastMessages += $"User: {newMessage.PlayerName}, Message:{newMessage.WordKey}";
    //    _receivedText.text = lastMessages;
    //}

    //private async void SendMessage()
    //{
    //    await _connector.SendMessageAsync(new SignalRMessage
    //    {
    //        PlayerName = "Example",
    //        WordKey = _messageInput.text,
    //    });
    //}

    public async void SendJoiningPlayer()
    {
        if (_wordKeyOnCreation.text != "")
        {
            _wordKey = _wordKeyOnCreation.text;
        }
        else
        {
            _wordKey = _wordKeyOnJoining.text;
        }

        await _connector.JoinPlayerAsync(new PlayerToJoinInputModel
        {
            PlayerName = _playerName.text,
            WordKey = _wordKey,
        });
    }

    private void SavePlayerData(JoinedPlayerOutputModel joinedPlayer)
    {

    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
