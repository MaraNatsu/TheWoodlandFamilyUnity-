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
        public List<GameObject> ConnectedPlayers { get; private set; } = new List<GameObject>();
        private Color _colorConnected = new Color(130, 190, 30, 255);
        private Color _colorDisconnected = new Color(255, 105, 0, 150);

        public void FillHolder(GameObject newPlayer, int totalPlayerNumber)
        {
            newPlayer.SetActive(true);

            int i = 0;

            while (i < totalPlayerNumber)
            {
                ConnectedPlayers.Add(newPlayer);
            }
        }

        public void ConnectPlayer(GameObject newPlayer, string playerName)
        {
            newPlayer.GetComponentInChildren<Text>().text = playerName;
            newPlayer.GetComponent<Renderer>().material.SetColor("SaturatedLightGreen", _colorConnected);
        }

        public void DisconnectPlayer(string playerName)
        {
            foreach (var player in ConnectedPlayers)
            {
                if (ConnectedPlayers.FirstOrDefault(player => player.GetComponentInChildren<Text>().text == playerName))
                {
                    player.GetComponentInChildren<Text>().text = "";
                    player.GetComponent<Renderer>().material.SetColor("SaturatedRed", _colorDisconnected);
                }
            }
        }
    }
}
