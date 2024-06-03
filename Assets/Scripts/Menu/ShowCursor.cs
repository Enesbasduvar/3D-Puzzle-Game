using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCursor : MonoBehaviour
{
    [Header("Keybinds")]
    public KeyCode showCursor = KeyCode.RightShift;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(showCursor))
        { 
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        }
        else if (Input.GetKeyUp(KeyCode.C)|| Input.GetKeyUp(showCursor))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
