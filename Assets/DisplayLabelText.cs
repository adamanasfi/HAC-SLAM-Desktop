using UnityEngine;
using TMPro; // Make sure you have the TextMeshPro namespace included

public class DisplayLabelText : MonoBehaviour
{
    // Reference to the TextMeshPro component
    public TextMeshPro textDisplay;

    // This will store the current text input
    private string currentText = "";

    void Start()
    {
        textDisplay = GetComponent<TextMeshPro>();
    }

    void Update()
    {
        this.textDisplay.text = currentText;
        // Iterate through all the possible characters (A-Z, 0-9, etc.)
        foreach (char c in Input.inputString)
        {
            // Handle backspace
            if (c == '\b' && currentText.Length > 0)
            {
                // Remove the last character
                currentText = currentText.Substring(0, currentText.Length - 1);
            }
            // Handle enter/return key (new line)
            else if (c == '\n' || c == '\r')
            {
                // Do nothing or add a new line if required
                currentText += "\n";
            }
            // Handle regular input
            else
            {
                currentText += c; // Add the character to the current text
            }

            // Update the TextMeshPro component with the current text
            textDisplay.text = currentText;
        }
    }
}
