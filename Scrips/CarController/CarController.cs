using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class CarController : MonoBehaviour
{
    // Start is called before the first frame update
    public enum CarType
    {
        FrontWheelDrive,
        RearWheelDrive,
        FourWheelDrive
    }
    public CarType cartype = CarType.FourWheelDrive;

    public enum ControlMode
    {
        KeyBoard,
        Button
    }

    public ControlMode controlmode;
    [Header("Wheel GameObeject Meshes")]
    [SerializeField] private GameObject FrontWheelLeft;
    [SerializeField] private GameObject BackWheelLeft;
    [SerializeField] private GameObject FrontWheelRight;
    [SerializeField] private GameObject BackWheelRight;

    [Header("Wheel Conllider")]
    [SerializeField] private WheelCollider FrontWheelLeftCollider;
    [SerializeField] private WheelCollider BackWheelLeftCollider;
    [SerializeField] private WheelCollider FrontWheelRightCollider;
    [SerializeField] private WheelCollider BackWheelRightCollider;

    [Header("Movement, steering, Braking")]
    private float CurrentSpeed;
    public float maximumMotoTorque;
    public float maximumSteeringAngel = 20f;
    public float maximumSpeed;
    public float brakePower;
    public Transform COM;
    float carSpeed;
    float carSpeedCoverted;
    float MotorTorque;
    float tireAngel;
    float vertical = 0f;
    float horizotal = 0f;
    bool handBrake = false;
    Rigidbody carRigibody;

    [Header("Sound and Effect")]
    [SerializeField] private ParticleSystem[] SmokeEffect;
    [SerializeField] private TrailRenderer[] trailRenderers;
    private bool SmokeEffectEnabel;

    [SerializeField] private Transform brakeLightLeft;
    [SerializeField] private Transform brakeLightRight;
    Material brakeLightLeftMat;
    Material brakeLightRightMat;
    Color brakeColor = new Color32(180, 0, 10, 0);


    [SerializeField] private AudioSource EngineSource;
    [SerializeField] private AudioClip EngineClip;


    [Header("Lap")]
    [SerializeField] private float maxLap;
    [SerializeField] private float CurrentLap;

    void Start()
    {
        //color of light
        brakeLightLeftMat = brakeLightLeft.GetComponent<Renderer>().material;
        brakeLightRightMat = brakeLightRight.GetComponent<Renderer>().material;
        brakeLightLeftMat.EnableKeyword("_EMISSION");
        brakeLightRightMat.EnableKeyword("_EMISSION");

        //audio


        EngineSource.loop = true;
        EngineSource.playOnAwake = false;
        EngineSource.volume = 0.5f;
        EngineSource.pitch = 1f;

        maxLap = FindObjectOfType<LapSystem>().getMaxLap();

        //
        SmokeEffectEnabel = false;

        carRigibody = GetComponent<Rigidbody>();
        if(carRigibody != null )
        {
            carRigibody.centerOfMass = COM.localPosition;
        }


        
    }
    void Update()
    {
        getInput();
        CalculateCarMovoment();
        calculaterSteering();
        ApplyTransformToWheels();
    }

    public void getInput()
    {
        if(controlmode == ControlMode.KeyBoard)
        {
            horizotal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
        }
    }    
    public void CalculateCarMovoment()
    {
        carSpeed = carRigibody.velocity.magnitude;//lay ra van toc cua 
        carSpeedCoverted = Mathf.Round(carSpeed * 3.6f);
        // aply braking
        if(Input.GetKey(KeyCode.Space))
        {
            handBrake = true;

        }
        else
        {
            handBrake= false;
        }
        if(handBrake)
        {
            MotorTorque = 0f;
            EnabelTrailEffect(true);
            ApplyBrake();

            brakeLightLeftMat.SetColor("_EmissionColor", brakeColor);
            brakeLightRightMat.SetColor("_EmissionColor", brakeColor);

            if(!SmokeEffectEnabel) 
            {
                EnaBleSmokeEffect(true);
                SmokeEffectEnabel = true;

            }
        }
        else
        {
            if (vertical >= 0)
            {
                brakeLightLeftMat.SetColor("_EmissionColor", Color.black);
                brakeLightRightMat.SetColor("_EmissionColor", Color.black);
            }
            else
            {
                brakeLightLeftMat.SetColor("_EmissionColor", brakeColor);
                brakeLightRightMat.SetColor("_EmissionColor", brakeColor);
            }    
            RealeaBrake();
            EnabelTrailEffect(false);
            if (carSpeedCoverted < maximumSpeed)
            {
                MotorTorque = maximumMotoTorque * vertical;
               
            }
            else
            {
                MotorTorque = 0;
            }
            if (SmokeEffectEnabel)
            {
                EnaBleSmokeEffect(false);
                SmokeEffectEnabel = false;

            }
            //am thanh
            if (carSpeedCoverted > 0 || handBrake)
            {
                EngineSource.UnPause();

                float gearRatio = CurrentSpeed / maximumSpeed;
                int numberOfGears = 6;
                int currentGear = Mathf.Clamp(Mathf.FloorToInt(gearRatio * numberOfGears) + 1, 1, numberOfGears);

                float pitchMulitiplier = 0.5f + 0.5f * (carSpeedCoverted / maximumSpeed);
                float volumeMulitiplier = 0.2f + 0.8f * (carSpeedCoverted / maximumSpeed);

                EngineSource.pitch = Mathf.Lerp(0.5f, 1.0f, pitchMulitiplier) * currentGear;
                EngineSource.volume = volumeMulitiplier;
            }
            else
            {
                EngineSource.UnPause();
                EngineSource.pitch = 0.5f;
                EngineSource.volume = 0.2f;
            }

        }


        ApplyMororTorque();


    }    

    public void calculaterSteering()
    {
        tireAngel = maximumSteeringAngel * horizotal;
        FrontWheelLeftCollider.steerAngle = tireAngel;
        FrontWheelRightCollider.steerAngle = tireAngel;
    }    

    public void ApplyMororTorque()
    {
        if(cartype == CarType.FrontWheelDrive)
        {
            FrontWheelLeftCollider.motorTorque = MotorTorque;
            FrontWheelRightCollider.motorTorque = MotorTorque;
        }
        else if(cartype == CarType.RearWheelDrive)
        {
            BackWheelLeftCollider.motorTorque = MotorTorque;
            BackWheelRightCollider.motorTorque = MotorTorque;

        }
        else if(cartype == CarType.FourWheelDrive) {
            FrontWheelLeftCollider.motorTorque = MotorTorque;
            FrontWheelRightCollider.motorTorque = MotorTorque;
            BackWheelLeftCollider.motorTorque = MotorTorque;
            BackWheelRightCollider.motorTorque = MotorTorque;
        }

    }    
    public void ApplyBrake()
    {
        FrontWheelLeftCollider.brakeTorque = brakePower;
        BackWheelLeftCollider.brakeTorque = brakePower;
        FrontWheelRightCollider.brakeTorque = brakePower;
        BackWheelRightCollider.brakeTorque = brakePower;


    }
    public void RealeaBrake()
    {
        FrontWheelLeftCollider.brakeTorque = 0f;
        BackWheelLeftCollider.brakeTorque = 0f;
        FrontWheelRightCollider.brakeTorque = 0f;
        BackWheelRightCollider.brakeTorque = 0f;
    }    
    public void ApplyTransformToWheels()
    {
        Vector3 position;
        Quaternion rotation;
        FrontWheelLeftCollider.GetWorldPose(out position, out rotation);
        FrontWheelLeft.transform.position = position;
        FrontWheelLeft.transform.rotation = rotation;

        FrontWheelRightCollider.GetWorldPose(out position,out rotation);
        FrontWheelRight.transform.position = position;
        FrontWheelRight.transform.rotation = rotation;

        BackWheelLeftCollider.GetWorldPose(out position, out rotation);
        BackWheelLeft.transform.position = position;
        BackWheelLeft.transform.rotation = rotation;

        BackWheelRightCollider.GetWorldPose(out position, out rotation);
        BackWheelRight.transform.position = position;
        BackWheelRight.transform.rotation = rotation;
    }    

    public void EnaBleSmokeEffect(bool Enabel)
    {
        foreach(ParticleSystem smoke in SmokeEffect)
        {
            if(Enabel)
            {
                smoke.Play();

            }
            else
            {
                smoke.Stop();
            }
        }
    }

    public void EnabelTrailEffect(bool Enabel)
    {
        foreach(TrailRenderer trailRenderer in trailRenderers)
        {
            trailRenderer.emitting = Enabel;
        }    
    }
    // Update is called once per frame


    public void IncreaseLap()
    {
        CurrentLap++;
        Debug.Log("Car" + gameObject.name + "Lap" + CurrentLap);
    }


    public float getCurrentLap()
    {
        return CurrentLap;
    }    
}
