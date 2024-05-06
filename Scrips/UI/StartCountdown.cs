using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartCountdown : MonoBehaviour
{
    [SerializeField] private Text[] countDownTexts;
    [SerializeField] private float countDownTime;
    private CarController[] playerCars;
    private OpponentCar[] opponentCars;
    private Waypoint[] waypoints;


    void Awake()
    {
        playerCars = FindObjectsOfType<CarController>();
        waypoints = FindObjectsOfType<Waypoint>();
        opponentCars = FindObjectsOfType<OpponentCar>();
        
    }
    private void Start()
    {
        StartCoroutine(StartCountDownCoroutine());
    }

    public void DisableScrips()
    {
        foreach (var playerCar in playerCars) 
        { 
            playerCar.enabled = false;
        }
        foreach (var waypoint in waypoints)
        {
            waypoint.enabled = false;
        } 
        foreach(var opponentCar in opponentCars)
        {
            opponentCar.enabled = false;
        }    
        
    }
    public void EnableScrips()
    {
        foreach (var playerCar in playerCars)
        {
            playerCar.enabled = true;
        }
        foreach (var waypoint in waypoints)
        {
            waypoint.enabled = true;
        }
        foreach (var opponentCar in opponentCars)
        {
            opponentCar.enabled = true;
        }

    }

    IEnumerator StartCountDownCoroutine()
    {
        DisableScrips();
        float currentTime = countDownTime;
        while (currentTime > 0 ) 
        {
            UpdateCounDownText(currentTime);
            yield return new WaitForSeconds( 1f);
            currentTime--;
        }
        EnableScrips();

        UpdateCounDownText("GO");
        yield return new WaitForSeconds( 1f);
        SetCountDownTextActive(false);
        
    }

    public void UpdateCounDownText(string text)
    {
        foreach(Text textCountDown in countDownTexts)
        {
            textCountDown.text = text;

        }    
    }
    public void UpdateCounDownText(float time)
    {
        foreach (Text textCountDown in countDownTexts)
        {
            textCountDown.text = time.ToString("0");

        }
    }

    public void SetCountDownTextActive(bool isActive)
    {
        foreach(Text textcountDown in countDownTexts)
        {
            textcountDown.gameObject.SetActive(isActive);
        }    
    }
}
