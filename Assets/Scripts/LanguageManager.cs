using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class LanguageManager 
{
    private static Dictionary<string, string> _currentLanguageText;

    public static Action OnLanguageChange;

    public static Language LanguageSetting = Language.CZ;

    // in future load from json
    public static void Init()
    {
        _currentLanguageText = new Dictionary<string, string>();

        _currentLanguageText.Add("CZ.Accesories", "Příslušenství");
        _currentLanguageText.Add("ENG.Accesories", "Accesories");
        _currentLanguageText.Add("CZ.MetalicLevel", "Metal Level");
        _currentLanguageText.Add("ENG.MetalicLevel", "MetalicLevel");

        _currentLanguageText.Add("CZ.CarInfo", "Informace");
        _currentLanguageText.Add("ENG.CarInfo", "CarInfo");
        _currentLanguageText.Add("WarningMissing", "Text is Missing");
        _currentLanguageText.Add("ENG.Language", "ENG");
        _currentLanguageText.Add("CZ.Language", "CZ");


        _currentLanguageText.Add("ENG.Volume", "Displacement cm3:");
        _currentLanguageText.Add("CZ.Volume", "Zdvihový objem cm3:");

        _currentLanguageText.Add("ENG.Clutch", "Clutch:");
        _currentLanguageText.Add("CZ.Clutch", "Spojka:");

        _currentLanguageText.Add("ENG.Transmission", "Transmission:");
        _currentLanguageText.Add("CZ.Transmission", "Převodovka:");

        _currentLanguageText.Add("ENG.Mass", "Mass:");
        _currentLanguageText.Add("CZ.Mass", "Hmotnost:");

        _currentLanguageText.Add("ENG.Default", "Default:");
        _currentLanguageText.Add("CZ.Default", "Zaklad:");

    }

    public static string GetTextValue(string textKey)
    {
        if (_currentLanguageText == null)
        {
            Init();
        }

        string value = null;
        bool isThere = _currentLanguageText.TryGetValue($"{LanguageSetting.ToString()}.{textKey}", out value);

        if(isThere)
        {
            return value;
        }

        return null;
    }

    public static void ChangeLanguage(Language nextLanguage)
    {
        LanguageSetting = nextLanguage;
        OnLanguageChange?.Invoke();
    }

    public enum Language
    {
        CZ,
        ENG
    }
}
