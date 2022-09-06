using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Utils
{
    public class CaseConverter : MonoBehaviour
    {
        public void ConvertCaseToUpper(InputField inputField)
        {
            inputField.text = inputField.text.ToUpper();
        }
    }
}
