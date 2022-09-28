using Assets.Scripts.Utils.Validation;
using Assets.Scripts.Utils.Validation.Enums;
using UnityEngine;
using UnityEngine.UI;

public class JoinRoomValidation : MonoBehaviour
{
    [SerializeField]
    private InputField _keyword;
    [SerializeField]
    private Button _joinRoom;

    private bool _keywordHasError = true;

    void Start()
    {
        _keyword.onValueChanged.AddListener(delegate
        {
            InputValidation.Instance.DeleteAllWhiteSpaces(_keyword, (ErrorType error) =>
            {
                StartCoroutine(InputErrorUIManager.Instance.VizualizeError(_keyword, error));
            });

            InputErrorUIManager.Instance.SetNormalPlaceholder(_keyword);
        });

        _keyword.onEndEdit.AddListener(delegate
        {
            _keywordHasError = InputValidation.Instance.CheckIfInputEmpty(_keyword, (ErrorType error) =>
            {
                _joinRoom.interactable = false;
                StartCoroutine(InputErrorUIManager.Instance.VizualizeError(_keyword, error));
            });

            SetButtonInteractability();
        });
    }

    private void SetButtonInteractability()
    {
        if (_keywordHasError)
        {
            _joinRoom.interactable = false;
        }
        else
        {
            _joinRoom.interactable = true;
        }
    }
}
