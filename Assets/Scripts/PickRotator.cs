using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PickRotator : MonoBehaviour
{
    // Difficulty - Needs Work
    public int LockLevel = 5;
    public int lockPickSkill = 30;
    float LeverageScaler = 0.5f;
    float Leverage;

    // Timer Variables
    public TMPro.TextMeshProUGUI Timer;
    public float LockPickTime = 60;
    float TimeThreshold = 0f;

    // Audio Variables
    public AudioSource LockpickSource;
    public AudioClip ClickClip;
    public AudioClip LockpickClip;

    // Pick Variables
    float PickHealth;
    float AmountOfPicks;

    Vector3 BreakTarget;
    Vector3 HookTarget;

    float MouseClamp;
    float MousePercentage;
    float MouseX;
    public bool foundSpot;
    public float sweetSpotVal;
    float Angle = -90f;
    float smooth = 5.0f;

    // Start is called just before any of the update methods is called the first time
    private void Start()
    {
        Leverage = (lockPickSkill * LeverageScaler) - (LockLevel * LeverageScaler);
        foundSpot = false;
        CreateNewSpot();
    }
    // Update is called once per frame
    void Update()
    {
        LockTimer();
        rotatePivot();
        SweetSpotLocater();
    }

    void LockTimer()
    {
        TimeThreshold += Time.deltaTime;
        if (TimeThreshold >= 1)
        {
            LockPickTime--;
            TimeThreshold = 0f;
        }
        Timer.text = "Time: " + LockPickTime;
    }

    void PlayAudio(AudioClip clip)
    {

        if (LockpickSource.clip != clip)
        {
            LockpickSource.Stop();
            LockpickSource.clip = clip;
            LockpickSource.Play();
        }
        
    }

    void rotatePivot()
    {
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

    void CreateNewSpot()
    {
        float randSpot = Random.Range(-90f, 90f);
        sweetSpotVal = randSpot;
    }

    void SweetSpotLocater()
    {
        //Debug.Log("Z Rotation: " + transform.eulerAngles.z);
        if(sweetSpotVal > 0)
        {
            if (transform.eulerAngles.z > (sweetSpotVal - Leverage) && transform.eulerAngles.z < (sweetSpotVal + Leverage))
            {
                foundSpot = true;
                PlayAudio(ClickClip);
            }
            else
            {
                foundSpot = false;
                PlayAudio(LockpickClip);
            }
        }
        else if(sweetSpotVal < 0)
        {
            if((transform.eulerAngles.z - 360) > (sweetSpotVal - Leverage) && (transform.eulerAngles.z - 360) < (sweetSpotVal + Leverage))
            {
                foundSpot = true;
                PlayAudio(ClickClip);
            }
            else
            {
                foundSpot = false;
                PlayAudio(LockpickClip);
            }
        }
    }

    void SnapPick()
    {
        if(PickHealth <= 0)
        {
            BreakTarget = transform.position;
        }
    }

    private IEnumerator BreakPick(float wait)
    {
        yield return null;
        transform.position = Vector3.Lerp(transform.position, BreakTarget, Time.deltaTime * smooth);
    }

}
