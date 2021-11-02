using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Microsoft.AspNetCore.SignalR.Client;

namespace Assets.SignalRModels
{
    class SignalRConnector
    {
        public Action<SignalRMessage> OnMessageReceived;

        private HubConnection _connection;

        public async Task InitAsync(string serverUrl)
        {
            _connection = new HubConnectionBuilder()
                    .WithUrl(serverUrl)
                    .Build();

            _connection.On<string, string>("ReceiveMessage", (player, message) =>
            {
                OnMessageReceived?.Invoke(new SignalRMessage
                {
                    PlayerName = player,
                    WordKey = message,
                });
            });
            await StartConnectionAsync();

            Debug.Log("Start connection");
        }

        public async Task SendMessageAsync(SignalRMessage message)
        {
            try
            {
                await _connection.InvokeAsync("SendMessage",
                    message.PlayerName, message.WordKey);
                 
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
            }
        }
    }
}
