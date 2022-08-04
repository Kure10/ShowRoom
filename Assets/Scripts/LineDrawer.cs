using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private float _counter;
    private float _dist;

    [SerializeField] Transform _origin;
    [SerializeField] Transform _destination;

    public float drawSpeed = 5f;
    public float lineWidth = 0.03f;


    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.SetPosition(0, _origin.position);
        _lineRenderer.startWidth = lineWidth;

        _dist = Vector3.Distance(_origin.position , _destination.position);
    }

    // Update is called once per frame
    void Update()
    {
        // slowly anim line to end
        //if(_counter < _dist)
        //{
        //    _counter += 0.1f / drawSpeed;
        //    float x = Mathf.Lerp(0, _dist, _counter);

        //    Vector3 pointA = _origin.position;
        //    Vector3 pointB = _destination.position;
        //    Vector3 pointLine = x * Vector3.Normalize(pointB - pointA) + pointA;

        //    _lineRenderer.SetPosition(1, pointLine);
        //}

        _lineRenderer.SetPosition(0, _origin.position);
        _lineRenderer.SetPosition(1, _destination.position);
    }


}
