using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDifficulty : MonoBehaviour
{
    public PickRotator pick;
    public Screwdriver_Rotation Screwdriver;

    public bool medLevel1 = false;


    public void MediumDifficulty()
    {

        Debug.Log("Medium Difficulty Selected");

        medLevel1 = true;
        Screwdriver.tiltAngle = 45;
        pick.LockLevel = 5;
        
        
    }
    void Update()
    {
        //if(medLevel1 == true && Screwdriver.RotateValue == -1)
        //{
        //    medLevel2();
        //    medLevel1 = false;
        //}
    }

    void medLevel2()
    {        
        pick.CreateNewSpot();
        Screwdriver.tiltAngle = 90;
    }
   
}
