using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBreaker : MonoBehaviour
{
    // Start is called before the first frame update
    public float durationOfRedution = 1f;
    private void OnTriggerEnter(Collider other)
    {
        OpponentCar opponentCar = other.GetComponent<OpponentCar>();
        if (opponentCar != null )
        {
            opponentCar.SetAcceleration(Random.Range(0.5f,1f));
            opponentCar.SetCurrentSpeed(Random.Range(20f, 22));
            StartCoroutine(ResetAcceleration(opponentCar));
            
        }
    }
    IEnumerator ResetAcceleration(OpponentCar opponentCar)
    {
        yield return new WaitForSeconds(durationOfRedution);
        opponentCar.resetAccleration();
    }
}
