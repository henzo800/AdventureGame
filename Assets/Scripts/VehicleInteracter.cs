using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleInteracter : MonoBehaviour, IInteractable
{
    public GameObject VehicleRoot;
    Outline outline;
    void Awake() {
        outline = GetComponent<Outline>();
    }
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
        InputController.instance.SetCharacter(VehicleRoot);
    }

    public void OnSecondary()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
