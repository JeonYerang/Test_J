using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIPanel : MonoBehaviour
{
    [SerializeField]
    FixedJoystick joystick;
    public FixedJoystick Joystick {  get; private set; }
}
