//using UnityEngine;
//using UnityEngine.UI;
//using PlayerInputValidationLibrary.Validators;
//using PlayerInputValidationLibrary.Models;
//using System;
//using FluentValidation.Results;

//public class ValidationScript : MonoBehaviour
//{
//    [SerializeField]
//    private InputField _nickname;
//    [SerializeField]
//    private InputField _wordKey;
//    [SerializeField]
//    private InputField _playerNumber;

//    public ValidationResult ValidateNickname()
//    {
//        PlayerValidator validator = new PlayerValidator();
//        return validator.Validate(_nickname.text);
//    }

//    public ValidationResult ValidateWordkey()
//    {
//        RoomValidator validator = new RoomValidator();
//        RoomModel room = new RoomModel();
//        room.WordKey = _wordKey.text;
//        room.PlayerNumber = Int32.Parse(_playerNumber.text);

//        return validator.Validate(room);
//    }
//}
