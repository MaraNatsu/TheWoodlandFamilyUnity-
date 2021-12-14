using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Assets.SignalRModels;
using Assets.SignalRServices;
using Assets.SIgnalRServices;
using Assets.Enams;

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
    GameObject _endingScreen;
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
        _connector.OnGameStarted += StartGame;
        _connector.OnCardTyprDefined += ShowCardTaken;
        _connector.OnPlayerUpdated += UpdatePlayerViewList;
        _connector.OnMoveAllowed += AllowPlayerToMove;
        _connector.OnGameEnded += EndGame;

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
        _processor.InstantiateGameBoard(connectedPlayers, _currentPlayerView, _playerView, _gameScreen, _deck, _healthPoint, _waitingScreen);
    }

    private void AllowPlayerToMove()
    {
        _deck.interactable = true;
    }

    public async void TakeCard()
    {
        await _connector.SendMove(GameDataStorage.CurrentClient.PlayerId, GameDataStorage.CurrentClient.Wordkey);
        _deck.interactable = false;
    }

    private void ShowCardTaken(string cardType)
    {
        switch (cardType)
        {
            case nameof(CardType.Simple):
                _cardTaken = _simpleCard;
                break;
            case nameof(CardType.Life):
                _cardTaken = _lifeCard;
                break;
            case nameof(CardType.Trap):
                _cardTaken = _trapCard;
                break;
        }

        var instance = Instantiate(_cardTaken, _gameScreen.transform);
        Debug.Log("Card is shown: " + cardType);
        Destroy(instance, 2);
        Debug.Log("Card was destroyed");
    }

    private void UpdatePlayerViewList(PlayerOutputModel updatedPlayer)
    {
        _processor.UpdatePlayerViews(updatedPlayer);
    }

    private void EndGame(int winnerId)
    {
        _processor.ShowWinner(winnerId, _endingScreen, _gameScreen);
    }

    public async void QuitTheGame()
    {
        await _connector.CloseConnection();
        Application.Quit();
        Debug.Log("Close connection");
    }
}
