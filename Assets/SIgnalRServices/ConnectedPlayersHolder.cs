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
        public List<GameObject> ConnectedPlayersView { get; private set; } = new List<GameObject>();
        public List<PlayerOutputModel> ConnectedPlayers { get; } = new List<PlayerOutputModel>();
        private Color _colorConnected = new Color(130, 190, 30, 255);
        private Color _colorDisconnected = new Color(255, 105, 0, 150);

        public void FillHolder(GameObject newPlayer, int totalPlayerNumber)
        {
            //newPlayer.SetActive(true);

            int i = 0;

            while (i < totalPlayerNumber)
            {
                ConnectedPlayersView.Add(newPlayer);
                i++;
            }

            ConnectedPlayersView[0].transform.localPosition = new Vector3(-45, 70);
            ConnectedPlayersView[1].transform.localPosition = new Vector3(45, 70);

            if (totalPlayerNumber == 3)
            {
                ConnectedPlayersView[2].transform.localPosition = new Vector3(0, 25);
            }

            if (totalPlayerNumber > 3)
            {
                ConnectedPlayersView[2].transform.localPosition = new Vector3(-45, 25);
                ConnectedPlayersView[3].transform.localPosition = new Vector3(45, 25);
            }

            if (totalPlayerNumber == 5)
            {
                ConnectedPlayersView[4].transform.localPosition = new Vector3(0, -20);
            }
        }

        public void ConnectPlayer(PlayerOutputModel player)
        {
            ConnectedPlayers.Add(player);

            GameObject newPlayer = ConnectedPlayersView
                .Where(player => player.GetComponentInChildren<Text>().text == "")
                .FirstOrDefault();
            newPlayer.GetComponentInChildren<Text>().text = player.PlayerName;
            newPlayer.GetComponent<Renderer>().material.SetColor("SaturatedLightGreen", _colorConnected);
        }

        public void DisconnectPlayer(PlayerOutputModel playerToDisconnect)
        {
            ConnectedPlayers.Remove(playerToDisconnect);

            foreach (var connectedPlayer in ConnectedPlayersView)
            {
                if (ConnectedPlayersView
                    .FirstOrDefault(player => player.GetComponentInChildren<Text>().text == playerToDisconnect.PlayerName))
                {
                    connectedPlayer.GetComponentInChildren<Text>().text = "";
                    connectedPlayer.GetComponent<Renderer>().material.SetColor("SaturatedRed", _colorDisconnected);
                }
            }
        }
    }
}
