using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LapSystem : MonoBehaviour
{


    [SerializeField] private float maxLap;
    [SerializeField] private Text textresult;
    private float Currentlap;

    // Start is called before the first frame update
    void Start()
    {
        textresult.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float getMaxLap() { return maxLap; }
    public float getCurrentlap() { return Currentlap;}


    private void OnTriggerEnter(Collider other)
    {
        OpponentCar opponentCar = other.GetComponent<OpponentCar>();
        CarController playerCar = other.GetComponent<CarController>();

        if (opponentCar != null)
        {
            opponentCar.IncreaseLap();
            CheckRaceCompletion(opponentCar);
            Invoke("loadMenuScene", 3f);

        }

        if (playerCar != null)
        {
            playerCar.IncreaseLap();
            CheckRaceCompletion(playerCar);
            Invoke("loadMenuScene", 3f);
        }
    }

    private void CheckRaceCompletion(OpponentCar opponentCar)
    {
        if (opponentCar.getCurrentLap() == maxLap)
        {
            EndMission(false);
            Debug.Log(opponentCar.getCurrentLap());
            textresult.text = "OPPONENT WIN";
        }
    }

    private void CheckRaceCompletion(CarController playerCar)
    {
        if (playerCar.getCurrentLap() == maxLap)
        {
            EndMission(true);
            Debug.Log(playerCar.getCurrentLap());
            textresult.text = "PLAYER WIN";

        }
    }


    void EndMission(bool success)
    {
        if (success)
        {
            Debug.Log("Player Wins");
        }
        else
        {
            Debug.Log("Player Lose");
        }
    }
    void loadMenuScene()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
