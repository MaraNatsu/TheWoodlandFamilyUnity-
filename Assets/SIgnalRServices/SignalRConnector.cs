using System;
using System.Collections;
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

        public Action<List<PlayerOutputModel>> OnPlayerConnected;
        public Action<int> OnPlayerDisconnected;
        public Action OnAllPlayersConnected;

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
                    .WithUrl($"http://localhost:5000/gamehub?playerId={GameDataStorage.CurrentClient.PlayerId}", HttpTransportType.WebSockets)
                    .WithAutomaticReconnect()
                    .Build();

            //subscriber registration; subscriber: "ShowConnectedPlayer", (connectedPlayer)
            _connection.On<List<PlayerOutputModel>>("ShowConnectedPlayers", (connectedPlayers) =>
            {
                OnPlayerConnected?.Invoke(connectedPlayers);
            });

            _connection.On<int>("DisconnectPlayer", (playerId) =>
            {
                OnPlayerDisconnected?.Invoke(playerId);
            });

            //subscriber registration; subscriber: "StartGame", ()
            _connection.On("StartGame", () =>
            {
                OnAllPlayersConnected?.Invoke();
            });

            _connection.Closed += Disconnect;

            await StartConnectionAsync();
            Debug.Log("Start connection");
        }

        public async Task Disconnect(Exception ex)
        {
            Debug.Log("Disconnected: " + ex);
        }

        public async Task ConnectPlayerAsync(PlayerToConnectInputModel playerToConnect)
        {
            try
            {
                await _connection.InvokeAsync("SendConnectedPlayers", playerToConnect);

                Debug.Log("InvokeAsync \"SendConnectedPlayers\"");
            }
            catch (Exception ex)
            {
                Debug.Log("SendConnectedPlayer Error: " + ex.Message);
                Debug.LogError($"Error {ex.Message}");
            }
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
    }
}
