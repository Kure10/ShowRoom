using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceToCamera : MonoBehaviour
{
    [SerializeField] Transform _cameraTransform;

    [SerializeField] float _distance = 5f;

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = _cameraTransform.rotation;
    }
}
