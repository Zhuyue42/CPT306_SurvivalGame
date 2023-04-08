using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameriaTrack : MonoBehaviour
{
    public Transform target;
    public float distanceUp = 10f;//Vertical height parameter of the camera to the target
    public float distanceAway = 10f;//Horizontal distance parameter between camera and target
    public float smooth = 2f;//Positional smooth shift interpolation parameter values
    public float camDepthSmooth = 20f;

    void Update()
    {
        // Mouse axis to control the distance of the camera
        if ((Input.mouseScrollDelta.y < 0 && Camera.main.fieldOfView >= 3) || Input.mouseScrollDelta.y > 0 && Camera.main.fieldOfView <= 80)
        {
            Camera.main.fieldOfView += Input.mouseScrollDelta.y * camDepthSmooth * Time.deltaTime;
        }
    }

    void LateUpdate()
    {
        //Calculate the position of the camera
        Vector3 disPos = target.position + Vector3.up * distanceUp - target.forward * distanceAway;

        transform.position = Vector3.Lerp(transform.position, disPos, Time.deltaTime * smooth);
        //Camera angles
        transform.LookAt(target.position);
    }
}
