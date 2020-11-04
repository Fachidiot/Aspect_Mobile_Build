﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class csvReader_ver2 : MonoBehaviour
{
    public string str_Filename;

    private static string FileName;
    private const string m_Default_Filename = "Resource/CatchitWord.csv";
    private static List<Word> m_WordList = new List<Word>();

    void Awake()
    {
        ReadData(str_Filename);
    }

    public bool ReadData(string str_name = m_Default_Filename)
    {
        StreamReader sr = new StreamReader(Application.dataPath + "/" + str_name);

        if (sr.Peek() == 0)
            Debug.LogError("File doesn't exist");

        //=========================================
        bool m_bIsEnd = false;
        bool m_bExample = true;

        Word tempword = new Word();

        while (!m_bIsEnd)
        {
            string str_Data = sr.ReadLine();
            if (str_Data == null)
            {
                m_bIsEnd = true;
                break;
            }

            if (m_bExample)
            { // 첫번째는 무조건 건너뜀
                m_bExample = false;
                continue;
            }
            else
            {
                var var_Value = str_Data.Split(',');

                var_Value[1] = var_Value[1].TrimStart();
                var_Value[2] = var_Value[2].Replace("/", ",").TrimStart();
                tempword = new Word(var_Value[1].ToUpper(), var_Value[2].ToUpper());
                m_WordList.Add(tempword);
                //Debug.Log(m_WordList.Last.Value.Answer + "추가 완료");
            }
        }

        if (m_WordList.Count <= 0)
            Debug.LogError("Empty reading list");

        Debug.Log("Successfully reading data");
        return true;
    }

    public static List<Word> GetList()
    {
        return m_WordList;
    }

    public void Link()
    {
        LinkedList<Word> TempList = new LinkedList<Word>();
        foreach (var item in m_WordList)
        {
        }
    }
}
