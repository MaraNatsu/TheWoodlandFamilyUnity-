using Assets.Scripts.Utils.Validation.Enums;
using System;
using System.Collections;
using System.Text.RegularExpressions;
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
                    placeholder.text = _errorTextIfEmpty;
                    yield return TranslateColor(placeholder.color, _normalColorPlaceholder, _errorColorPlaceholder);
                    break;
                case ErrorType.InputError:
                    yield return TranslateColor(input.image.color, _normalColorInputField, _errorColorInputField);
                    yield return TranslateColor(_normalColorInputField, input.image.color, _errorColorInputField);
                    break;
            }
        }

        private IEnumerator TranslateColor(Color targer, Color from, Color to)
        {
            float index = 0;

            while (index <= 1)
            {
                targer = Color.Lerp(from, to, index);
                yield return null;
                index += _indexModifier;
            }
        }

        //public IEnumerator VizualizeErrorPlaceholder(Text placeholder, ErrorPlace error, float timeToSetView = .2f, byte numberOfIterations = 6, float index = 0)
        //{
        //    float indexModifier = timeToSetView / numberOfIterations;
        //    placeholder.text = _errorTextIfEmpty;

        //    switch (error)
        //    {
        //        case ErrorPlace.Nickname:
        //            while (index <= 1)
        //            {
        //                placeholder.color = Color.Lerp(_normalColorPlaceholder, _errorColorPlaceholder, index);
        //                yield return null;
        //                index += indexModifier;
        //            }
        //            break;
        //        case ErrorPlace.PlayerNumber:
        //            break;
        //        default:
        //            break;
        //    }
        //}

        public void SetNormalPlaceholder(Text placeholder)
        {
            placeholder.text = _normalTextPlaceholder;
            placeholder.color = _normalColorPlaceholder;
        }
    }
}
