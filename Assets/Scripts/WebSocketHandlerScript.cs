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
using Assets.SIgnalRServices;

public class WebSocketHandlerScript : MonoBehaviour
{
    [SerializeField]
    private InputField _playerName;
    [SerializeField]
    private InputField _wordKeyOnCreation;
    [SerializeField]
    private InputField _wordKeyOnJoining;
    [SerializeField]
    private GameObject _newPlayer;

    private SignalRConnector _connector;
    private ConnectedPlayersHolder _holder = new ConnectedPlayersHolder();

    private string _wordKey;

    async Task Start()
    {
        _holder.FillHolder(_newPlayer, GameDataStorage.CurrentClient.PlayerNumber);
        _connector = SignalRConnector.GetInstance();
        SendConnectingPlayer();

        _connector.OnPlayerConnected += ShowConnectedPlayer;
        _connector.OnAllPlayersConnected += LoadGameScene;
        //_connector.OnMessageReceived += UpdateReceivedMessages;

        await _connector.InitAsync();
        //_sendButton.onClick.AddListener(SendMessage);
    }

    private async void SendConnectingPlayer()
    {
        if (_wordKeyOnCreation.text != "")
        {
            _wordKey = _wordKeyOnCreation.text;
        }
        else
        {
            _wordKey = _wordKeyOnJoining.text;
        }

        await _connector.ConnectPlayerAsync(new PlayerToConnectInputModel
        {
            Id = GameDataStorage.CurrentClient.PlayerId,
            Name = GameDataStorage.CurrentClient.PlayerName,
            Wordkey = _wordKey,
        });
    }

    private void ShowNewPlayer(string playerName, int playerNumber)
    {

    }

    private void DisconnectPlayer(int playerId)
    {
         

        Debug.Log("Disconnected: " + playerId);
    }

    private void ShowConnectedPlayer(PlayerOutputModel connectedPlayer)
    {
        
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
