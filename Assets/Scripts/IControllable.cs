using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IControllable
{
    Transform CameraMount { get; }
    PlayerInput PlayerInput { get; set; }
    bool IsControled { get; set; }
}
