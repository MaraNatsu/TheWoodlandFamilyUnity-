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
using System.Linq;

public class WebSocketHandlerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject _newPlayer;
    [SerializeField]
    private GameObject _waitingScreen;

    private SignalRConnector _connector;
    private ConnectedPlayersHolder _holder = new ConnectedPlayersHolder();

    async Task Start()
    {
        _holder.FillHolder(_newPlayer, _waitingScreen, GameDataStorage.CurrentClient.PlayerNumber);

        //foreach (var connectedPlayer in _holder.ConnectedPlayersView)
        //{
        //    Instantiate(connectedPlayer, _waitingScreen.transform);
        //}

        _connector = SignalRConnector.GetInstance();
        //_connector.OnPlayerConnected += ShowConnectedPlayers;
        _connector.OnConnectionStarted += GetConnectedPlayers;
       // _connector.OnPlayerConnected += AddConnectedPlayer;
        _connector.OnPlayerDisconnected += RemoveDisconnectedPlayer;
        _connector.OnAllPlayersConnected += LoadGameScene;

        await _connector.InitAsync();
        //SendConnectingPlayer();
    }

    //private async void SendConnectingPlayer()
    //{
    //    await _connector.ConnectPlayerAsync(new PlayerToConnectInputModel
    //    {
    //        Id = GameDataStorage.CurrentClient.PlayerId,
    //        Name = GameDataStorage.CurrentClient.PlayerName,
    //        Wordkey = GameDataStorage.CurrentClient.Wordkey,
    //    });
    //}

    //private void ShowConnectedPlayers(List<PlayerOutputModel> connectedPlayers)
    //{
    //    _holder.ConnectPlayer(connectedPlayers);
    //    Debug.Log("Displayed all connected players");
    //}

    private void GetConnectedPlayers(List<PlayerOutputModel> connectedPlayers)
    {
        _holder.UpdateConnections(connectedPlayers);
        Debug.Log("All connected players are displayed.");
    }

    //private void AddConnectedPlayer(PlayerOutputModel connectedPlayer)
    //{
    //    _holder.ConnectPlayer(connectedPlayer);
    //    Debug.Log("Connected: " + connectedPlayer.PlayerName);
    //}

    private void RemoveDisconnectedPlayer(int playerId)
    {
        _holder.DisconnectPlayer(playerId);
        Debug.Log("Disconnected: " + playerId);
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
