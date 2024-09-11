using UnityEngine;

public class IPDesktop : MonoBehaviour
{
    public NumberPadInput numberPadInput;

    // Update is called once per frame
    void Update()
    {
        // Check for key presses
        if (Input.GetKeyDown(KeyCode.Alpha0)) // Key "0"
        {
            HandleKeyPress("0");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1)) // Key "1"
        {
            HandleKeyPress("1");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) // Key "2"
        {
            HandleKeyPress("2");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) // Key "3"
        {
            HandleKeyPress("3");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) // Key "4"
        {
            HandleKeyPress("4");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5)) // Key "5"
        {
            HandleKeyPress("5");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6)) // Key "6"
        {
            HandleKeyPress("6");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7)) // Key "7"
        {
            HandleKeyPress("7");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8)) // Key "8"
        {
            HandleKeyPress("8");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9)) // Key "9"
        {
            HandleKeyPress("9");
        }
        else if (Input.GetKeyDown(KeyCode.Return)) // Enter key
        {
            HandleKeyPress("Enter");
        }
        else if (Input.GetKeyDown(KeyCode.Delete)) // Clear key
        {
            HandleKeyPress("Clear");
        }
        else if (Input.GetKeyDown(KeyCode.Backspace)) // Backspace key
        {
            HandleKeyPress("Backspace");
        }
        else if (Input.GetKeyDown(KeyCode.Period)) // Period key (.)
        {
            HandleKeyPress(".");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad0)) // Key "0" on number pad
        {
            HandleKeyPress("0");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad1)) // Key "1" on number pad
        {
            HandleKeyPress("1");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2)) // Key "2" on number pad
        {
            HandleKeyPress("2");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3)) // Key "3" on number pad
        {
            HandleKeyPress("3");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4)) // Key "4" on number pad
        {
            HandleKeyPress("4");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad5)) // Key "5" on number pad
        {
            HandleKeyPress("5");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad6)) // Key "6" on number pad
        {
            HandleKeyPress("6");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad7)) // Key "7" on number pad
        {
            HandleKeyPress("7");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad8)) // Key "8" on number pad
        {
            HandleKeyPress("8");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad9)) // Key "9" on number pad
        {
            HandleKeyPress("9");
        }
        else if (Input.GetKeyDown(KeyCode.KeypadEnter)) // Enter key on number pad
        {
            HandleKeyPress("Enter");
        }
        else if (Input.GetKeyDown(KeyCode.KeypadPeriod)) // Period key on number pad (.)
        {
            HandleKeyPress(".");
        }
    }

    private void HandleKeyPress(string keyName)
    {
        // Ensure numberPadInput is not null
        if (numberPadInput != null)
        {
            numberPadInput.OnKeyPressedEvent(keyName);
        }
    }
}
