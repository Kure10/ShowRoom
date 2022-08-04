using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarModel : MonoBehaviour
{
    private int _id = 0;
    public GameObject body = null;
    public GameObject back = null;
    [SerializeField] private string _defaultWheel = "wheelDefault";
    private Color _defaultColor = new Color();

    public int GetID { get { return _id; } }

    public Color GetDefaultColor { get { return _defaultColor; } }

    public string GetDefaultWheel { get { return _defaultWheel; } }

    private void Awake()
    {
        if (body != null)
        {
            MeshRenderer meshRenderer = body.GetComponent<MeshRenderer>();
            _defaultColor = meshRenderer.material.color;
        }

        //foreach (Transform child in GetComponentsInChildren<Transform>())
        //{
        //    if (child.gameObject.name.StartsWith("wheel"))
        //    {
        //        _defaultWheel = child.name;
        //        break;
        //    }
        //}
    }

    public void SetId(int id)
    {
        _id = id;
    }
}
