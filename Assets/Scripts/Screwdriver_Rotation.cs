using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screwdriver_Rotation : MonoBehaviour
{
    public PickRotator Pick;
    public GameDifficulty game;

    float DamageMuliplier = 40.0f;
    float smooth = 5.0f;
    public float tiltAngle = 90.0f;

    public float RotateIncrement;
    public float RotateValue;
    public float RotateMin = -1f;

    public bool isConditionReached = false;


    private void Update()
    {
        RotateLock();
    }

    void RotateLock()
    {
        RotateIncrement = Input.GetAxis("Horizontal") * Time.deltaTime;
        RotateValue += RotateIncrement;
        RotateValue = Mathf.Clamp(RotateValue, RotateMin, 0);

        // Smoothly tilts a transform towards a target rotation.
        float tiltAroundZ = RotateValue * tiltAngle;

        if(Pick.foundSpot && RotateValue < 0)
        {
            RotateMin = -1;
            if(tiltAngle == 90 && transform.rotation.eulerAngles.z <= 275 && transform.rotation.eulerAngles.z > 0 && !isConditionReached)
            {
                game.WinCondition();
                isConditionReached = true;
            }
        }
        else if(!Pick.foundSpot && RotateValue < 0)
        {
            RotateMin = -0.1f;
            Pick.PickHealth -= Time.deltaTime * DamageMuliplier;
            Pick.PickHealth = Mathf.Clamp(Pick.PickHealth, 0, 100);
            Pick.SnapPick();
        }

        // Rotate the cube by converting the angles into a quaternion.
        Quaternion target = Quaternion.Euler(0, 0, tiltAroundZ);

        // Dampen towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
    }  
    public void resetPosition()
    {
        transform.rotation = new Quaternion(0, 0, 0,0);
    }
}
