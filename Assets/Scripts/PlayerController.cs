using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IControllable
{
    public float MOVEMENT_SPEED;
    public Transform viewRoot;
    public Transform CameraMount { get { return viewRoot; } }
    public PlayerInput PlayerInput { get; set; }
    public bool IsControled { get; set; }

    void Awake() {
        
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(IsControled) {
            transform.position += transform.forward * PlayerInput.actions["Move"].ReadValue<Vector2>().y * MOVEMENT_SPEED * Time.fixedDeltaTime;
            transform.position += transform.right * PlayerInput.actions["Move"].ReadValue<Vector2>().x * MOVEMENT_SPEED * Time.fixedDeltaTime;

            mouseLook();
        }
    }

    public void mouseLook() {
        Vector2 lookDelta = PlayerInput.actions["Look"].ReadValue<Vector2>() * InputController.instance.LOOK_SENSETIVITY * Time.fixedDeltaTime;
        transform.eulerAngles += new Vector3(0, lookDelta.x, 0);
        viewRoot.localEulerAngles += new Vector3(-lookDelta.y, 0f, 0f);
        viewRoot.localEulerAngles = new Vector3(ClampAngle(viewRoot.eulerAngles.x, -60f, 60f), 0f, 0f);
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
