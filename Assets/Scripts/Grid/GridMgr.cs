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
    private List<Word> m_TempList;
    private List<Word> m_MainList = new List<Word>();
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
    private List<GameObject> ObjList = new List<GameObject>();

    private void Start()
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
        
        Debug.Log(m_3List.Count);
        Debug.Log(m_4List.Count);
        Debug.Log(m_5List.Count);
        Debug.Log(m_6List.Count);
        Debug.Log(m_7List.Count);
        Debug.Log(m_8List.Count);
        Debug.Log(m_9List.Count);
        Debug.Log(m_10List.Count);
        Debug.Log(m_11List.Count);
        Debug.Log(m_12List.Count);
        Debug.Log(m_13List.Count);
        Debug.Log(m_14List.Count);
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
                            temp_grid.Grid[i, j] = bool.Parse(var_Value1[j]);
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

    void PutData()
    {
        for (int i = 0; i < tempGrid.WordCrossList.Count; i++)
        {
            var var_int = tempGrid.WordCrossList[i];
            if (var_int[0] == -1)
            { // 겹치는 단어가 없을때의 단어 처리
                m_TempList = PeekList(tempGrid.WordLenghtList[i]); // 같은 길이의 단어를 뽑습니다.
                int index = Random.Range(0, m_TempList.Count);
                m_WordList.Add(m_TempList[index]); // 단어를 추가해준다.
                m_WordList.Remove(m_TempList[index]); // 단어를 사용했으니 삭제해준다.
            }
            else
            { // 겹치는 단어가 있을때의 단어 처리
                var temp = tempGrid.WordCrossList[i].Split('/');
                bool find = true;
                m_TempList = PeekList(tempGrid.WordLenghtList[i]); // 같은 길이의 단어를 뽑습니다.

                while (find)
                {
                    int index = Random.Range(0, m_TempList.Count);
                    m_TempWord = m_TempList[index];

                    int Count = 0;
                    int MaxCount = 0;
                    List<Word> temp_list1 = new List<Word>();
                    List<Word> temp_list2 = new List<Word>();
                    List<Word> temp_list3 = new List<Word>();

                    if (temp.Length == 1)
                    { // 겹치는 단어가 1개
                        temp_list1 = PeekList(tempGrid.WordLenghtList[i + 1]);
                        MaxCount = temp_list1.Count;
                        while (find)
                        { // 같은 인덱스에서 같은 단어로 겹치는 단어가 뽑힐때까지
                            // 비상 탈출문
                            if (temp_list1.Count <= 0)
                            {
                                Debug.LogWarning("Cant Peek Cross Word");
                                return;
                            }
                            // 안전 탈출문
                            if (Count >= MaxCount)
                            {
                                break;
                            }

                            int crossindex = Random.Range(0, temp_list1.Count);
                            var var_value = tempGrid.WordCrossList[i + 1].Split('/');
                            for (int j = 0; j < var_value.Length; j++)
                            {
                                if (m_TempWord.Answer[int.Parse(temp[0])] == temp_list1[crossindex].Answer[int.Parse(var_value[0])])
                                { // 겹치는 단어가 같을때
                                    m_MainList.Add(m_TempWord);
                                    m_MainList.Add(temp_list1[crossindex]);
                                    m_TempWord = temp_list1[crossindex];
                                    m_WordList.Remove(m_TempWord);
                                    m_WordList.Remove(temp_list1[crossindex]);
                                    find = false;
                                    i++;
                                    break;
                                }
                                else
                                { // 겹치는 단어가 없을때
                                    temp_list1.Remove(temp_list1[crossindex]);
                                }
                            }
                            Count++;
                        }
                    }
                    else if (temp.Length == 2)
                    { // 겹치는 단어가 2개
                        temp_list1 = PeekList(tempGrid.WordLenghtList[i + 1]);
                        temp_list2 = PeekList(tempGrid.WordLenghtList[i + 2]);
                        MaxCount = temp_list1.Count * temp_list2.Count;
                        while (find)
                        { // 같은 인덱스에서 같은 단어로 겹치는 단어가 뽑힐때까지
                            // 비상 탈출문
                            if (temp_list1.Count <= 0 || temp_list2.Count <= 0)
                            {
                                Debug.LogWarning("Cant Peek Cross Word");
                                return;
                            }
                            // 안전 탈출문
                            if (Count >= MaxCount)
                            {
                                break;
                            }

                            int crossindex1 = Random.Range(0, temp_list1.Count);
                            int crossindex2 = Random.Range(0, temp_list2.Count);
                            var var_value1 = tempGrid.WordCrossList[i + 1];
                            var var_value2 = tempGrid.WordCrossList[i + 2];
                            for (int j = 0; j < var_value1.Length; j++)
                            {
                                if (m_TempWord.Answer[int.Parse(temp[0])] == temp_list1[crossindex1].Answer[var_value1[j]])
                                {
                                    for (int k = 0; k < var_value2.Length; k++)
                                    {
                                        if (m_TempWord.Answer[int.Parse(temp[1])] == temp_list1[crossindex2].Answer[var_value2[k]])
                                        { // 겹치는 단어가 같을때
                                            m_MainList.Add(m_TempWord);
                                            m_MainList.Add(temp_list1[crossindex1]);
                                            m_MainList.Add(temp_list1[crossindex2]);
                                            m_WordList.Remove(m_TempWord);
                                            m_WordList.Remove(temp_list1[crossindex1]);
                                            m_WordList.Remove(temp_list1[crossindex2]);
                                            find = false;
                                            i++;
                                            break;
                                        }
                                        else
                                        { // 겹치는 단어가 없을때
                                            temp_list1.Remove(temp_list1[crossindex1]);
                                            temp_list1.Remove(temp_list1[crossindex2]);
                                        }
                                    }
                                }
                            }
                            Count++;
                        }
                    }
                    else if (temp.Length == 3)
                    { // 겹치는 단어가 3개
                        temp_list1 = PeekList(tempGrid.WordLenghtList[i + 1]);
                        temp_list2 = PeekList(tempGrid.WordLenghtList[i + 2]);
                        temp_list3 = PeekList(tempGrid.WordLenghtList[i + 3]);
                        MaxCount = temp_list1.Count * temp_list2.Count * temp_list3.Count;
                        while (find)
                        { // 같은 인덱스에서 같은 단어로 겹치는 단어가 뽑힐때까지
                            // 비상 탈출문
                            if (temp_list1.Count <= 0 || temp_list2.Count <= 0 || temp_list3.Count <= 0)
                            {
                                Debug.LogWarning("Cant Peek Cross Word");
                                return;
                            }
                            // 안전 탈출문
                            if (Count >= MaxCount)
                            {
                                break;
                            }

                            int crossindex1 = Random.Range(0, temp_list1.Count);
                            int crossindex2 = Random.Range(0, temp_list2.Count);
                            int crossindex3 = Random.Range(0, temp_list3.Count);
                            if (m_TempWord.Answer[int.Parse(temp[0])] == temp_list1[crossindex1].Answer[int.Parse(tempGrid.WordCrossList[i + 1])])
                            {
                                if (m_TempWord.Answer[int.Parse(temp[1])] == temp_list2[crossindex2].Answer[int.Parse(tempGrid.WordCrossList[i + 2])])
                                {
                                    if (m_TempWord.Answer[int.Parse(temp[2])] == temp_list3[crossindex2].Answer[int.Parse(tempGrid.WordCrossList[i + 3])])
                                    { // 겹치는 단어가 같을때
                                        m_MainList.Add(m_TempWord);
                                        m_MainList.Add(temp_list1[crossindex1]);
                                        m_MainList.Add(temp_list1[crossindex2]);
                                        m_MainList.Add(temp_list1[crossindex3]);
                                        m_WordList.Remove(m_TempWord);
                                        m_WordList.Remove(temp_list1[crossindex1]);
                                        m_WordList.Remove(temp_list1[crossindex2]);
                                        m_WordList.Remove(temp_list1[crossindex3]);
                                        find = false;
                                        i++;
                                        break;
                                    }
                                }
                            }
                            else
                            { // 겹치는 단어가 없을때
                                temp_list1.Remove(temp_list1[crossindex1]);
                                temp_list1.Remove(temp_list1[crossindex2]);
                                temp_list1.Remove(temp_list1[crossindex3]);
                            }
                            Count++;
                        }
                    }
                }
            }
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
                if(info.Grid[i,j] == true)
                {
                    var temp = Instantiate(GridPrefab, GameObject.Find("GridContainer").gameObject.transform);
                    //temp.name = m_MakeWord.Answer + " " + i;
                    temp.tag = "Horizontal";
                    ObjList.Add(temp);
                    temp.transform.localPosition = new Vector3((110 * i) - 500, (110 * j) - 500, 0);
                }
            }
        }
    }
    public void DeleteObj()
    {
        if (ObjList == null)
            return;
        int temp_int = ObjList.Count;
        for (int i = 0; i < temp_int; i++)
        {
            Destroy(ObjList[0]);
            ObjList.Remove(ObjList[0]);
        }
    }

    public void MakeGrid1()
    {
        DeleteObj();
        MakeGrid(LoadMapData("Scripts/Grid/Grid1.csv"));
        PutData();
    }
    public void MakeGrid2()
    {
        DeleteObj();
        MakeGrid(LoadMapData("Scripts/Grid/Grid2.csv"));
        PutData();
    }
    public void MakeGrid3()
    {
        DeleteObj();
        MakeGrid(LoadMapData("Scripts/Grid/Grid3.csv"));
        PutData();
    }
    public void MakeGrid4()
    {
        DeleteObj();
        MakeGrid(LoadMapData("Scripts/Grid/Grid4.csv"));
        PutData();
    }
    public void MakeGrid5()
    {
        DeleteObj();
        MakeGrid(LoadMapData("Scripts/Grid/Grid5.csv"));
        PutData();
    }
}
