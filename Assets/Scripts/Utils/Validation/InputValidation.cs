using Assets.Scripts.Utils.Validation.Enums;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Utils.Validation
{
    public class InputValidation : MonoBehaviour
    {
        public static InputValidation Instance { get; private set; }

        [SerializeField]
        private Button _createRoom;
        [SerializeField]
        private Button _joinRoom;

        private int _minPlayerNumver = 2;
        private int _maxPlayerNumber = 5;

        private bool _hasError = true;
        private InputErrorUIManager _inputError;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            _inputError = new InputErrorUIManager();
        }

        public void CheckWhiteSpaces(InputField input)
        {
            if (string.IsNullOrWhiteSpace(input.text))
            {
                input.text = string.Empty;
                return;
            }

            if (input.name == "Nickname")
            {
                if (input.text.Contains("  "))
                {
                    StartCoroutine(_inputError.VizualizeError(input, ErrorType.InputError));
                }

                input.text = Regex.Replace(input.text, "  ", " ");
            }
            else
            {
                if (input.text.Contains(" "))
                {
                    StartCoroutine(_inputError.VizualizeError(input, ErrorType.InputError));
                }

                input.text = Regex.Replace(input.text, " ", "");
            }
        }

        public void CheckIfInputEmpty(InputField input)
        {

            if (!string.IsNullOrEmpty(input.text))
            {
                _hasError = false;
                _createRoom.interactable = true;
                _joinRoom.interactable = true;
                input.text = input.text.Trim();
                return;
            }

            _hasError = true;
            _createRoom.interactable = false;
            _joinRoom.interactable = false;
            StartCoroutine(_inputError.VizualizeError(input, ErrorType.EmptyField));
        }

        public void CheckIfNumber()
        {

        }

        public void CheckIfPlayerNumberInRange()
        {

        }

        public void SetNormalPlaceholder(Text placeholder)
        {
            _inputError.SetNormalPlaceholder(placeholder);
        }
    }
}
