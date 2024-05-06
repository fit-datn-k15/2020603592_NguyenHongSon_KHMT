using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OpponentCar : MonoBehaviour
{
    [Header("Car Engine")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float CurentSpeed;
    [SerializeField] private float acceleration = 1f;
    [SerializeField] private float turningSpeed = 30f;
    [SerializeField] private float breakSpeed = 12f;

   


    [Header("Destination Var")]
    [SerializeField] private Vector3 desnitation;
    [SerializeField] private bool destinationReached;


    [Header("Spawn")]
    public float respawnTimeThreshold = 30f;
    public float respawnTimer = 0f;


    [Header("Lap")]
    [SerializeField] private float maxLap;
    [SerializeField] private float CurrentLap;



    private Rigidbody rb;





    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        maxLap = FindObjectOfType<LapSystem>().getMaxLap();
    }

    void Update()
    {
        Drive();
        if(!destinationReached)
        {
            respawnTimer += Time.deltaTime;

            if(respawnTimer >= respawnTimeThreshold)
            {
               RespawnAtDestination();
            }
        }  
        else
        {
            respawnTimer = 0;
        }    
    }
    public void Drive()
    {
        if (!destinationReached)
        {
            Vector3 destinationDirection = desnitation - transform.position;
            destinationDirection.y =  0f;
            float destinationDistance = destinationDirection.magnitude;
            if (destinationDistance >= breakSpeed)
            {
                Quaternion targetRotation = Quaternion.LookRotation(destinationDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turningSpeed * Time.deltaTime);

                CurentSpeed = Mathf.MoveTowards(CurentSpeed,maxSpeed, acceleration*Time.deltaTime); 
                rb.velocity = transform.forward * CurentSpeed;
            }    
            else
            {
                destinationReached = true;
                rb.velocity = Vector3.zero;

            }    

        }     
    } 
    

    public void LocateDestination(Vector3 destination)
    {
        this.desnitation = destination;
        destinationReached  = false;
    }    
    public float getAcceleration()
    { return acceleration; }

    public float getCurrentSpeed()
    {
        return CurentSpeed;
    }

    public void SetAcceleration(float acceleration)
    {
        this.acceleration = acceleration;
    }

    public void SetCurrentSpeed(float currentSpeed)
    {
        this.CurentSpeed = currentSpeed;
    }
    public bool IsDestinationReached() { return destinationReached; }


    public float getCurrentLap()
    {
        return CurrentLap;
    }
    public void resetAccleration()
    {
        this.acceleration = Random.Range(3.5f, 5f);
        this.CurentSpeed = Random.Range(38f, 43f);
    }    
    // Update is called once per frame



    //hoi sinh
    public void RespawnAtDestination()
    {
        respawnTimer = 0;
        CurentSpeed = 5f;
        transform.position = desnitation;
     
        destinationReached = false;
     }    
     

    public void IncreaseLap()
    {
        CurrentLap++;
        Debug.Log("Car" + gameObject.name + "Lap" + CurrentLap);
    }


}
