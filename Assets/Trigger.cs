using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public ButtonVR buttonVR;
    private bool isCoolDown;

    private void Start(){
        buttonVR = GetComponentInParent<ButtonVR>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finger") && !buttonVR.isPressed && !isCoolDown)
        {
            buttonVR.presser = other.gameObject;
            buttonVR.isPressed = true;
            isCoolDown = true;
            StartCoroutine(CoolDown());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == buttonVR.presser)
        {
            buttonVR.ResetButton();
        }
    }

    private IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(0.5f);
        isCoolDown = false;
    }
}
