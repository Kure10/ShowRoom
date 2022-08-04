using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ShowRoom/CarDatabase")]
public class CarDatabase : ScriptableObject
{
    [System.Serializable]
    public class Car
    {
        public string carName = "Audi";
        public int uid = 0;
        public string resourcesPath = "car 1203 black";

        public string price = "389 euro";
        public List<Atribute> Atributes = new List<Atribute>();
    }

    public List<Car> cars = new List<Car>();
}

[System.Serializable]
public class Atribute
{
    public Stat Name;
    public string value = "1658";
}

public enum Stat
{
    Volume,
    Clutch,
    Transmission,
    Mass
}
