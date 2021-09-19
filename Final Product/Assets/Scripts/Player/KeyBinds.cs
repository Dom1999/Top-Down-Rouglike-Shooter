using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KeyBinds : MonoBehaviour
{
    //MOVEMENT
    public static KeyCode UP_KEY = KeyCode.W;
    public static KeyCode DOWN_KEY = KeyCode.S;
    public static KeyCode LEFT_KEY = KeyCode.A;
    public static KeyCode RIGHT_KEY = KeyCode.D;
    public static KeyCode SPRINT_KEY = KeyCode.LeftShift;
    public static KeyCode USE_KEY = KeyCode.E;

    //MENUS
    public static KeyCode INVENTORY_KEY = KeyCode.I;


    //OTHER
    public static KeyCode QUICK_SAVE_KEY = KeyCode.F5;
    public static KeyCode QUICK_LOAD_KEY = KeyCode.F9;
}
