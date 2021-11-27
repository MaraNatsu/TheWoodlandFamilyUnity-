using Assets.SignalRModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.SIgnalRServices
{
    class ConnectedPlayersHolder
    {
        //public List<GameObject> ConnectedPlayersView { get; private set; } = new List<GameObject>();
        //public List<PlayerOutputModel> ConnectedPlayers { get; } = new List<PlayerOutputModel>();
        private Dictionary<GameObject, PlayerOutputModel> connectedPlayers = new Dictionary<GameObject, PlayerOutputModel>();
        private Color _colorConnected = new Color(130, 190, 30, 255);
        private Color _colorDisconnected = new Color(255, 105, 0, 150);

        public void FillHolder(GameObject newPlayer, GameObject waitingScreen, int totalPlayerNumber)
        {
            int i = 0;

            while (i < totalPlayerNumber)
            {
                var instance = UnityEngine.Object.Instantiate(newPlayer, waitingScreen.transform);
                connectedPlayers.Add(instance, new PlayerOutputModel());
                i++;
            }

            connectedPlayers.ElementAt(0).Key.transform.localPosition = new Vector3(-45, 70);
            connectedPlayers.ElementAt(1).Key.transform.localPosition = new Vector3(45, 70);

            if (totalPlayerNumber == 3)
            {
                connectedPlayers.ElementAt(2).Key.transform.localPosition = new Vector3(0, 25);
            }

            if (totalPlayerNumber > 3)
            {
                connectedPlayers.ElementAt(2).Key.transform.localPosition = new Vector3(-45, 25);
                connectedPlayers.ElementAt(3).Key.transform.localPosition = new Vector3(45, 25);
            }

            if (totalPlayerNumber == 5)
            {
                connectedPlayers.ElementAt(4).Key.transform.localPosition = new Vector3(0, -20);
            }
        }

        public void ConnectPlayer(List<PlayerOutputModel> connectedPlayersReceived)
        {
            for (int i = 0; i < connectedPlayersReceived.Count; i++)
            {
                PlayerOutputModel playerToConnect = connectedPlayers
                    .Values
                    .FirstOrDefault(value => value == connectedPlayersReceived[i]);

                if (playerToConnect == null)
                {
                    var key = connectedPlayers.ElementAt(i).Key;
                    connectedPlayers[key] = connectedPlayersReceived[i];
                    key.GetComponentInChildren<Text>().text = connectedPlayersReceived[i].PlayerName;
                    key.GetComponent<Image>().color = _colorConnected;
                }
            }
        }

        public void DisconnectPlayer(int playerId)
        {
            PlayerOutputModel playerToDisconnect = connectedPlayers
                .Values
                .FirstOrDefault(player => player.Id == playerId);

            var key = connectedPlayers
                .FirstOrDefault(player => player.Value == playerToDisconnect)
                .Key;

            connectedPlayers[key] = null;
            key.GetComponentInChildren<Text>().text = "";
            key.GetComponent<Renderer>().material.SetColor("SaturatedRed", _colorDisconnected);
        }
    }
}
