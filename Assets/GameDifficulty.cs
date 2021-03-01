using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDifficulty : MonoBehaviour
{
    public PickRotator pick;
    public Screwdriver_Rotation Screwdriver;

    public bool medLevel1 = false;

    public bool hardLevel1 = false;

    public bool hardLevel2 = false;

    bool isHardLevel2Bool = true;
    
    public GameObject StartGameUI;
    public GameObject EndGameUI;
    
    public TMPro.TextMeshProUGUI resultText;

    private void Start()
    {
        Time.timeScale = 0;
        pick.LockpickSource.volume = 0;
    }
    public void EasyDifficulty()
    {
        Screwdriver.tiltAngle = 90;
        pick.LockLevel = 0;
        pick.AmountOfPicks = 10f;
        pick.LockPickTime = 30;
        pick.PickHealth = 100f;
        pick.isConditionReached = false;
        Screwdriver.isConditionReached = false;
        Time.timeScale = 1;
        pick.LockpickSource.Play();
        pick.LockpickSource.volume = 1;
        StartGameUI.SetActive(false);
        EndGameUI.SetActive(false);
        pick.CreateNewSpot();
        pick.LeverageCalculator();

    }

    public void MediumDifficulty()
    {

        Debug.Log("Medium Difficulty Selected");
        
        Time.timeScale = 1;
        
        Screwdriver.tiltAngle = 45;
        pick.LockLevel = 2;
        pick.AmountOfPicks = 5f;
        pick.LockPickTime = 30;
        pick.PickHealth = 100f;
        pick.isConditionReached = false;
        Screwdriver.isConditionReached = false;      
        pick.LockpickSource.Play();
        pick.LockpickSource.volume = 1;
        StartGameUI.SetActive(false);
        EndGameUI.SetActive(false);
        pick.CreateNewSpot();
        pick.LeverageCalculator();
        StartCoroutine(medLevel1Set());
    }

    public void HardDifficulty()
    {
        Debug.Log("Hard Difficulty Selected");

        Screwdriver.tiltAngle = 30;
        pick.LockLevel = 5;
        pick.AmountOfPicks = 3f;
        pick.LockPickTime = 30;
        pick.PickHealth = 100f;
        pick.isConditionReached = false;
        Screwdriver.isConditionReached = false;
        Time.timeScale = 1;
        pick.LockpickSource.volume = 1;
        StartGameUI.SetActive(false);
        EndGameUI.SetActive(false);
        pick.CreateNewSpot();
        isHardLevel2Bool = true;
        pick.LeverageCalculator();
        StartCoroutine(hardLevel1Set());
    }

    void Update()
    {
        Debug.Log(medLevel1);
        if (medLevel1 == true && Screwdriver.RotateValue == -1)
        {
            Debug.Log("Med Level 2 Activated");
            medLevel2();
            medLevel1 = false;
        }

        if (hardLevel1 == true && Screwdriver.RotateValue == -1)
        {
            Debug.Log("Hard Level 2 Activated");
            hardLvl2();
            StartCoroutine(hardLevel2bool(isHardLevel2Bool));              
        } 
        
        if (hardLevel2 == true && Screwdriver.RotateValue == -1)
        {
            Debug.Log("Hard Level 3 Activated");
            hardLevel3();
            hardLevel2 = false;
        }
    }

    IEnumerator hardLevel2bool(bool isLevlActivated)
    {
        if(isLevlActivated)
        {
            yield return new WaitForSeconds(0.5f);
            hardLevel2 = true;
            isHardLevel2Bool = false;
            Debug.Log("Once");
        }       
    }
    IEnumerator medLevel1Set()
    {
        yield return new WaitForSeconds(1);
        medLevel1 = true;
    }

    void medLevel2()
    {        
        pick.CreateNewSpot();
        Screwdriver.tiltAngle = 90;
    }

    IEnumerator hardLevel1Set()
    {
        yield return new WaitForSeconds(1);
        hardLevel1 = true;
    }

    void hardLvl2()
    {
        pick.CreateNewSpot();
        Screwdriver.tiltAngle = 45;
        pick.LockLevel = 9;
        hardLevel1 = false;
    }

    void hardLevel3()
    {
        pick.CreateNewSpot();
        Screwdriver.tiltAngle = 90;
        
        pick.LockLevel = 10;
    }

    public void WinCondition()
    {
        Debug.Log("You win!");
        EndGameUI.SetActive(true);
        resultText.text = "Winner!";
        Time.timeScale = 0;
        StartGameUI.SetActive(true);
        if(pick.lockPickSkill<5) pick.lockPickSkill += 1.0f;
        pick.LeverageCalculator();
    }

    public void LoseCondition()
    {
        Debug.Log("You Lose!");
        EndGameUI.SetActive(true);
        resultText.text = "Loser!";
        Time.timeScale = 0;
        StartGameUI.SetActive(true);
        if (pick.lockPickSkill < 5) pick.lockPickSkill += 0.5f;
        pick.LeverageCalculator();
    }
}
