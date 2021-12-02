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

            _connectedPlayers.ElementAt(0).Key.transform.localPosition = new Vector3(-45, 70);
            _connectedPlayers.ElementAt(1).Key.transform.localPosition = new Vector3(45, 70);

            if (totalPlayerNumber == 3)
            {
                _connectedPlayers.ElementAt(2).Key.transform.localPosition = new Vector3(0, 25);
            }

            if (totalPlayerNumber > 3)
            {
                _connectedPlayers.ElementAt(2).Key.transform.localPosition = new Vector3(-45, 25);
                _connectedPlayers.ElementAt(3).Key.transform.localPosition = new Vector3(45, 25);
            }

            if (totalPlayerNumber == 5)
            {
                _connectedPlayers.ElementAt(4).Key.transform.localPosition = new Vector3(0, -20);
            }
        }

        //public void ConnectPlayer(List<PlayerOutputModel> connectedPlayersReceived)
        //{
        //    for (int i = 0; i < connectedPlayersReceived.Count; i++)
        //    {
        //        PlayerOutputModel playerToConnect = connectedPlayers
        //            .Values
        //            .FirstOrDefault(value => value == connectedPlayersReceived[i]);

        //        if (playerToConnect == null)
        //        {
        //            var key = connectedPlayers.ElementAt(i).Key;
        //            connectedPlayers[key] = connectedPlayersReceived[i];
        //            key.GetComponentInChildren<Text>().text = connectedPlayersReceived[i].PlayerName;
        //            key.GetComponent<Image>().color = _colorConnected;
        //        }
        //    }
        //}
        public void UpdateConnections(List<PlayerOutputModel> connectedPlayers)
        {
            for (int i = 0; i < connectedPlayers.Count; i++)
            {
                var key = _connectedPlayers.ElementAt(i).Key;

                if (!connectedPlayers.Contains(_connectedPlayers[key]))
                {
                    ConnectPlayer(key, connectedPlayers[i]);
                    //_connectedPlayers[key] = connectedPlayers[i];
                    //key.GetComponentInChildren<Text>().text = connectedPlayers[i].PlayerName;
                    //key.GetComponent<Image>().color = _colorConnected;
                }
            }
        }

        private void ConnectPlayer(GameObject key, PlayerOutputModel connectedPlayer)
        {
            _connectedPlayers[key] = connectedPlayer;
            key.GetComponentInChildren<Text>().text = connectedPlayer.PlayerName;
            key.GetComponent<Image>().color = _colorConnected;

            //var key = _connectedPlayers.Where(player => player.Value.Id == 0).FirstOrDefault().Key;

            //if (key != null)
            //{
            //    _connectedPlayers[key] = connectedPlayer;
            //    key.GetComponentInChildren<Text>().text = connectedPlayer.PlayerName;
            //    key.GetComponent<Image>().color = _colorConnected;
            //}
        }

        public void DisconnectPlayer(int playerId)
        {
            PlayerOutputModel playerToDisconnect = _connectedPlayers
                .Values
                .FirstOrDefault(player => player.Id == playerId);

            var key = _connectedPlayers
                .FirstOrDefault(player => player.Value == playerToDisconnect)
                .Key;

            _connectedPlayers[key] = new PlayerOutputModel
            {
                Id = 0,
                PlayerName = ""
            };

            key.GetComponentInChildren<Text>().text = "";
            key.GetComponent<Renderer>().material.SetColor("SaturatedRed", _colorDisconnected);
        }
    }
}
