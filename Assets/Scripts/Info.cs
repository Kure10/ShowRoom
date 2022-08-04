using UnityEngine;
using UnityEngine.UI;

public class Info : MonoBehaviour
{
    [SerializeField] Text _value;
    [SerializeField] LocalizationText _locText;

    public string SetValue 
    { 
        set 
        {
            this.gameObject.SetActive(true);
            _value.text = value;
        }
    }

    public string SetLoText
    {
        set
        {
            _locText.SetLocalizationKey = value;
        }
    }
}
