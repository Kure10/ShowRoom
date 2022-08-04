using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementController : MonoBehaviour
{
    [Header("Automatic Panning")]
    [Tooltip("Toggles whether force-stop camera")]
    [SerializeField] private bool freez = false;
    [Tooltip("Toggles whether the camera will automatically rotate around it's target")]
    [SerializeField] private bool autoPan = false;
    [Tooltip("Bolean for reverse control")]
    [SerializeField] private bool reverse = false;
    [SerializeField] private float autoPanRotationSpeed = 10f;
    [Header("Manual Panning")]
    [Tooltip("The smoothness coming to a stop of the camera afer the uses pans the camera and releases. Lower values result in significantly smoother results. This means the camera will take longer to stop rotating")]
    [SerializeField] private float rotationSmoothing = 2f;
    [SerializeField] private Transform target;
    [SerializeField] private float panSensitivity = 180f;
    [SerializeField] private Vector2 panLimit = new Vector2(5, 80);
    [Tooltip("The position along the Z-axis the camera game object is.")]
    [SerializeField] private float distance = 5;
    [Header("Zooming")]
    [SerializeField] private Vector2 cameraZoomRange;
    [SerializeField] private float zoomSoothness = 0.1f;
    [SerializeField] private float zoomSensitivity;
    [Header("WheelLookPosition")]
    [SerializeField] private Vector3 cameraWheelPosition;
    [SerializeField] private Vector3 cameraWheelRotation;
    [SerializeField] private int cameraWheelRangeView;

    private Camera _cam;
    private float _cameraZDistance;
    new private Transform _transform;
    private Vector2 _startAngle;
    private float _xVelocity = 0f;
    private float _yVelocity = 0f;
    private float _xRotationAxis = 0.0f;
    private float _yRoptationAxis = 0.0f;
    private float _zoomVelocity = 0f;
    private bool _isZoomedOnWheel = false;

    private DefaultCamValues defaultValues = new DefaultCamValues();

    public bool Freez { get { return freez; } set { freez = value; } }
    public bool AutoPan { get { return autoPan; } set { autoPan = value; } }
    public Transform SetTargetCamera { set { target = value; } }

    public void SetDefaultCameraTarget()
    {
        if (target != null)
            target = defaultValues.defaultTarget;
    }

    public void ReleaseCamera()
    {
        if(_isZoomedOnWheel)
        {
            freez = false;
            target = defaultValues.defaultTarget;
            transform.position = defaultValues.defaultPosition;
            Quaternion rotation = Quaternion.Euler(_yRoptationAxis, _xRotationAxis * autoPanRotationSpeed, 0);
            transform.rotation = rotation;
            _cam.fieldOfView = defaultValues.defaultCameraWheelRangeView;
            _isZoomedOnWheel = false;
        }
    }

    public void RotateCameraToWheel()
    {
        Vector3 vec = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
        defaultValues.SetDefaultValues(transform.position, vec, _cam.fieldOfView);

        freez = true;
        transform.position = cameraWheelPosition;
        Quaternion rotation = Quaternion.Euler(cameraWheelRotation.x, cameraWheelRotation.y, cameraWheelRotation.z);
        transform.rotation = rotation;
        _cam.fieldOfView = cameraWheelRangeView;
        _isZoomedOnWheel = true;
    }

    private void Awake()
    {
        _cam = GetComponent<Camera>();
        _transform = GetComponent<Transform>();
        if (target != null)
            defaultValues.defaultTarget = target;
    }

    private void Start()
    {
        _cameraZDistance = _cam.fieldOfView;

        Vector3 angles = _transform.eulerAngles;

        _startAngle.x = angles.x;
        _startAngle.y = angles.y;
    }

    private void Update()
    {
        if(!freez)
        {
            if (autoPan)
            {
                _xVelocity += panSensitivity * Time.deltaTime;
            }
            if (!ColorPicker.MouseOverColorPicker)
            {
                Zoom();
            }
        }
    }

    private void LateUpdate()
    {
        if (target && !freez)
        {

            if (Input.GetMouseButton(0) && !ColorPicker.MouseOverColorPicker)
            {
                _xVelocity += Input.GetAxis("Mouse X") * panSensitivity;
                _yVelocity -= Input.GetAxis("Mouse Y") * panSensitivity;
            }

            _startAngle.x = _xVelocity;
            _startAngle.y = _yVelocity;

            if (reverse)
            {
                _xRotationAxis -= _xVelocity;
                _yRoptationAxis -= _yVelocity;
            }
            else
            {
                _xRotationAxis += _xVelocity;
                _yRoptationAxis += _yVelocity;
            }

            _yRoptationAxis = ClampAngle(_yRoptationAxis, panLimit.x, panLimit.y);

            Quaternion rotation = Quaternion.Euler(_yRoptationAxis, _xRotationAxis * autoPanRotationSpeed, 0);
            Vector3 position = rotation * new Vector3(0f, 0f, -distance) + target.position;

            _transform.rotation = rotation;
            _transform.position = position;

            _xVelocity = Mathf.Lerp(_xVelocity, 0, Time.deltaTime * rotationSmoothing);
            _yVelocity = Mathf.Lerp(_yVelocity, 0, Time.deltaTime * rotationSmoothing);
        }
    }

    private void Zoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            _cameraZDistance = Mathf.SmoothDamp(_cameraZDistance, _cameraZDistance -= zoomSensitivity, ref _zoomVelocity, Time.deltaTime * zoomSoothness);

            if (_cameraZDistance <= cameraZoomRange.x)
            {
                _cameraZDistance = cameraZoomRange.x;
            }
        }
        else
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                _cameraZDistance = Mathf.SmoothDamp(_cameraZDistance, _cameraZDistance += zoomSensitivity, ref _zoomVelocity, Time.deltaTime * zoomSoothness);

                if (_cameraZDistance >= cameraZoomRange.y)
                {
                    _cameraZDistance = cameraZoomRange.y;
                }
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            _cam.fieldOfView = _cameraZDistance;
        }
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    private class DefaultCamValues
    {
        public Transform defaultTarget;
        public float defaultCameraWheelRangeView;
        public Vector3 defaultPosition;
        public Vector3 defaultRotation;

        public void SetDefaultValues(Vector3 vec1, Vector3 vec2 , float rangeView)
        {
                defaultCameraWheelRangeView = rangeView;
                defaultPosition = vec1;
                defaultRotation = vec2;
        }
    }
}
