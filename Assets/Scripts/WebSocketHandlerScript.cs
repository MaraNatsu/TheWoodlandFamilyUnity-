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
    [SerializeField]
    private InputField _playerName;
    [SerializeField]
    private InputField _wordKeyOnCreation;
    [SerializeField]
    private InputField _wordKeyOnJoining;
    [SerializeField]
    private GameObject _newPlayerName;

    private SignalRConnector _connector;

    private string _wordKey;
    private PlayerOutputModel joinedPlayer;

    async Task Start()
    {
        _connector = SignalRConnector.GetInstance();
        _connector.OnPlayerJoined += SavePlayerData;
        _connector.OnNewPlayerGot += ShowNewPlayer;
        _connector.OnAllPlayersConnected += LoadGameScene;
        //_connector.OnMessageReceived += UpdateReceivedMessages;

        await _connector.InitAsync();
        //_sendButton.onClick.AddListener(SendMessage);
    }

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

    private void ShowNewPlayer(string playerName, int playerNumber)
    {

    }

    private void DisconnectPlayer(int playerId)
    {

    }

    private void SavePlayerData(PlayerOutputModel joinedPlayer)
    {
        Debug.Log("Recive server response for one client.");
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
