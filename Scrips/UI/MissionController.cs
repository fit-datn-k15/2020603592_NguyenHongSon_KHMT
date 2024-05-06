using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MissionController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float timeLimitInSecond = 120f;
    private float eslapsedTime = 0f;
    private bool misstionCompleted = false;


    [SerializeField] private Text timeText;
    [SerializeField] private Text resutlText;


    private void Start()
    {
        resutlText.text = "";
    }
    private void Update()
    {
        if(!misstionCompleted)
        {
            eslapsedTime += Time.deltaTime;
            if(eslapsedTime > timeLimitInSecond)
            {
                EndMission(false);
            }

            UpdateTimeText();
        }    
    }

    private void OnTriggerEnter(Collider other)
    {
        CarController carController = other.GetComponent<CarController>();
        if (carController != null)
        {
            EndMission(true);
        }
        
    }

    public void UpdateTimeText()
    {
        float remainingTime = Mathf.Max(0f, timeLimitInSecond - eslapsedTime);
        int munites = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);

        timeText.text = string.Format("{0:00}:{1:00}", munites, seconds);
    }

    void EndMission(bool success)
    {
        misstionCompleted = true;
        if (success)
        {
            resutlText.text = "YOU WIN";
        }
        else
        {
            resutlText.text = "YOU LOSE";
        }
        Invoke("loadMenuScene", 3f);
    }   

    void loadMenuScene()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }    
}
