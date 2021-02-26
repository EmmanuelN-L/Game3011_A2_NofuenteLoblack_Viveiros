using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screwdriver_Rotation : MonoBehaviour
{
    public PickRotator Pick;

    float smooth = 5.0f;
    float tiltAngle = 90.0f;

    public float RotateIncrement;
    public float RotateValue;

    private void Update()
    {
        RotateLock();
    }

    void RotateLock()
    {
        RotateIncrement = Input.GetAxis("Horizontal") * Time.deltaTime;
        RotateValue += RotateIncrement;
        RotateValue = Mathf.Clamp(RotateValue, -1, 0);

        // Smoothly tilts a transform towards a target rotation.
        float tiltAroundZ = RotateValue * tiltAngle;

        if(Pick.foundSpot)
        {
            // Rotate the cube by converting the angles into a quaternion.
            Quaternion target = Quaternion.Euler(0, 0, tiltAroundZ);

            // Dampen towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
        }
        else
        {
            RotateValue = 0;

            // Rotate the cube by converting the angles into a quaternion.
            Quaternion target = Quaternion.Euler(0, 0, tiltAroundZ);

            // Dampen towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
        }
    }
}
