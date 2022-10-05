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

    private readonly byte _minCharacterLimit = 4;
    private readonly byte _maxCharacterLimit = 8;

    private bool _keywordHasError = true;
    private bool _keywordIsShort = true;

    void Start()
    {
        _keyword.characterLimit = _maxCharacterLimit;

        _keyword.onValueChanged.AddListener(delegate
        {
            InputValidation.Instance.DeleteAllWhiteSpaces(_keyword, (ErrorType error) =>
            {
                StartCoroutine(InputErrorUIManager.Instance.VizualizeError(_keyword, error));
            });

            _keywordIsShort = InputValidation.Instance.CheckIfWordInRange(_keyword, (ErrorType error, byte minCharacterLimit) =>
            {
                StartCoroutine(InputErrorUIManager.Instance.VizualizeError(_keyword, error, minCharacterLimit));
            },
            _minCharacterLimit);

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
        if (!_keywordHasError && !_keywordIsShort)
        {
            _joinRoom.interactable = true;
        }
        else
        {
            _joinRoom.interactable = false;
        }
    }
}
