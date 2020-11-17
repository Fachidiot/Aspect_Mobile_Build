using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMgr : MonoBehaviour
{
    public int MaxCount;

    private int m_ItemSelectCount = 0;

    void Start()
    {
        m_ItemSelectCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Select()
    {
        if (m_ItemSelectCount >= MaxCount)
        {
            return;
        }
        m_ItemSelectCount++;
    }
}
