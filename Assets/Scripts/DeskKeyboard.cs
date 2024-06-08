using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DeskKeyboard : MonoBehaviour
{
    [Header("Prefab Settings")] 
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private List<ButtonVR> buttons = new List<ButtonVR>();

    [SerializeField] private UnityEvent<string> onInsertChar;
    
    private bool isCapital;


    void Start()
    {
        isCapital = false;
    }

    public void InsertChar(string character)
    {
        inputField.text += character;
        onInsertChar?.Invoke(character);
    }

    public void Backspace()
    {
        if (inputField.text.Length > 0)
        {
            inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
        }
    }

    public void Space()
    {
        inputField.text += " ";
    }

    public void Capital(bool state)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].SetCapital(state);
        }
    }
}
