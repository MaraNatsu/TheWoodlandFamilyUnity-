using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Services
{
    class ConnectedPlayersHolder
    {
        private Dictionary<GameObject, PlayerOutputModel> _connectedPlayers = new Dictionary<GameObject, PlayerOutputModel>();
        private Color _colorConnected = new Color(130, 190, 30, 255);
        private Color _colorDisconnected = new Color(255, 105, 0, 150);

        public void FillHolder(GameObject newPlayer, GameObject waitingScreen, int totalPlayerNumber)
        {
            int i = 0;

            while (i < totalPlayerNumber)
            {
                var instance = UnityEngine.Object.Instantiate(newPlayer, waitingScreen.transform);
                _connectedPlayers.Add(instance, new PlayerOutputModel());
                i++;
            }

            _connectedPlayers.ElementAt(0).Key.transform.localPosition = new Vector3(-45, 40);
            _connectedPlayers.ElementAt(1).Key.transform.localPosition = new Vector3(45, 40);

            if (totalPlayerNumber == 3)
            {
                _connectedPlayers.ElementAt(2).Key.transform.localPosition = new Vector3(0, -5);
            }

            if (totalPlayerNumber > 3)
            {
                _connectedPlayers.ElementAt(2).Key.transform.localPosition = new Vector3(-45, -5);
                _connectedPlayers.ElementAt(3).Key.transform.localPosition = new Vector3(45, -5);
            }

            if (totalPlayerNumber == 5)
            {
                _connectedPlayers.ElementAt(4).Key.transform.localPosition = new Vector3(0, -50);
            }
        }

        public void UpdateConnections(List<PlayerOutputModel> connectedPlayers)
        {
            for (int i = 0; i < connectedPlayers.Count; i++)
            {
                var key = _connectedPlayers.ElementAt(i).Key;
                _connectedPlayers[key] = connectedPlayers[i];
                key.GetComponentInChildren<Text>().text = connectedPlayers[i].PlayerName;
                //key.GetComponent<Renderer>().material.SetColor("SaturatedGreen", _colorConnected);
            }
        }

        private void ConnectPlayer(GameObject key, PlayerOutputModel connectedPlayer)
        {
            _connectedPlayers[key] = connectedPlayer;
            key.GetComponentInChildren<Text>().text = connectedPlayer.PlayerName;
            key.GetComponent<Renderer>().material.SetColor("SaturatedGreen", _colorConnected);
        }

        public void DisconnectPlayer(int playerId)
        {
            PlayerOutputModel playerToDisconnect = _connectedPlayers
                .Values
                .FirstOrDefault(player => player.Id == playerId);

            var key = _connectedPlayers
                .FirstOrDefault(player => player.Value == playerToDisconnect)
                .Key;

            _connectedPlayers[key] = new PlayerOutputModel();

            key.GetComponentInChildren<Text>().text = "";
            key.GetComponent<Renderer>().material.SetColor("SaturatedRed", _colorDisconnected);
        }

        public List<PlayerOutputModel> GetConnectedPlayers()
        {
            List<PlayerOutputModel> connectedPlayers = new List<PlayerOutputModel>();

            foreach (var player in _connectedPlayers)
            {
                connectedPlayers.Add(player.Value);
            }

            return connectedPlayers;
        }
    }
}
