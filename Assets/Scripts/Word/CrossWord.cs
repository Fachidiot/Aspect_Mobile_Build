using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossWord : MonoBehaviour
{
    private static List<Word> m_OldList;
    private static List<Word> m_NewList;

    private int[] m_UsedIndex;

    public bool Cross()
    {
        if(m_NewList.Count <= 0)
        {
            return false;
        }
        return true;
    }

    public void SetRandomList(int _count)
    {
        if (m_OldList == null)
        {
            SetList();
        }

        // 기존 리스트는 삭제
        m_NewList = new List<Word>();

        // 랜덤으로 단어를 뽑아온다.
        List<int> temp_int = new List<int>();
        for (int i = 0; i < _count; i++)
        {
            temp_int.Add(Random.Range(0, m_OldList.Count - 1));
            bool single = true;

            for (int j = 0; j < i; j++)
            {
                if (temp_int[i] == temp_int[j])
                {
                    i--;
                    temp_int.Remove(temp_int[i]);
                    single = false;
                }
            }

            if (single)
            {
                m_NewList.Add(m_OldList[temp_int[i]]);
            }
        }

        if (m_NewList.Count > 0)
        { // 크로싱 작업
            for (int i = 0; i < m_NewList.Count; i++)
            {
                CheckCross(i);
            }
        }

        var temp = GameObject.Find("WordMgr");
        temp.GetComponent<WordMgr>().SetCount(m_NewList.Count);
    }

    private string getPath()
    {
#if UNITY_EDITOR
        return Application.dataPath + "/Resource/CrossInfo.csv";
#elif UNITY_ANDROID
        return Application.persistentDataPath+"CrossInfo.csv";
#elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+"CrossInfo.csv";
#else
        return Application.dataPath +"/"+"CrossInfo.csv";
#endif
    }


    // 체크용
    void print()
    {
        for (int i = 0; i < m_NewList.Count; i++)
        {
            Debug.Log(m_NewList[i].Answer);
            for (int j = 0; j < m_NewList[i].IndexInfo.Count; j++)
            {
                for (int k = 0; k < m_NewList[i].IndexInfo[j].Count; k++)
                {
                    Debug.Log(m_NewList[i].IndexInfo[j][k]);
                }
            }
        }
    }

    // 단어장 가져오기
    void SetList()
    {
        m_OldList = csvReader_ver2.GetList();
        if (m_OldList.Count <= 0)
        {
            Debug.Log("No Dictionary");
            return;
        }
        m_UsedIndex = new int[m_OldList.Count];
    }

    // 교차단어를 체크
    void CheckCross(int index)
    {
        for (int i = 0; i < m_NewList.Count; i++)
        {
            // 자기 자신을 제외
            if (i == index)
                continue;

            // 해당 단어와 교차하는 단어를 저장
            for (int j = 0; j < m_NewList[i].Length; j++)
            {
                if (m_NewList[index].Answer.Contains(m_NewList[i].Answer[j].ToString()))
                { // 해당 단어와 교차되는 단어일때
                    var temp_int = m_NewList[index].FindAlphabetCount(m_NewList[i].Answer[j].ToString());
                    if (temp_int <= 0)
                    {
                        // 오류
                        continue;
                    }
                    else if(temp_int == 1)
                    { // 맞는 알파벳이 1개일때
                        var temp = m_NewList[index].FindAlphabetIndex(m_NewList[i].Answer[j].ToString());
                        // 해당 리스트인덱스에 string리스트를 추가해준다.
                        // Word, 1, 2, false
                        var temp_string = m_NewList[i].Answer + ", " + temp + ", " + j;
                        m_NewList[index].IndexInfo[temp].Add(temp_string);
                    }
                    else
                    { // 2개 이상일때
                        for (int k = 0; k < temp_int; k++)
                        {
                            var temp = m_NewList[index].FindAlphabetIndexGreater(m_NewList[i].Answer[j].ToString(), k);
                            // 해당 리스트인덱스에 string리스트를 추가해준다.
                            // Word, 1, 2, false
                            var temp_string = m_NewList[i].Answer + ", " + temp + ", " + j;
                            m_NewList[index].IndexInfo[temp].Add(temp_string);
                        }
                    }
                }
            }
        }
    }

    public List<Word> GetList()
    {
        if(m_NewList == null)
        {
            return null;
        }
        var temp = m_NewList.ToList();
        return temp;
    }

    public void Reset()
    {
        for (int i = 0; i < m_NewList.Count; i++)
        {
            for (int j = 0; j < m_NewList[i].m_bIsOpen.Length; j++)
            {
                m_NewList[i].m_bIsOpen[j] = true;
            }
        }
    }



    //string filePath = getPath();

    //System.IO.StreamReader sr = new System.IO.StreamReader(filePath);
    //if (sr.BaseStream.Length == 0)
    //{
    //    System.Text.StringBuilder sb = new System.Text.StringBuilder();

    //    for (int index = 0; index < m_NewList.Count; index++)
    //    {
    //        sb.AppendLine(m_NewList[index].Answer);
    //        for (int i = 0; i < m_NewList[index].IndexInfo.Count; i++)
    //        {
    //            for (int j = 0; j < m_NewList[index].IndexInfo[i].Count; j++)
    //            {
    //                sb.AppendLine(m_NewList[index].IndexInfo[i][j]);
    //            }
    //        }
    //    }

    //    sr.Close();

    //    System.IO.StreamWriter sw = new System.IO.StreamWriter(filePath);
    //    sw.WriteLine(sb);
    //    sw.Close();
    //}
}
