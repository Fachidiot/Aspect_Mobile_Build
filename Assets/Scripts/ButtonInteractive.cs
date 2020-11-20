using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInteractive : MonoBehaviour
{
    public void InteractiveOn()
    {
        gameObject.GetComponent<Button>().interactable = true;
    }
}
