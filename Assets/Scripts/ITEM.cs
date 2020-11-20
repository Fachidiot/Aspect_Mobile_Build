using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITEM : MonoBehaviour
{
    private GameObject InputManager;

    private void Start()
    {
        InputManager = GameObject.Find("InputMgr");
    }

    public void Hint1()
    {
    }

    public void Hint2()
    {
        InputManager.GetComponent<InputMgr>().ShowHint();
    }

    public void Time()
    {

    }

    public void Blind1()
    {

    }

    public void Blind2()
    {

    }

    public void Pass()
    {

    }

    public void Quake()
    {

    }
}
