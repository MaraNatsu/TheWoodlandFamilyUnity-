using UnityEngine.UI;

namespace Assets.Scripts.Utils
{
    public class CaseConverter
    {
        public static void ConvertCaseToUpper(InputField inputField)
        {
            inputField.text = inputField.text.ToUpper();
        }
    }
}
