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
using Assets.Enams;
using System.Threading;

public class WebSocketHandlerScript : MonoBehaviour
{
    //player connection objects
    [SerializeField]
    private GameObject _newPlayer;
    [SerializeField]
    private GameObject _waitingScreen;

    //game process objects
    [SerializeField]
    private GameObject _currentPlayerView;
    [SerializeField]
    private GameObject _playerView;
    [SerializeField]
    private GameObject _gameScreen;
    [SerializeField]
    private Button _deck;

    [SerializeField]
    GameObject _winnerText;
    [SerializeField]
    GameObject _healthPoint;

    [SerializeField]
    private GameObject _simpleCard;
    [SerializeField]
    private GameObject _lifeCard;
    [SerializeField]
    private GameObject _trapCard;
    private GameObject _cardTaken;

    private SignalRConnector _connector;
    private ConnectedPlayersHolder _holder = new ConnectedPlayersHolder();
    private GameProcessor _processor = new GameProcessor();

    async Task Start()
    {
        _holder.FillHolder(_newPlayer, _waitingScreen, GameDataStorage.CurrentClient.PlayerNumber);

        _connector = SignalRConnector.GetInstance();
        _connector.OnConnectionStarted += UpdateConnectionViews;
        _connector.OnPlayerDisconnected += RemoveDisconnectedPlayer;
        _connector.OnPlayersConnected += StartGame;
        _connector.OnCardTyprDefined += ShowCardTaken;
        _connector.OnPlayerUpdated += UpdatePlayerViewList;
        _connector.OnPlayersConnected += MakeMove;
        _connector.OnPlayersConnected += EndGame;

        await _connector.InitAsync();
    }

    private void UpdateConnectionViews(List<PlayerOutputModel> connectedPlayers)
    {
        _holder.UpdateConnections(connectedPlayers);
        Debug.Log("Connected players are displayed:" + connectedPlayers.Count);
    }

    private void RemoveDisconnectedPlayer(int playerId)
    {
        _holder.DisconnectPlayer(playerId);
        Debug.Log("Disconnected: " + playerId);
    }

    private void StartGame(int firstPlayerId)
    {
        var connectedPlayers = _holder.GetConnectedPlayers();

        Thread.Sleep(3000);
        _processor.InstantiateGameBoard(connectedPlayers, _currentPlayerView, _playerView, _gameScreen, _deck, _healthPoint, _waitingScreen);
        MakeMove(firstPlayerId);
    }

    private void ShowCardTaken(string cardType)
    {
        GameObject instance = null;

        switch (cardType)
        {
            case nameof(CardType.Simple):
                instance = Instantiate(_simpleCard, _waitingScreen.transform);
                break;
            case nameof(CardType.Life):
                instance = Instantiate(_lifeCard, _waitingScreen.transform);
                break;
            case nameof(CardType.Trap):
                instance = Instantiate(_trapCard, _waitingScreen.transform);
                break;
        }

        _cardTaken = instance;

        Thread.Sleep(3000);
        Destroy(_cardTaken);
    }

    private void UpdatePlayerViewList(PlayerOutputModel updatedPlayer)
    {
        _processor.UpdatePlayerViews(updatedPlayer, _healthPoint, _gameScreen);
    }

    private void MakeMove(int nextPlayerId)
    {
        _processor.TakeCard(nextPlayerId, _deck);

        _deck.onClick.AddListener(async () =>
        {
            await _connector.SendMove(nextPlayerId, GameDataStorage.CurrentClient.Wordkey);
        });
    }

    private void EndGame(int winnerId)
    {
        _processor.ShowWinner(winnerId, _winnerText, _gameScreen);
    }

    public async void QuitTheGame()
    {
        await _connector.CloseConnection();
        Application.Quit();
        Debug.Log("Close connection");
    }
}
