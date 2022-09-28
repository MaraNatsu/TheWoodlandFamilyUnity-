using Assets.Scripts.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Services
{
    class GameProcessor
    {
        private Dictionary<GameObject, PlayerOutputModel> _playerViews = new Dictionary<GameObject, PlayerOutputModel>();
        //private Color _colorActive = new Color(130, 190, 30, 255);
        private Color _colorWaiting = new Color(255, 150, 0, 2500);
        private GameObject _healthPoint;

        public void InstantiateGameBoard(
            List<PlayerOutputModel> connectedPlayers,
            GameObject currentPlayerView,
            GameObject playerView,
            GameObject gameScreen,
            Button deck,
            GameObject healthPoint,
            GameObject waitingScreen)
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            waitingScreen.SetActive(false);
            gameScreen.SetActive(true);
            deck.interactable = false;

            var currentPlayer = new PlayerOutputModel
            {
                //Id = GameDataStorage.CurrentClient.Id,
                //PlayerName = GameDataStorage.CurrentClient.Name,
                //Turn = GameDataStorage.CurrentClient.Turn,
                //HealthCount = GameDataStorage.CurrentClient.HealthCount
            };

            connectedPlayers = SetInstantiationTurn(connectedPlayers, currentPlayer);

            foreach (var player in connectedPlayers)
            {
                GameObject instance;

                if (player.Id == currentPlayer.Id)
                {
                    instance = Object.Instantiate(currentPlayerView, gameScreen.transform);
                    _healthPoint = Object.Instantiate(healthPoint, gameScreen.transform);
                    _healthPoint.GetComponentInChildren<Text>().text = player.HealthCount.ToString();
                }
                else
                {
                    instance = Object.Instantiate(playerView, gameScreen.transform);
                }

                instance.GetComponentInChildren<Text>().text = player.PlayerName.ToUpper();
                //instance.GetComponentInChildren<Renderer>().material.SetColor("SaturatedYellow", _colorWaiting);

                _playerViews.Add(instance, player);
            }

            //switch (GameDataStorage.CurrentClient.PlayerNumber)
            //{
            //    case 2:
            //        _playerViews.ElementAt(1).Key.GetComponentInChildren<RectTransform>().sizeDelta = new Vector3(300, 200);
            //        _playerViews.ElementAt(1).Key.transform.localPosition = new Vector3(0, 150);
            //        _playerViews.ElementAt(1).Key.GetComponentInChildren<Text>().alignment = TextAnchor.LowerCenter;
            //        break;
            //    case 3:
            //        _playerViews.ElementAt(1).Key.transform.localPosition = new Vector3(-70, 100);
            //        _playerViews.ElementAt(2).Key.transform.localPosition = new Vector3(70, 100);
            //        break;
            //    case 4:
            //        _playerViews.ElementAt(1).Key.transform.localPosition = new Vector3(-70, 0);
            //        _playerViews.ElementAt(2).Key.transform.localPosition = new Vector3(0, 155);
            //        _playerViews.ElementAt(3).Key.transform.localPosition = new Vector3(70, 0);
            //        break;
            //    case 5:
            //        _playerViews.ElementAt(1).Key.transform.localPosition = new Vector3(-70, 80);
            //        _playerViews.ElementAt(2).Key.transform.localPosition = new Vector3(-70, 125);
            //        _playerViews.ElementAt(3).Key.transform.localPosition = new Vector3(70, 125);
            //        _playerViews.ElementAt(4).Key.transform.localPosition = new Vector3(70, 80);
            //        break;
            //}
        }

        private List<PlayerOutputModel> SetInstantiationTurn(List<PlayerOutputModel> players, PlayerOutputModel currentPlayer)
        {
            players.OrderBy(player => player.Turn);
            List<PlayerOutputModel> temp = new List<PlayerOutputModel>();

            temp.Add(players.First(player => player.Turn == currentPlayer.Turn));

            foreach (var player in players)
            {
                if (player.Turn > currentPlayer.Turn)
                {
                    temp.Add(player);
                }
            }

            foreach (var player in players)
            {
                if (player.Turn < currentPlayer.Turn)
                {
                    temp.Add(player);
                }
            }

            return temp;
        }

        public void UpdatePlayerViews(PlayerOutputModel updatedPlayer)
        {
            //GameDataStorage.CurrentClient.HealthCount = updatedPlayer.HealthCount;
            GameObject key = _playerViews.First(player => player.Value.Id == updatedPlayer.Id).Key;
            _playerViews[key] = updatedPlayer;
            _healthPoint.GetComponentInChildren<Text>().text = updatedPlayer.HealthCount.ToString();
        }

        public void ShowWinner(int winnerId, GameObject endingScreen, GameObject gameScreen)
        {
            gameScreen.SetActive(false);

            string winnerName = _playerViews.Values.First(player => player.Id == winnerId).PlayerName;

            endingScreen.GetComponentInChildren<Text>().text = winnerName.ToUpper();
            endingScreen.SetActive(true);
        }
    }
}
