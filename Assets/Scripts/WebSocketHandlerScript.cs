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
        _holder.FillHolder(_newPlayer, GameDataStorage.CurrentClient.PlayerNumber);

        foreach (var connectedPlayer in _holder.ConnectedPlayersView)
        {
            Instantiate(connectedPlayer, _waitingScreen.transform);
        }

        _connector = SignalRConnector.GetInstance();
        _connector.OnPlayerConnected += ShowConnectedPlayer;
        _connector.OnPlayerDisconnected += DisconnectPlayer;
        _connector.OnAllPlayersConnected += LoadGameScene;

        await _connector.InitAsync();
        SendConnectingPlayer();
    }

    private async void SendConnectingPlayer()
    {
        await _connector.ConnectPlayerAsync(new PlayerToConnectInputModel
        {
            Id = GameDataStorage.CurrentClient.PlayerId,
            Name = GameDataStorage.CurrentClient.PlayerName,
            Wordkey = GameDataStorage.CurrentClient.Wordkey,
        });
    }

    private void ShowConnectedPlayer(PlayerOutputModel connectedPlayer)
    {
        _holder.ConnectPlayer(connectedPlayer);
        Debug.Log("Displayed: " + connectedPlayer.PlayerName + ", " + connectedPlayer.Id);
    }

    private void DisconnectPlayer(int playerId)
    {
        PlayerOutputModel playerToDisconnect = _holder
            .ConnectedPlayers
            .Where(player => player.Id == playerId)
            .FirstOrDefault();

        _holder.DisconnectPlayer(playerToDisconnect);

        Debug.Log("Disconnected: " + playerId);
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
