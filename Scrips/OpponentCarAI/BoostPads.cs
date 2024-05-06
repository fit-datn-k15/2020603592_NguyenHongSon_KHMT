using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPads : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        OpponentCar opponentCar = other.GetComponent<OpponentCar>();
        if (opponentCar != null )
        {
            opponentCar.SetAcceleration(Random.Range(4f, 5f));
            opponentCar.SetCurrentSpeed(Random.Range(35f, 41f));

        }
    }
}
