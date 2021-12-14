using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Microsoft.AspNetCore.SignalR.Client;
using Assets.SignalRModels;
using Microsoft.AspNetCore.Http.Connections;
using Assets.SIgnalRServices;

namespace Assets.SignalRServices
{
    class SignalRConnector
    {
        public static SignalRConnector instance = null;
        private string _serverUrl = "http://localhost:5000/gamehub";

        public Action<List<PlayerOutputModel>> OnConnectionStarted;
        public Action<int> OnPlayerDisconnected;
        public Action<int> OnGameStarted;
        public Action OnMoveAllowed;
        public Action<string> OnCardTyprDefined;
        public Action<PlayerOutputModel> OnPlayerUpdated;
        public Action<int> OnGameEnded;

        private HubConnection _connection;

        private SignalRConnector()
        {

        }

        public static SignalRConnector GetInstance()
        {
            if (instance == null)
            {
                instance = new SignalRConnector();
                return instance;
            }

            return instance;
        }

        public async Task InitAsync()
        {
            _connection = new HubConnectionBuilder()
                    .WithUrl($"http://localhost:5000/gamehub?playerId={GameDataStorage.CurrentClient.PlayerId}", HttpTransportType.LongPolling)
                    .WithAutomaticReconnect()
                    .Build();

            _connection.Reconnecting += error =>
            {
                Debug.Assert(_connection.State == HubConnectionState.Reconnecting);
                Debug.Log("Reconnecting: " + error.Message);

                return Task.CompletedTask;
            };

            //subscriber registration; subscriber: "GetConnectedPlayers", (connectedPlayer)
            _connection.On<List<PlayerOutputModel>>("GetConnectedPlayers", (connectedPlayers) =>
            {
                OnConnectionStarted?.Invoke(connectedPlayers);
            });

            _connection.On<int>("RemoveDisconnectedPlayer", (playerId) =>
            {
                OnPlayerDisconnected?.Invoke(playerId);
            });

            _connection.On<int>("StartGame", (firstPlayerId) =>
            {
                OnGameStarted?.Invoke(firstPlayerId);
            });

            _connection.On<string>("ShowCardTaken", (cardType) =>
            {
                OnCardTyprDefined?.Invoke(cardType);
            });

            _connection.On<PlayerOutputModel>("UpdatePlayerData", (updatedPlayer) =>
            {
                OnPlayerUpdated?.Invoke(updatedPlayer);
            });

            _connection.On("SetPlayerToMove", () =>
            {
                OnMoveAllowed?.Invoke();
            });

            _connection.On<int>("FinishGame", (winnerId) =>
            {
                OnGameEnded?.Invoke(winnerId);
            });

            _connection.Closed += Disconnect;

            await StartConnectionAsync();
            Debug.Log("Start connection");
        }

        public async Task Disconnect(Exception ex)
        {
            Debug.Log("Disconnected: " + ex);
        }

        private async Task StartConnectionAsync()
        {
            try
            {
                await _connection.StartAsync();

                Debug.Log("connection StartAsync");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error {ex.Message}");
                Debug.LogError($"Error {ex.StackTrace}");
            }
        }

        public async Task SendMove(int playerId, string wordkey)
        {
            try
            {
                await _connection.InvokeAsync("ProcessMove", playerId, wordkey);
                Debug.Log("InvokeAsync \"ProcessMove\"");
            }
            catch (Exception ex)
            {
                Debug.Log("DefineCardType: " + ex.Message);
                Debug.LogError($"Error {ex.Message}");
            }
        }

        public async Task CloseConnection()
        {
            await _connection.StopAsync();
        }
    }
}
