using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour, IControllable
{
    public List<WheelData> wheels;
    public float vehicleMotorTorque;
    public float vehicleBrakeTorque;
    public float targetSteeringAngle;
    public float downForceMultiplier;
    float steeringAngle;
    public float turnSpeed;
    public Transform cameraMount;
    public Transform CameraMount { get{return cameraMount;} }
    public PlayerInput PlayerInput { get; set; }
    public bool IsControled { get; set; }
    public float CENTER_OF_MASS_Y_MODIFIER;

    // Start is called before the first frame update
    void Start()
    {
        foreach(WheelData wheel in wheels){
            wheel.wheelCollider = wheel.rootObject.GetComponent<WheelCollider>();
            wheel.wheelMesh = wheel.rootObject.GetComponentInChildren<MeshRenderer>();
        }
        GetComponent<Rigidbody>().centerOfMass += -transform.up * CENTER_OF_MASS_Y_MODIFIER;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsControled) {
            steeringAngle = PlayerInput.actions["Move"].ReadValue<Vector2>().x * targetSteeringAngle;
            string data = "";
            foreach(WheelData wheel in wheels){
                //Steering
                wheel.wheelCollider.steerAngle = steeringAngle * wheel.steeringMultiplier;
                //Motor and Brakeing
                switch(PlayerInput.actions["Move"].ReadValue<Vector2>().y){
                    case 0:
                        wheel.wheelCollider.motorTorque = 0;
                        wheel.wheelCollider.brakeTorque = 0;
                        break;
                    case >0:
                        wheel.wheelCollider.motorTorque = PlayerInput.actions["Move"].ReadValue<Vector2>().y * vehicleMotorTorque * wheel.torqueMultiplier;
                        wheel.wheelCollider.brakeTorque = 0;
                        break;
                    case <0:
                        wheel.wheelCollider.brakeTorque = 0;
                        wheel.wheelCollider.brakeTorque = PlayerInput.actions["Move"].ReadValue<Vector2>().y * vehicleBrakeTorque * wheel.brakeMultiplier;
                        break;
                }
                wheel.wheelCollider.GetGroundHit(out WheelHit hit);
                data += " (" + hit.forwardSlip.ToString().PadRight(5)[..5] + ")";
                //Align mesh to wheel collider
                wheel.wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion quat);
                wheel.wheelMesh.transform.position = pos;
                wheel.wheelMesh.transform.rotation = quat;
            }
            Debug.Log(data);
        }

    }

    void FixedUpdate() {
        if(IsControled){
            mouseLook();
        }
        DownForce();
    }
    void DownForce(){
        GetComponent<Rigidbody>().AddForce( -transform.up * GetComponent<Rigidbody>().GetPointVelocity(transform.position).magnitude *downForceMultiplier, ForceMode.VelocityChange);
        Debug.DrawRay(transform.position, -transform.up * GetComponent<Rigidbody>().GetPointVelocity(transform.position).magnitude *downForceMultiplier, Color.red, 10);
        Debug.DrawRay(transform.position, GetComponent<Rigidbody>().GetPointVelocity(transform.position), Color.blue);
    }
    public void mouseLook() {
        Vector2 lookDelta = PlayerInput.actions["Look"].ReadValue<Vector2>() * InputController.instance.LOOK_SENSETIVITY * Time.fixedDeltaTime;
        cameraMount.localEulerAngles += new Vector3(-lookDelta.y, lookDelta.x, 0);
        cameraMount.localEulerAngles = new Vector3(ClampAngle(cameraMount.localEulerAngles.x, -60f, 60f), ClampAngle(cameraMount.localEulerAngles.y, -60f, 60f), 0f);
    }
    public static float ClampAngle(float current, float min, float max)
    {
        float dtAngle = Mathf.Abs(((min - max) + 180) % 360 - 180);
        float hdtAngle = dtAngle * 0.5f;
        float midAngle = min + hdtAngle;
    
        float offset = Mathf.Abs(Mathf.DeltaAngle(current, midAngle)) - hdtAngle;
        if (offset > 0)
            current = Mathf.MoveTowardsAngle(current, midAngle, offset);
        return current;
    }

}
