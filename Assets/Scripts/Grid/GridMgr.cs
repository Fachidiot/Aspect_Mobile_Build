using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GridMgr : MonoBehaviour
{
    public string str_name;
    public GameObject GridPrefab;

    private GridInfo tempGrid;
    private Word m_TempWord;
    private List<Word> m_WordList;
    private List<Word> m_MainList;
    // Count Word List
    private List<Word> m_3List = new List<Word>();
    private List<Word> m_4List = new List<Word>();
    private List<Word> m_5List = new List<Word>();
    private List<Word> m_6List = new List<Word>();
    private List<Word> m_7List = new List<Word>();
    private List<Word> m_8List = new List<Word>();
    private List<Word> m_9List = new List<Word>();
    private List<Word> m_10List = new List<Word>();
    private List<Word> m_11List = new List<Word>();
    private List<Word> m_12List = new List<Word>();
    private List<Word> m_13List = new List<Word>();
    private List<Word> m_14List = new List<Word>();
    private bool[,] temp = new bool[5, 5];
    private List<GameObject> m_ObjList = new List<GameObject>();

    private void Start()
    {        
        //Debug.Log(m_3List.Count);
        //Debug.Log(m_4List.Count);
        //Debug.Log(m_5List.Count);
        //Debug.Log(m_6List.Count);
        //Debug.Log(m_7List.Count);
        //Debug.Log(m_8List.Count);
        //Debug.Log(m_9List.Count);
        //Debug.Log(m_10List.Count);
        //Debug.Log(m_11List.Count);
        //Debug.Log(m_12List.Count);
        //Debug.Log(m_13List.Count);
        //Debug.Log(m_14List.Count);
    }

    void ListSet()
    {
        m_WordList = csvReader_ver2.GetList();
        for (int i = 0; i < m_WordList.Count; i++)
        {
            switch (m_WordList[i].Length)
            {
                case 3:
                    m_3List.Add(m_WordList[i]);
                    break;
                case 4:
                    m_4List.Add(m_WordList[i]);
                    break;
                case 5:
                    m_5List.Add(m_WordList[i]);
                    break;
                case 6:
                    m_6List.Add(m_WordList[i]);
                    break;
                case 7:
                    m_7List.Add(m_WordList[i]);
                    break;
                case 8:
                    m_8List.Add(m_WordList[i]);
                    break;
                case 9:
                    m_9List.Add(m_WordList[i]);
                    break;
                case 10:
                    m_10List.Add(m_WordList[i]);
                    break;
                case 11:
                    m_11List.Add(m_WordList[i]);
                    break;
                case 12:
                    m_12List.Add(m_WordList[i]);
                    break;
                case 13:
                    m_13List.Add(m_WordList[i]);
                    break;
                case 14:
                    m_14List.Add(m_WordList[i]);
                    break;
                default:
                    Debug.LogError("새로운 리스트 생성필요");
                    break;
            }
        }

    }

    void SaveMapData()
    {
        StreamWriter sw = new StreamWriter(Application.dataPath + "/" + str_name);

        string temp_string = "";
        sw.WriteLine("Grid");
        sw.WriteLine(5);
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (j == 0)
                {
                    temp_string = tempGrid.Grid[i, j].ToString();
                    continue;
                }
                temp_string = string.Join(",", temp_string, tempGrid.Grid[i, j]);
            }
            sw.WriteLine(temp_string);
            temp_string = "";
        }
        sw.WriteLine("WordLengthList");
        temp_string = "";
        for (int i = 0; i < tempGrid.WordLenghtList.Count; i++)
        {
            if (i == 0)
            {
                temp_string = tempGrid.WordLenghtList[i].ToString();
                continue;
            }
            temp_string = string.Join(",", temp_string, tempGrid.WordLenghtList[i]);
        }
        sw.WriteLine(temp_string);
        sw.WriteLine("WordCrossList");
        temp_string = "";
        for (int i = 0; i < tempGrid.WordCrossList.Count; i++)
        {
            if (i == 0)
            {
                temp_string = tempGrid.WordCrossList[i].ToString();
                continue;
            }
            temp_string = string.Join(",", temp_string, tempGrid.WordCrossList[i]);
        }
        sw.WriteLine(temp_string);
        sw.Flush();
        sw.Close();
    }
    GridInfo LoadMapData(string path)
    {
        StreamReader sr = new StreamReader(Application.dataPath + "/" + path);

        if (0 == sr.Peek())
        {
            Debug.LogError("File doesn't exist");
        }
        bool m_bIsEnd = false;
        int WordCount = 0;

        GridInfo temp_grid = new GridInfo();

        while (!m_bIsEnd)
        {
            string str_Data = sr.ReadLine();
            if (str_Data == null)
            {
                m_bIsEnd = true;
                break;
            }
            switch (str_Data)
            {
                case "Grid":
                    str_Data = sr.ReadLine();
                    WordCount = int.Parse(str_Data);
                    temp_grid = new GridInfo(WordCount, WordCount);
                    for (int i = 0; i < WordCount; i++)
                    {
                        str_Data = sr.ReadLine();
                        var var_Value1 = str_Data.Split(',');
                        for (int j = 0; j < WordCount; j++)
                        {
                            temp_grid.Grid[i, j] = var_Value1[j];
                        }
                    }
                    break;
                case "WordLengthList":
                    str_Data = sr.ReadLine();
                    var var_Value2 = str_Data.Split(',');
                    for (int i = 0; i < WordCount; i++)
                    {
                        temp_grid.WordLenghtList.Add(int.Parse(var_Value2[i]));
                    }
                    break;
                case "WordCrossList":
                    str_Data = sr.ReadLine();
                    var var_Value3 = str_Data.Split(',');
                    for (int i = 0; i < WordCount; i++)
                    {
                        temp_grid.WordCrossList.Add(var_Value3[i]);
                    }
                    break;
                default:
                    Debug.LogWarning("예상치 못한 데이터 ㄴㅇㄱ");
                    break;
            }
        }

        tempGrid = temp_grid;
        Debug.Log("Successfully reading data");
        return temp_grid;
    }

    void PushData()
    {
        ListSet();
        m_MainList = new List<Word>();
        char[] PrevCross = new char[0];
        int[] PrevIndex = new int[0];
        for (int i = 0; i < tempGrid.WordLenghtList.Count; i++)
        {
            bool jump = false;
            var TempList = GetList(tempGrid.WordLenghtList[i]);
            var CrossInfo = tempGrid.WordCrossList[i].Split('/');
            if (PrevCross.Length != 0)
            {
                int count = 0;
                for (int j = 0; j < TempList.Count; j++)
                {
                    count = 0;
                    if(PrevCross.Length == 1)
                    {
                        if (TempList[j].Answer[PrevIndex[0]] == PrevCross[0]) // 3개 이상 겹칠때는 예외처리 필요함
                        {
                            count++;
                        }

                        if (count != 0)
                        { // 맞는 단어를 찾음
                            m_MainList.Add(TempList[j]);
                            TempList.Remove(TempList[j]);
                            break;
                        }
                        else
                        {
                            count = 0;
                        }
                    }
                    else if(PrevCross.Length >= 2)
                    { // 3개 이상부터
                        if (TempList[j].Answer[PrevIndex[0]] == PrevCross[0]) // 3개 이상 겹칠때는 예외처리 필요함
                        {
                            count++;
                        }

                        if (count != 0)
                        { // 맞는 단어를 찾음
                            m_MainList.Add(TempList[j]);
                            TempList.Remove(TempList[j]);
                            var temp1 = PrevCross[1];
                            PrevCross = new char[PrevCross.Length - 1];
                            PrevCross[0] = temp1;
                            var temp2 = PrevIndex[1];
                            PrevIndex = new int[PrevIndex.Length - 1];
                            PrevIndex[0] = temp2;
                            jump = true;
                            break;
                        }
                        else
                        {
                            count = 0;
                        }
                    }
                }
                if(jump)
                {
                    continue;
                }
                else if (count != 0)
                {
                    if (m_MainList.Count == tempGrid.WordLenghtList.Count)
                    {
                        break;
                    }


                    if (CrossInfo.Length == 1)
                    {
                        PrevCross = new char[0];
                        PrevIndex = new int[0];
                        continue;
                    }
                    if (CrossInfo.Length == 2)
                    { // 2개 겹침
                        if(int.Parse(CrossInfo[0]) < 0)
                        {
                            PrevCross[0] = m_MainList[m_MainList.Count - 1].Answer[int.Parse(CrossInfo[0])];
                        }
                        else
                        {
                            PrevCross[0] = m_MainList[m_MainList.Count - 1].Answer[int.Parse(CrossInfo[1])];
                        }
                        CrossInfo = tempGrid.WordCrossList[i + 1].Split('/');
                        if(int.Parse(CrossInfo[0]) < 0)
                        {
                            PrevIndex[0] = int.Parse(CrossInfo[1]);
                            continue;
                        }
                        PrevIndex[0] = int.Parse(CrossInfo[0]);
                        continue;
                    }
                    if (CrossInfo.Length == 3)
                    { // 3개 겹침
                        PrevCross[0] = m_MainList[m_MainList.Count - 1].Answer[int.Parse(CrossInfo[1])];
                        PrevCross[1] = m_MainList[m_MainList.Count - 1].Answer[int.Parse(CrossInfo[2])];
                        CrossInfo = tempGrid.WordCrossList[i + 1].Split('/');
                        if (int.Parse(CrossInfo[0]) < 0)
                        {
                            PrevIndex[0] = int.Parse(CrossInfo[2]);
                            continue;
                        }
                        PrevIndex[0] = int.Parse(CrossInfo[0]);
                        CrossInfo = tempGrid.WordCrossList[i + 2].Split('/');
                        PrevIndex[1] = int.Parse(CrossInfo[0]);
                        continue;
                    }

                }
                else
                { // 다 돌았지만 완성되지 않았을때
                    m_MainList.RemoveRange(0, m_MainList.Count);
                    PrevCross = new char[0];
                    PrevIndex = new int[0];
                    i = -1;
                    continue;
                }
            }

            if(CrossInfo.Length == 1)
            {
                if(int.Parse(CrossInfo[0]) == -1)
                { // 겹치지 않음
                    int index = Random.Range(0, TempList.Count);
                    m_MainList.Add(TempList[index]);
                    TempList.Remove(TempList[index]);
                    PrevCross = new char[0];
                    PrevIndex = new int[0];
                    continue;
                }
                else
                { // 한개만 겹침
                    int index = Random.Range(0, TempList.Count);
                    m_MainList.Add(TempList[index]);
                    TempList.Remove(TempList[index]);
                    PrevCross = new char[CrossInfo.Length];
                    PrevCross[0] = m_MainList[m_MainList.Count - 1].Answer[int.Parse(CrossInfo[0])];
                    PrevIndex = new int[CrossInfo.Length];
                    CrossInfo = tempGrid.WordCrossList[i + 1].Split('/');
                    PrevIndex[0] = int.Parse(CrossInfo[0]);
                    continue;
                }
            }
            if(CrossInfo.Length == 2)
            { // 2개 겹침
                int index = Random.Range(0, TempList.Count);
                m_MainList.Add(TempList[index]);
                TempList.Remove(TempList[index]);
                PrevCross = new char[CrossInfo.Length];
                PrevCross[0] = m_MainList[m_MainList.Count - 1].Answer[int.Parse(CrossInfo[0])];
                PrevCross[1] = m_MainList[m_MainList.Count - 1].Answer[int.Parse(CrossInfo[1])];
                CrossInfo = tempGrid.WordCrossList[i + 1].Split('/');
                PrevIndex[0] = int.Parse(CrossInfo[0]);
                CrossInfo = tempGrid.WordCrossList[i + 2].Split('/');
                PrevIndex[1] = int.Parse(CrossInfo[0]);
                continue;
            }
            if (CrossInfo.Length == 3)
            { // 3개 겹침
                int index = Random.Range(0, TempList.Count);
                m_MainList.Add(TempList[index]);
                TempList.Remove(TempList[index]);
                PrevCross = new char[CrossInfo.Length];
                PrevCross[0] = m_MainList[m_MainList.Count - 1].Answer[int.Parse(CrossInfo[0])];
                PrevCross[1] = m_MainList[m_MainList.Count - 1].Answer[int.Parse(CrossInfo[1])];
                PrevCross[2] = m_MainList[m_MainList.Count - 1].Answer[int.Parse(CrossInfo[2])];
                PrevIndex = new int[CrossInfo.Length];
                CrossInfo = tempGrid.WordCrossList[i + 1].Split('/');
                PrevIndex[0] = int.Parse(CrossInfo[0]);
                CrossInfo = tempGrid.WordCrossList[i + 2].Split('/');
                PrevIndex[1] = int.Parse(CrossInfo[0]);
                CrossInfo = tempGrid.WordCrossList[i + 3].Split('/');
                PrevIndex[1] = int.Parse(CrossInfo[0]);
                continue;
            }
        }

        for (int i = 0; i < m_MainList.Count; i++)
        {
            Debug.Log(m_MainList[i].Answer);
        }

        int temp_cross = 0;
        for (int i = 0; i < m_MainList.Count; i++)
        {
            int count = 0;
            if (temp_cross != 0)
            {
                count++;
                temp_cross = 0;
            }
            for (int j = 0; j < m_ObjList.Count; j++)
            {
                var var_int = m_ObjList[j].name.Split('&');
                if (var_int.Length == 1)
                {
                    if (var_int[0] == (i + 1).ToString())
                    {
                        m_ObjList[j].name = m_MainList[i].Answer[count].ToString();
                        count++;
                    }
                }
                else
                {
                    if (var_int[0] == (i + 1).ToString())
                    {
                        m_ObjList[j].name = m_MainList[i].Answer[count].ToString();
                        temp_cross = int.Parse(var_int[1]);
                        count++;
                    }
                }
            }
        }
    }

    List<Word> GetList(int i)
    {
        switch (i)
        {
            case 3:
                return m_3List;
            case 4:
                return m_4List;
            case 5:
                return m_5List;
            case 6:
                return m_6List;
            case 7:
                return m_7List;
            case 8:
                return m_8List;
            case 9:
                return m_9List;
            case 10:
                return m_10List;
            case 11:
                return m_11List;
            case 12:
                return m_12List;
            case 13:
                return m_13List;
            case 14:
                return m_14List;
            default:
                Debug.LogError("ㄴㅇㄱ");
                return null;
        }
    }

    List<Word> PeekList(int length)
    {
        List<Word> temp = new List<Word>();
        for (int i = 0; i < m_WordList.Count; i++)
        {
            if(m_WordList[i].Length == length)
            {
                temp.Add(m_WordList[i]);
            }
        }

        return temp;
    }

    // 교차단어를 체크
    void CheckCross(int index)
    {
        for (int i = 0; i < m_WordList.Count; i++)
        {
            // 자기 자신을 제외
            if (i == index)
                continue;

            // 해당 단어와 교차하는 단어를 저장
            for (int j = 0; j < m_WordList[i].Length; j++)
            {
                if (m_WordList[index].Answer.Contains(m_WordList[i].Answer[j].ToString()))
                { // 해당 단어와 교차되는 단어일때
                    var temp_int = m_WordList[index].FindAlphabetCount(m_WordList[i].Answer[j].ToString());
                    if (temp_int <= 0)
                    {
                        // 오류
                        continue;
                    }
                    else if (temp_int == 1)
                    { // 맞는 알파벳이 1개일때
                        var temp = m_WordList[index].FindAlphabetIndex(m_WordList[i].Answer[j].ToString());
                        // 해당 리스트인덱스에 string리스트를 추가해준다.
                        // Word, 1, 2, false
                        var temp_string = m_WordList[i].Answer + ", " + temp + ", " + j;
                        m_WordList[index].IndexInfo[temp].Add(temp_string);
                    }
                    else
                    { // 2개 이상일때
                        for (int k = 0; k < temp_int; k++)
                        {
                            var temp = m_WordList[index].FindAlphabetIndexGreater(m_WordList[i].Answer[j].ToString(), k);
                            // 해당 리스트인덱스에 string리스트를 추가해준다.
                            // Word, 1, 2, false
                            var temp_string = m_WordList[i].Answer + ", " + temp + ", " + j;
                            m_WordList[index].IndexInfo[temp].Add(temp_string);
                        }
                    }
                }
            }
        }
    }
    void MakeGrid(GridInfo info)
    {
        for (int i = 0; i < info.GridSize; i++)
        {
            for (int j = 0; j < info.GridSize; j++)
            {
                var temp_int = info.Grid[i, j].Split('&');
                if (int.Parse(temp_int[0]) != 0)
                {
                    var temp = Instantiate(GridPrefab, GameObject.Find("GridContainer").gameObject.transform);
                    //temp.name = m_MakeWord.Answer + " " + i;
                    temp.name = info.Grid[i,j].ToString();
                    m_ObjList.Add(temp);
                    temp.transform.localPosition = new Vector3((110 * i) - 250, (110 * j) - 250, 0);
                }
            }
        }

        PushData();
    }
    public void DeleteObj()
    {
        if (m_ObjList == null)
            return;
        int temp_int = m_ObjList.Count;
        for (int i = 0; i < temp_int; i++)
        {
            Destroy(m_ObjList[0]);
            m_ObjList.Remove(m_ObjList[0]);
        }
    }

    public void MakeGrid1()
    {
        DeleteObj();
        MakeGrid(LoadMapData("Scripts/Grid/Grid1.csv"));
    }
    public void MakeGrid2()
    {
        DeleteObj();
        MakeGrid(LoadMapData("Scripts/Grid/Grid2.csv"));
    }
    public void MakeGrid3()
    {
        DeleteObj();
        MakeGrid(LoadMapData("Scripts/Grid/Grid3.csv"));
    }
    public void MakeGrid4()
    {
        DeleteObj();
        MakeGrid(LoadMapData("Scripts/Grid/Grid4.csv"));
    }
    public void MakeGrid5()
    {
        DeleteObj();
        MakeGrid(LoadMapData("Scripts/Grid/Grid5.csv"));
    }
}
