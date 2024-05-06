using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarSelection : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject allCarContainer;
    [SerializeField] private GameObject[] allCars;
    private int currentIndex = 0;


    void Start()
    {
        allCars = new GameObject[allCarContainer.transform.childCount];

        for (int i = 0; i < allCarContainer.transform.childCount; i++)
        {
            allCars[i] = allCarContainer.transform.GetChild(i).gameObject;
            allCars[i].SetActive(false);
                
        }
        if(PlayerPrefs.HasKey("SelectedCarIndex"))
        {
            currentIndex = PlayerPrefs.GetInt("SelectedCarIndex");
                
        }
        ShowCurrentCar();
    }
    public void ShowCurrentCar()
    {
        foreach (GameObject car in allCars) 
        {
            car.SetActive(false);
        }
        allCars[currentIndex].SetActive(true);
    }
    // Update is called once per frame

    public void NextCar()
    {
        currentIndex = (currentIndex + 1) % allCars.Length;
        ShowCurrentCar();
    }

    public void PreviousCar()
    {
        currentIndex = (currentIndex - 1 + allCars.Length) % allCars.Length;
        ShowCurrentCar();
    }

    public void OnyesButtonClick(string ScreneName)
    {
        PlayerPrefs.SetInt("SelectedCarIndex", currentIndex);
        PlayerPrefs.Save();
        //Debug.Log("Select Car Save");
        SceneManager.LoadSceneAsync(ScreneName);
        
    }
    void Update()
    {
        
    }
}
