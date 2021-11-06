using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerInputHandlerScript : MonoBehaviour
{
    [SerializeField]
    private InputField _playerName;
    [SerializeField]
    private InputField _wordKeyToCreateRoom;
    [SerializeField]
    private InputField _wordKeyToJoinRoom;
    [SerializeField]
    private InputField _playerNumber;

    private string _roomCreationRoute = "http://localhost:5000/api/Home/create-room";
    //private string _roomJoiningRoute = "http://localhost:5000/api/Home/join-room";

    public void ConvertCaseToUpper()
    {
        _playerName.text = _playerName.text.ToUpper();
    }

    public void SendRoomCreationRequest()
    {
        StartCoroutine(SendRoomCreationData());
    }

    //public void SendRoomJoiningRequest()
    //{
    //    StartCoroutine(SendRoomJoiningData());
    //}

    private IEnumerator SendRoomCreationData()
    {
        string jsonRequest = $"{{\"PlayerName\": \"{_playerName.text}\", \"WordKey\": \"{_wordKeyToCreateRoom.text}\", \"PlayerNumber\": {_playerNumber.text}}}";
        return CreateRequest(_roomCreationRoute, jsonRequest);
    }

    //private IEnumerator SendRoomJoiningData()
    //{
    //    string jsonRequest = $"{{\"PlayerName\": \"{_playerName.text}\", \"WordKey\": \"{_wordKeyToJoinRoom.text}\"}}";
    //    return CreateRequest(_roomJoiningRoute, jsonRequest);
    //}

    private IEnumerator CreateRequest(string route, string jsonRequest)
    {
        UnityWebRequest request = new UnityWebRequest(route, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonRequest);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("An error has uccured: " + request.error);
        }
        Debug.Log("Server response: " + request);
        Debug.Log("Server response: " + request.result);
        Debug.Log("Status code: " + request.responseCode);
    }
}
