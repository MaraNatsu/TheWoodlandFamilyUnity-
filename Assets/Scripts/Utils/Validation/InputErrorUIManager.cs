using Assets.Scripts.Utils.Validation.Enums;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Utils.Validation
{
    public class InputErrorUIManager
    {
        public static InputErrorUIManager Instance { get; private set; } = new InputErrorUIManager();

        private readonly string _normalTextNickname = "ENTER YOUR NAME";
        private readonly string _normalTextKeyword = "SET THE KEYWORD";
        private readonly string _errorTextIfEmpty = "FIELD IS EMPTY!";
        private readonly string _errorTextIfNotNumberInRange = "2-5 NUMBERS ONLY";

        private readonly Color32 _errorColorPlaceholder = new Color32(97, 8, 21, 135);
        private readonly Color32 _normalColorPlaceholder = new Color32(5, 100, 20, 120);
        private readonly Color32 _errorColorInputField = new Color32(97, 8, 21, 100);
        private readonly Color32 _normalColorInputField = new Color32(240, 255, 200, 255);

        private readonly float _indexModifier = .2f;

        private InputErrorUIManager() { }

        public IEnumerator VizualizeError(InputField input, ErrorType errorType)
        {
            switch (errorType)
            {
                case ErrorType.EmptyField:
                    {
                        Text placeholder = input.placeholder.GetComponent<Text>();
                        yield return TranslateColor(placeholder, _normalColorPlaceholder, _errorColorPlaceholder);
                        //yield return TranslateColor((Color32 transitionColor) => placeholder.color = transitionColor, _normalColorPlaceholder, _errorColorPlaceholder);
                        placeholder.text = _errorTextIfEmpty;
                    }
                    break;
                case ErrorType.WhiteSpace:
                    yield return TranslateColor(input.image, _normalColorInputField, _errorColorInputField);
                    yield return TranslateColor(input.image, _errorColorInputField, _normalColorInputField);
                    //yield return TranslateColor((Color32 transitionColor) => input.image.color = transitionColor, _normalColorInputField, _errorColorInputField);
                    //yield return TranslateColor((Color32 transitionColor) => input.image.color = transitionColor, _errorColorInputField, _normalColorInputField);
                    break;
                case ErrorType.NotNumberInRange:
                    {
                        Text placeholder = input.placeholder.GetComponent<Text>();
                        yield return TranslateColor(placeholder, _normalColorPlaceholder, _errorColorPlaceholder);
                        placeholder.text = _errorTextIfNotNumberInRange;
                    }
                    break;
            }
        }

        private IEnumerator TranslateColor(MaskableGraphic target, Color from, Color to)
        {
            float index = 0;

            while (index <= 1)
            {
                target.color = Color.Lerp(from, to, index);
                yield return null;
                index += _indexModifier;
            }
        }

        public void SetNormalPlaceholder(InputField input)
        {
            Text placeholder = input.placeholder.GetComponent<Text>();
            placeholder.color = _normalColorPlaceholder;

            if (input.name.Equals(InputFieldName.Nickname.ToString()))
            {
                placeholder.text = _normalTextNickname;
            }
            else if (input.name.Equals(InputFieldName.KeyWord.ToString()))
            {
                placeholder.text = _normalTextKeyword;
            }
        }

        //private delegate void MyDelegate(Color32 color);

        //private IEnumerator TranslateColor(MyDelegate myDelegate, Color from, Color to)
        //{
        //    float index = 0;

        //    while (index <= 1)
        //    {
        //        Color32 transitionColor = Color.Lerp(from, to, index);
        //        myDelegate(transitionColor);
        //        yield return null;
        //        index += _indexModifier;
        //    }
        //}
    }
}
