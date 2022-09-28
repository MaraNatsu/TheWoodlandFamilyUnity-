using Assets.Scripts.Models;
using Assets.Scripts.Utils.Validation.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Utils.Validation.ValidationScripts
{
    public class NicknameValidation : MonoBehaviour
    {
        [SerializeField]
        private InputField _nickname;
        [SerializeField]
        private Button _createRoom;
        [SerializeField]
        private Button _joinRoom;

        private bool _nicknameHasError = true;

        void Start()
        {
            _nickname.onValueChanged.AddListener(delegate
            {
                CaseConverter.ConvertCaseToUpper(_nickname);

                InputValidation.Instance.ManageWhiteSpaces(_nickname, (ErrorType error) =>
                {
                    StartCoroutine(InputErrorUIManager.Instance.VizualizeError(_nickname, error));
                });

                InputErrorUIManager.Instance.SetNormalPlaceholder(_nickname);
            });

            _nickname.onEndEdit.AddListener(delegate
            {
                _nicknameHasError = InputValidation.Instance.CheckIfInputEmpty(_nickname, (ErrorType error) =>
                {
                    StartCoroutine(InputErrorUIManager.Instance.VizualizeError(_nickname, error));
                });

                SetButtonInteractability();
                RecordNickname();
            });
        }

        private void RecordNickname()
        {
            Player.Instance.Nickname = _nickname.text;
        }

        private void SetButtonInteractability()
        {
            if (_nicknameHasError)
            {
                _createRoom.interactable = false;
                _joinRoom.interactable = false;
            }
            else
            {
                _createRoom.interactable = true;
                _joinRoom.interactable = true;
            }
        }
    }
}
