using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickRotator : MonoBehaviour
{
    float MousePositionPercentage;
    float rotateAngle;
    public float rotationSpeed;
    // Update is called once per frame
    void Update()
    {
        rotatePivot();

    }
    void rotatePivot()
    {
        rotateAngle += Input.GetAxis("Mouse X") * rotationSpeed * -Time.deltaTime;
        rotateAngle = Mathf.Clamp(rotateAngle, -90, 90);
        transform.localRotation = Quaternion.AngleAxis(rotateAngle, new Vector3 (0,0,1));
    }
}
