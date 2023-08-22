using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WheelData{
    public GameObject rootObject;
    public WheelCollider wheelCollider;
    public MeshRenderer wheelMesh;
    public float torqueMultiplier;
    public float steeringMultiplier;
    public float brakeMultiplier;
}