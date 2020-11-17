using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Range(5, 30)]
    public int Count;
    [Range(30, 480)]
    public int Time;

    public InputMgr InputManager;
    public Text AssistantOn;
    public Text AssistantOff;
    
    private bool m_bAssistant = true;

    public void OnAssist()
    {
        if(!m_bAssistant)
        {
            InputManager.Assistant = true;
            AssistantOn.gameObject.SetActive(true);
            Debug.Log("Assistant On");
            m_bAssistant = true;
        }
        else
        {
            InputManager.Assistant = false;
            AssistantOff.gameObject.SetActive(true);
            Debug.Log("Assistant Off");
            m_bAssistant = false;
        }
    }

    public void Exit()
    { // 게임 종료
        Application.Quit();
    }
}
