using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] Info _infoPrefab;

    public List<Info> CarInformations = new List<Info>();

    public void SetCarInformation (CarDatabase.Car car)
    {
        CarInformations.Clear();
        int counter = 0;

        foreach (Transform child in transform)
        {
            Info info = child.GetComponent<Info>();
            CarInformations.Add(info);
            counter++;
        }

        int difference = car.Atributes.Count - counter;

        // car has not enought info cell need create some Info
        if (difference > 0)
        {
            for (int i = 0; i < difference; i++)
            {
                Info info = Instantiate(_infoPrefab, this.transform);
                CarInformations.Add(info);
            }
        }

        // car has more than enought info cell need disable some
        if (difference < 0)
        {
            for (int i = car.Atributes.Count; i < counter; i++)
            {
                CarInformations[i].gameObject.SetActive(false);
            }
        }

        // set value to car atributes
        for (int i = 0; i < car.Atributes.Count  ; i++)
        {
            CarInformations[i].SetValue = car.Atributes[i].value;
            CarInformations[i].SetLoText = Enum.GetName(typeof(Stat), car.Atributes[i].Name);
        }
    }


}
