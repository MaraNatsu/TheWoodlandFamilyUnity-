using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Microsoft.AspNetCore.SignalR.Client;
using Assets.SignalRModels;
using Microsoft.AspNetCore.Http.Connections;

namespace Assets.SignalRServices
{
    class SignalRConnector
    {
        public static SignalRConnector instance = null;

        public Action<JoinedPlayerOutputModel> OnPlayerJoined;
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

        public async Task InitAsync(string serverUrl)
        {
            _connection = new HubConnectionBuilder()
                    .WithUrl(serverUrl, HttpTransportType.WebSockets)
                    .Build();

//            _connection.HandshakeTimeout = new TimeSpan(10000);


            //subscriber registration; subscriber: "JoinedPlayer", (joinedPlayer)
            _connection.On<JoinedPlayerOutputModel>("JoinedPlayer", (joinedPlayer) =>
            {
                OnPlayerJoined?.Invoke(new JoinedPlayerOutputModel
                {
                    RoomId = joinedPlayer.RoomId,
                    PlayerId = joinedPlayer.PlayerId,
                    HealthCount = joinedPlayer.HealthCount,
                    PlayerTurn = joinedPlayer.PlayerTurn,
                });
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

        public async Task JoinPlayerAsync(PlayerToJoinInputModel playerToJoin)
        {
            try
            {
                await _connection.InvokeAsync("SendJoinedPlayer", playerToJoin);

                Debug.Log("InvokeAsync");
            }
            catch (Exception ex)
            {
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
