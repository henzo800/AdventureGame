using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public static InputController instance;
    public GameObject controlTarget;
    IControllable controller;
    public PlayerInput playerInput;
    public float LOOK_SENSETIVITY;
    void Awake() {
        if(instance != null){
            Debug.LogError("More than one InputController instance found");
        }
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        SetCharacter(controlTarget);
    }

    // Update is called once per frame
    void Update()
    {
        playerInput.camera.transform.position = controller.CameraMount.position;
        playerInput.camera.transform.rotation = controller.CameraMount.rotation;

        Interact();
        
    }

    public void SetCharacter(GameObject i_controller){
        if(controller != null){
            controller.IsControled = false;
        }

        controller = i_controller.GetComponent<IControllable>();
        controller.IsControled = true;
        controller.PlayerInput = playerInput;
    }

    IInteractable currentInteraction;
    void Interact() {
        RaycastHit hit;
        if(Physics.Raycast(playerInput.camera.transform.position, playerInput.camera.transform.forward, out hit, 10)){
            IInteractable interact = hit.collider.gameObject.GetComponent<IInteractable>();
            if(interact != null){
                if(currentInteraction != interact){
                    if(currentInteraction != null) {
                        currentInteraction.OnStopHover();
                    }
                    currentInteraction = interact;
                    interact.OnHover();
                }else{
                    interact.OnHover();
                }
                return;
            }
        }
        if(currentInteraction != null) {
            currentInteraction.OnStopHover();
        }
        currentInteraction = null;
        return;
    }

    void OnPrimary(){
        RaycastHit hit;
        if(Physics.Raycast(playerInput.camera.transform.position, playerInput.camera.transform.forward, out hit, 10)){
            IInteractable interact = hit.collider.gameObject.GetComponent<IInteractable>();
            if(interact != null){
                interact.OnPrimary();
            }
        }
    }
}
