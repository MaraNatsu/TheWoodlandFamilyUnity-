using Assets.Scripts.Utils.Validation.Enums;
using System;
using System.Text.RegularExpressions;
using UnityEngine.UI;

namespace Assets.Scripts.Utils.Validation
{
    public class InputValidation
    {
        public static InputValidation Instance { get; private set; } = new InputValidation();

        private InputValidation() { }

        public void ManageWhiteSpaces(InputField input, Action<ErrorType> ErrorVizualizer)
        {
            if (string.IsNullOrWhiteSpace(input.text))
            {
                input.text = string.Empty;
                return;
            }

            if (input.text.Contains("  "))
            {
                //StartCoroutine(_inputError.VizualizeError(input, ErrorType.WhiteSpace));
                ErrorVizualizer(ErrorType.WhiteSpace);
            }

            input.text = Regex.Replace(input.text, "  ", " ");
        }

        public void DeleteAllWhiteSpaces(InputField input, Action<ErrorType> ErrorVizualizer)
        {
            if (string.IsNullOrWhiteSpace(input.text))
            {
                input.text = string.Empty;
                return;
            }

            if (input.text.Contains(" "))
            {
                //StartCoroutine(_inputError.VizualizeError(input, ErrorType.WhiteSpace));
                ErrorVizualizer(ErrorType.WhiteSpace);
            }

            input.text = Regex.Replace(input.text, " ", "");
        }

        public bool CheckIfInputEmpty(InputField input, Action<ErrorType> ErrorVizualizer)
        {
            if (!string.IsNullOrEmpty(input.text))
            {
                input.text = input.text.TrimEnd();
                return false;
            }

            //StartCoroutine(_inputError.VizualizeError(input, ErrorType.EmptyField));
            ErrorVizualizer(ErrorType.EmptyField);
            return true;
        }

        public void CheckIfNumberInRange(InputField input, Action<ErrorType> ErrorVizualizer, byte minNumber, byte maxNumber, byte characterLimit = 1)
        {
            input.characterLimit = characterLimit;
            Regex regex = new Regex($"[{minNumber}-{maxNumber}]");

            if (regex.IsMatch(input.text))
            {
                return;
            }

            input.text = string.Empty;
            //StartCoroutine(_inputError.VizualizeError(input, ErrorType.NotNumberInRange));
            ErrorVizualizer(ErrorType.NotNumberInRange);
        }

        public bool CheckIfWordInRange(InputField input, Action<ErrorType, byte> ErrorVizualizer, byte minCharacterLimit)
        {
            ErrorVizualizer(ErrorType.NotWordInRange, minCharacterLimit);

            if (input.text.Length < minCharacterLimit)
            {
                return true;
            }

            return false;
        }
    }
}
