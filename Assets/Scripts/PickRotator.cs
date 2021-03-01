using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PickRotator : MonoBehaviour
{
    // Difficulty - Needs Work
    public GameDifficulty game;
    public int LockLevel = 0;
    public float lockPickSkill = 5;
    float LeverageScaler = 0.5f;
    public float Leverage;
    public bool isConditionReached = false;
    public float baseValue = 5f;

    // Timer Variables
    public TMPro.TextMeshProUGUI TimerText;
    public TMPro.TextMeshProUGUI PickHPText;
    public TMPro.TextMeshProUGUI numOfPicksText;
    public TMPro.TextMeshProUGUI playerLevelText;
    public float LockPickTime = 60;
    float TimeThreshold = 0f;

    // Audio Variables
    public AudioSource LockpickSource;
    public AudioClip ClickClip;
    public AudioClip LockpickClip;

    // Pick Variables
    public GameObject Pick;
    public float PickHealth;
    public float AmountOfPicks = 10f;

    Vector3 BreakTarget;
    Vector3 HookTarget;
    bool IsSnapPlaying = false;

    float MouseClamp;
    float MousePercentage;
    float MouseX;
    public bool foundSpot;
    public float sweetSpotVal;
    float Angle = -90f;
    float smooth = 5.0f;
    List<float> usedValues = new List<float>();
    
    // Start is called just before any of the update methods is called the first time
    private void Start()
    {
        Leverage = baseValue + ((lockPickSkill * LeverageScaler) - (LockLevel * LeverageScaler));
        foundSpot = false;
        CreateNewSpot();
        
    }

    public void LeverageCalculator()
    {
        Leverage = baseValue + ((lockPickSkill * LeverageScaler) - (LockLevel * LeverageScaler));
    }
    // Update is called once per frame
    void Update()
    {
        UpdateUiText();
        rotatePivot();
        SweetSpotLocater();
    }

    void UpdateUiText()
    {
        TimeThreshold += Time.deltaTime;
        if (TimeThreshold >= 1)
        {
            LockPickTime--;
            TimeThreshold = 0f;
            TimerText.text = "Time: " + LockPickTime;
            
            
        }       
        if(LockPickTime <=0 && !isConditionReached)
        {
            game.LoseCondition();
            isConditionReached = true;
        }
        numOfPicksText.text = "Picks left: " + AmountOfPicks;
        PickHPText.text = "Pick Health: " + PickHealth;
        playerLevelText.text = "Player Level: " + lockPickSkill;
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
        if(!IsSnapPlaying)
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
    }

    public void CreateNewSpot()
    {
        float randSpot = Random.Range(-90f, 90f);
        while(usedValues.Contains(randSpot))
        {
            randSpot = Random.Range(-90f, 90f);
        }
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

    public void SnapPick()
    {
        if (PickHealth <= 0 && IsSnapPlaying == false)
        {
            
            AmountOfPicks--;
            if(AmountOfPicks<0 && !isConditionReached)
            {
                game.LoseCondition();
                isConditionReached = true;
            }
            StartCoroutine(ResetPick());
        }
    }

    private IEnumerator ResetPick()
    {
        IsSnapPlaying = true;
        var StartPos = new Vector3(0f, 0f, 4.6f);
        var RiseLeftPos = new Vector3(-2.0f, 1.0f, StartPos.z);
        var RiseRightPos = new Vector3(2.0f, 1.0f, StartPos.z);
        var FallPos = new Vector3(StartPos.x, StartPos.y - 5.0f, StartPos.z);
        var SpawnPos = new Vector3(StartPos.x, StartPos.y + 5f, StartPos.z);

        if(MouseClamp < 0)
        {
            while (Vector3.Distance(Pick.transform.position, RiseLeftPos) > 0.5f)
            {
                Pick.transform.position = Vector3.Lerp(Pick.transform.position, RiseLeftPos, Time.deltaTime * 2f);
                yield return null;
            }

        }
        else
        {
            while (Vector3.Distance(Pick.transform.position, RiseRightPos) > 0.5f)
            {
                Pick.transform.position = Vector3.Lerp(Pick.transform.position, RiseRightPos, Time.deltaTime * 2f);
                yield return null;
            }
        }

        while (Vector3.Distance(Pick.transform.position, FallPos) > 0.2f)
        {
            Pick.transform.position = Vector3.Lerp(Pick.transform.position, FallPos, Time.deltaTime * 1f);
            yield return null;
        }

        //Set Pick Above Lock
        Pick.transform.position = SpawnPos;

        while (Vector3.Distance(Pick.transform.position, StartPos) > 0.05f)
        {
            Pick.transform.position = Vector3.Lerp(Pick.transform.position, StartPos, Time.deltaTime * 4f);
            yield return null;
        }

        IsSnapPlaying = false;
        PickHealth = 100;
    }

}
