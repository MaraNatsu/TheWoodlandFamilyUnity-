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

        public Action<PlayerOutputModel> OnPlayerJoined;
        public Action<string, int> OnNewPlayerGot;
        public Action OnAllPlayersConnected;

        private HubConnection _connection;
        private ConnectedPlayersHolder _holder = new ConnectedPlayersHolder();

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
                    .WithUrl($"http://localhost:5000/gamehub?playerId={GameDataStorage.CurrentPlayer.PlayerId}", HttpTransportType.WebSockets)
                    .WithAutomaticReconnect()
                    .Build();

            //subscriber registration; subscriber: "JoinedPlayer", (joinedPlayer)
            _connection.On<PlayerOutputModel>("JoinedPlayer", (joinedPlayer) =>
            {
                OnPlayerJoined?.Invoke(new PlayerOutputModel
                {
                    RoomId = joinedPlayer.RoomId,
                    PlayerId = joinedPlayer.PlayerId,
                    PlayerName = joinedPlayer.PlayerName,
                    HealthCount = joinedPlayer.HealthCount,
                    PlayerTurn = joinedPlayer.PlayerTurn,
                });
            });

            //subscriber registration; subscriber: "GetNewPlayer", ()
            _connection.On<string, int>("GetNewPlayer", (newPlayerName, totalPlayerNumber) =>
            {
                //OnNewPlayerGot?.Invoke(_holder.FillHolder(newPlayerName, totalPlayerNumber));
            });

            _connection.On<int>("DisconnectPlayer", (playerId) =>
            { 

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

                Debug.Log("InvokeAsync \"SendJoinedPlayer\"");
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
