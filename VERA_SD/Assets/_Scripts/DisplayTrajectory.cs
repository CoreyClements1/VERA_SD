using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayTrajectory : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField][Range(3, 30)] private int _lineSegmentCount = 30;
    private List<Vector3> _linePoints = new List<Vector3>();
    public static DisplayTrajectory Instance;
    private void Awake()
    //--------------------------------------//
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void calculateLine(Vector3 forceVector, Rigidbody rigidBody, Vector3 startingPoint)
    {
        //Transform force to velocity vector
        Vector3 velocity = (forceVector / rigidBody.mass) * Time.fixedDeltaTime;

        // Debug.Log(forceVector);
        // Debug.Log(rigidBody.mass);
        // Debug.Log(Time.fixedDeltaTime);
        // Debug.Log(startingPoint);
        // Debug.Log(velocity);


        // Calculate flight duration
        float flightDuration = (2 * forceVector.magnitude) / Physics.gravity.y;

        // Divide flight duration to step times
        float stepTime = flightDuration / _lineSegmentCount;
        // For each step time passed calculate the position of the object
        // Debug.Log(flightDuration);
        // Debug.Log(stepTime);

        _linePoints.Clear();
        for (int i = 0; i < _lineSegmentCount; i++)
        {
            float stepTimePassed = stepTime * i;
            Vector3 movementVector = new Vector3(
                forceVector.x * stepTimePassed,
                forceVector.y * stepTimePassed - 0.5f * Physics.gravity.y * stepTimePassed * stepTimePassed,
                forceVector.z * stepTimePassed
            );
            // Debug.Log(movementVector);

            _linePoints.Add(-movementVector + startingPoint);
        }
        // Compose the line renderer using the positions
        _lineRenderer.positionCount = _linePoints.Count;
        _lineRenderer.SetPositions(_linePoints.ToArray());
    }
    public void hideLine()
    {
        _lineRenderer.positionCount = 0;
    }
}
