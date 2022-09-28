using Assets.Scripts.Utils.Validation;
using Assets.Scripts.Utils.Validation.Enums;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomValidation : MonoBehaviour
{
    [SerializeField]
    private InputField _keyword;
    [SerializeField]
    private InputField _playerNumber;
    [SerializeField]
    private Button _createRoom;

    private int _minPlayerNumber = 2;
    private int _maxPlayerNumber = 5;

    private bool _keywordHasError = true;
    private bool _playerNumberHasError = true;

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
                StartCoroutine(InputErrorUIManager.Instance.VizualizeError(_keyword, error));
            });

            SetButtonInteractability();
        });

        _playerNumber.onValueChanged.AddListener(delegate
        {
            InputValidation.Instance.CheckIfNumberInRange(_playerNumber, (ErrorType error) =>
            {
                StartCoroutine(InputErrorUIManager.Instance.VizualizeError(_playerNumber, error));
            },
            _minPlayerNumber, _maxPlayerNumber);

            InputErrorUIManager.Instance.SetNormalPlaceholder(_playerNumber);
        });

        _playerNumber.onEndEdit.AddListener(delegate
        {
            _playerNumberHasError = InputValidation.Instance.CheckIfInputEmpty(_playerNumber, (ErrorType error) =>
            {
                InputErrorUIManager.Instance.VizualizeError(_playerNumber, error);
            });

        });
    }

    private void SetButtonInteractability()
    {
        if (!_keywordHasError && !_playerNumberHasError)
        {
            _createRoom.interactable = true;
        }
        else
        {
            _createRoom.interactable = false;
        }
    }
}
