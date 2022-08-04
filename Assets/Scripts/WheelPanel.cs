using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class WheelPanel : MonoBehaviour
{
    [SerializeField] Button tier1;
    [SerializeField] Button tier2;
    [SerializeField] Button tier3;

    public void PassDelegate(Action <string>evt)
    {
        tier1.onClick.AddListener(delegate { evt("Wheel1"); });
        tier2.onClick.AddListener(delegate { evt("Wheel2"); });
        tier3.onClick.AddListener(delegate { evt("Wheel3"); });
    }

}
