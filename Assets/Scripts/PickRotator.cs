using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickRotator : MonoBehaviour
{
    public float MouseClamp;
    public float MousePercentage;
    public float MouseX;
    float Angle = -90f;
    float smooth = 5.0f;
    // Update is called once per frame
    void Update()
    {
        rotatePivot();

    }
    void rotatePivot()
    {
        //rotateAngle += Input.GetAxis("Mouse X") * rotationSpeed * -Time.deltaTime;
        //rotateAngle = Mathf.Clamp(rotateAngle, -90, 90);
        //transform.localRotation = Quaternion.AngleAxis(rotateAngle, new Vector3 (0,0,1));
        
        // Get Mouse X Position
        MouseX = Input.mousePosition.x;
        float CenterMouseX = MouseX - (Screen.width / 2);

        MousePercentage = (CenterMouseX / (Screen.width / 3));
      
        // Clamp Mouse Percent
        MouseClamp = Mathf.Clamp(MousePercentage, -1, 1);

        //The Angle
        float tiltAroundZ = MouseClamp * Angle;

        //The Target Angle For Rotation
        Quaternion target = Quaternion.Euler(0, 0, tiltAroundZ);

        //Apply The Rotation To Our GameObject
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
    }
}
