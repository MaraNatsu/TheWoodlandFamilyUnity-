using Assets.Scripts.Models;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDataRecorder : MonoBehaviour
{
    public static PlayerDataRecorder Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    //private void Update()
    //{
    //    Debug.Log("name: " + Player.Instance.Nickname);
    //    Debug.Log("wordkey: " + Player.Instance.Wordkey);
    //    Debug.Log("number: " + Player.Instance.PlayerNumber);
    //    Debug.Log("");
    //}

    public void RecordNickname(InputField nickname)
    {
        Player.Instance.Nickname = nickname.text;
    }

    public void RecordKeyword(InputField keyword)
    {
        Player.Instance.Wordkey = keyword.text;
    }

    public void RecordPlayerNumber(InputField playerNumber)
    {
        Player.Instance.PlayerNumber = Convert.ToByte(playerNumber.text);
    }
}