using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screwdriver_Rotation : MonoBehaviour
{
    float smooth = 5.0f;
    float tiltAngle = 90.0f;
    float Rotator = 0.0f;

    public float RotateIncrement;
    public float RotateValue;
   
    private void Update()
    {

        RotateIncrement = Input.GetAxis("Horizontal") * Time.deltaTime;
        RotateValue += RotateIncrement;
        RotateValue = Mathf.Clamp(RotateValue, -1, 0);
        
        if (Input.GetKeyDown("a"))
        {
            Rotator = -1;
        }

        if (Input.GetKeyDown("d"))
        {
            Rotator = 0;
        }

        // Smoothly tilts a transform towards a target rotation.
        float tiltAroundZ = RotateValue * tiltAngle;

        // Rotate the cube by converting the angles into a quaternion.
        Quaternion target = Quaternion.Euler(0, 0, tiltAroundZ);

        // Dampen towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
    }
}
