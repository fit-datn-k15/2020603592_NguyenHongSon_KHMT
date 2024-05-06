using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private float MoveSmoothness;
    [SerializeField] private float RotationSmoothness;

    [SerializeField] private Vector3 moveOffset;
    [SerializeField] private Vector3 rotateOffset;

    [SerializeField] private Transform carTarget;



    private void LateUpdate()
    {
        
    }
    public void followTarget()
    {
        HandelMovement();
        HandleRotation();
      
    }    
    public void HandelMovement()
    {
        Vector3 targetPos = new Vector3();
        targetPos = carTarget.TransformPoint(moveOffset);
        transform.position = Vector3.Lerp(transform.position, targetPos, MoveSmoothness*Time.deltaTime);
    }

    public void HandleRotation()
    {
        var direction = carTarget.position - transform.position;
        var rotation = new Quaternion();
        rotation = Quaternion.LookRotation(direction+rotateOffset,Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation,rotation, RotationSmoothness*Time.deltaTime);

    }

    // Start is called before the first frame update
    void Start()
    {
        followTarget();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
