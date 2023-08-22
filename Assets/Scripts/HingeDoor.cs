using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingeDoor : MonoBehaviour, IInteractable
{
    new HingeJoint hingeJoint;
    Outline outline;
    public bool IsOpen;
    public float OpenVelocity;
    public float CloseVelocity;
    public void OnHover()
    {
        outline.enabled = true;
    }
    public void OnStopHover()
    {
        outline.enabled = false;
    }
    public void OnPrimary()
    {
        if(IsOpen){
            IsOpen = false;
        }else{
            IsOpen = true;
        }
    }

    public void OnSecondary()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<Outline>();
        hingeJoint = GetComponent<HingeJoint>();

        outline.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsOpen) {
            JointMotor motor = hingeJoint.motor;
            motor.targetVelocity = OpenVelocity;
            hingeJoint.motor = motor;
        }else{
            JointMotor motor = hingeJoint.motor;
            motor.targetVelocity = CloseVelocity;
            hingeJoint.motor = motor;
        }
    }
}
