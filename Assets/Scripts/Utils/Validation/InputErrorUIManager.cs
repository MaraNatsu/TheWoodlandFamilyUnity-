using Assets.Scripts.Utils.Validation.Enums;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Utils.Validation
{
    public class InputErrorUIManager
    {
        private readonly string _normalTextPlaceholder = "ENTER YOUR NAME";
        private readonly string _errorTextIfEmpty = "FIELD IS EMPTY!";

        private readonly Color32 _errorColorPlaceholder = new Color32(97, 8, 21, 135);
        private readonly Color32 _normalColorPlaceholder = new Color32(5, 100, 20, 120);
        private readonly Color32 _errorColorInputField = new Color32(97, 8, 21, 100);
        private readonly Color32 _normalColorInputField = new Color32(240, 255, 200, 255);

        private readonly float _indexModifier = .2f;

        public IEnumerator VizualizeError(InputField input, ErrorType errorType)
        {
            switch (errorType)
            {
                case ErrorType.EmptyField:
                    Text placeholder = input.placeholder.GetComponent<Text>();
                    yield return TranslateColor(placeholder, _normalColorPlaceholder, _errorColorPlaceholder);
                    //yield return TranslateColor((Color32 transitionColor) => placeholder.color = transitionColor, _normalColorPlaceholder, _errorColorPlaceholder);
                    placeholder.text = _errorTextIfEmpty;
                    break;
                case ErrorType.InputError:
                    yield return TranslateColor(input.image, _normalColorInputField, _errorColorInputField);
                    yield return TranslateColor(input.image, _errorColorInputField, _normalColorInputField);
                    //yield return TranslateColor((Color32 transitionColor) => input.image.color = transitionColor, _normalColorInputField, _errorColorInputField);
                    //yield return TranslateColor((Color32 transitionColor) => input.image.color = transitionColor, _errorColorInputField, _normalColorInputField);
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

        private delegate void MyDelegate(Color32 color);

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
        //    Debug.Log("test");
        //}

        public void SetNormalPlaceholder(Text placeholder)
        {
            placeholder.text = _normalTextPlaceholder;
            placeholder.color = _normalColorPlaceholder;
        }
    }
}
