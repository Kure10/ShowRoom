using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationText : MonoBehaviour
{
    private static List<LocalizationText> _localizedTextInstances;

    [SerializeField]
    private string _localizationKey = null;

    private Text _textComponent = null;
    private int _instanceListIndex = -1;

    public string SetLocalizationKey 
    {
        set
        {
            _localizationKey = value;
            UpdateText();
        }
    }

    private void Awake()
    {
        _textComponent = GetComponent<Text>();

        if(_textComponent != null)
        {
            if(_localizedTextInstances == null)
            {
                _localizedTextInstances = new List<LocalizationText>();
                LanguageManager.OnLanguageChange += OnLanguageChange;
            }
            _instanceListIndex = _localizedTextInstances.Count;
            _localizedTextInstances.Add(this);
        }
        else
        {
            return;
        }

        UpdateText();
    }

    private void OnLanguageChange()
    {
        for (int i = 0; i < _localizedTextInstances.Count;)
        {
            if (_localizedTextInstances[i])
            {
                _localizedTextInstances[i].UpdateText();
                ++i;
            }
            else
            {
                _localizedTextInstances[i].RemoveFromInstanceList();
            }
        }
    }

    private void UpdateText()
    {
        string translation = LanguageManager.GetTextValue(_localizationKey);
        if (translation == null)
        {
            translation = LanguageManager.GetTextValue("WarningMissing");
        }
        SetLocalizedText(translation);
    }

    private void SetLocalizedText(string newText)
    {
        if(_textComponent != null && newText != null)
        {
            _textComponent.text = newText;
        }
    }
    private void RemoveFromInstanceList()
    {
        if (_instanceListIndex >= 0)
        {
            _localizedTextInstances.RemoveAt(_instanceListIndex);
            if (_localizedTextInstances.Count > _instanceListIndex)
            {
                _localizedTextInstances[_instanceListIndex]._instanceListIndex = _instanceListIndex;
            }
            _instanceListIndex = -1;
        }
    }

    private void OnDestroy()
    {
        RemoveFromInstanceList();
    }
}
