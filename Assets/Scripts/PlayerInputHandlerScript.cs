using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerInputHandlerScript : MonoBehaviour
{
    [SerializeField]
    private InputField PlayerName;
    [SerializeField]
    private InputField WordKey;
    [SerializeField]
    private InputField PlayerNumber;

    string roomCreationRoute = "http://localhost:5000/api/Home/create-room";
    string roomJoiningRoute = "http://localhost:5000/api/Home/join-room";

    public void SendData()
    {
        StartCoroutine(SendRoomCreationData());
    }

    private IEnumerator SendRoomCreationData()
    {
        string jsonRequest = $"{{\"PlayerName\": \"{PlayerName.text}\", \"WordKey\": \"{WordKey.text}\", \"PlayerNumber\": {PlayerNumber.text}}}";
        UnityWebRequest roomCreationRequest = new UnityWebRequest(roomCreationRoute, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonRequest);
        roomCreationRequest.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        roomCreationRequest.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        roomCreationRequest.SetRequestHeader("Content-Type", "application/json");
        yield return roomCreationRequest.SendWebRequest();

        if (roomCreationRequest.error != null)
        {
            Debug.Log("An error has uccured: " + roomCreationRequest.error);
        }
        Debug.Log("Server response: " + roomCreationRequest);
        Debug.Log("Server response: " + roomCreationRequest.result);
        Debug.Log("Status code: " + roomCreationRequest.responseCode);
    }
}
