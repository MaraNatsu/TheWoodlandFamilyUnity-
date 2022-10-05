using Assets.Scripts.Models;
using Assets.Scripts.Services;
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WebRequestHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject _webRequestResult;
    [SerializeField]
    private GameObject _roomCreationSuccess;
    [SerializeField]
    private GameObject _webRequestFailure;
    [SerializeField]
    private GameObject _loader;
    [SerializeField]
    private GameObject _waitingTheGame;

    private string _roomCreationRoute = "http://localhost:5000/api/Home/create-room";
    private string _playerCreationRoute = "http://localhost:5000/api/Home/create-player";
    private string _playerRemovingRoute = "http://localhost:5000/api/Home/remove-player";
    private long _requestStatusCode;

    // Web requests for room and player creation
    public void SendRoomCreationRequest()
    {
        StartCoroutine(SendRoomCreationData());
        Debug.Log("Start Coroutine \"Room creation\"");
    }

    public void SendPlayerCreationRequest()
    {
        StartCoroutine(SendPlayerCreationData());
        Debug.Log("Start Coroutine \"Player creation\"");
    }

    private IEnumerator SendRoomCreationData()
    {
        string jsonRequest = $"{{\"Wordkey\": \"{Player.Instance.Wordkey}\", \"PlayerNumber\": {Player.Instance.PlayerNumber}}}";
        yield return CreateRequest(_roomCreationRoute, jsonRequest);

        GiveRoomCreationResult();
    }

    private IEnumerator SendPlayerCreationData()
    {
        string jsonRequest = $"{{\"Name\": \"{Player.Instance.Nickname}\", \"Wordkey\": \"{Player.Instance.Wordkey}\"}}";
        yield return CreateRequest(_playerCreationRoute, jsonRequest, (requestResponse) =>
        {
            GameDataStorage.CurrentClient = Newtonsoft.Json.JsonConvert.DeserializeObject<Player>(requestResponse);
        });

        GivePlayerCreationResult();
    }

    private IEnumerator CreateRequest(string route, string jsonRequest, Action<string> OnRequestDone = null)
    {
        _requestStatusCode = 0;

        UnityWebRequest request = new UnityWebRequest(route, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonRequest);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("An error has uccured: " + request.error);
            yield break;
        }

        OnRequestDone?.Invoke(request.downloadHandler.text);
        _requestStatusCode = request.responseCode;
        Debug.Log("Server response: " + request);
        Debug.Log("Server response: " + request.result);
        Debug.Log("Status code: " + request.responseCode);
    }

    private void GiveRoomCreationResult()
    {
        _loader.SetActive(false);
        _webRequestResult.SetActive(true);

        if (_requestStatusCode >= 200 && _requestStatusCode < 300)
        {
            _roomCreationSuccess.SetActive(true);
            return;
        }

        _webRequestFailure.SetActive(true);
    }

    private void GivePlayerCreationResult()
    {
        _loader.SetActive(false);
        _webRequestResult.SetActive(true);
        _roomCreationSuccess.SetActive(false);

        if (_requestStatusCode >= 200 && _requestStatusCode < 300)
        {
            _waitingTheGame.SetActive(true);
            _webRequestResult.SetActive(false);
            return;
        }

        _webRequestFailure.SetActive(true);
    }
}