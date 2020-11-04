using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GridMgr : MonoBehaviour
{
    public string str_name;

    private GridInfo tempGrid;
    private bool[,] temp = new bool[5, 5];

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
    void LoadMapData()
    {
        StreamReader sr = new StreamReader(Application.dataPath + "/" + str_name);

        if(0 == sr.Peek())
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
                    temp_grid.WordCrossList.Add(int.Parse(var_Value3[i]));
                }
                break;
            default:
                Debug.LogWarning("예상치 못한 데이터");
                break;
            }
        }

        temp_grid.Grid.GetType();
        Debug.Log("Successfully reading data");
    }

    private void Start()
    {
        temp[0, 0] = true;
        temp[0, 1] = true;
        temp[0, 2] = true;
        temp[0, 3] = true;
        temp[0, 4] = true;
        temp[1, 0] = true;
        temp[2, 0] = true;
        temp[1, 2] = true;
        temp[2, 2] = true;
        temp[3, 2] = true;
        temp[2, 3] = true;
        temp[2, 4] = true;
        temp[4, 0] = true;
        temp[4, 1] = true;
        tempGrid = new GridInfo(5,5);
        tempGrid.Grid = temp;
        tempGrid.WordLenghtList.Add(3);
        tempGrid.WordLenghtList.Add(5);
        tempGrid.WordLenghtList.Add(4);
        tempGrid.WordLenghtList.Add(3);
        tempGrid.WordLenghtList.Add(2);
        tempGrid.WordCrossList.Add(0);
        tempGrid.WordCrossList.Add(2);
        tempGrid.WordCrossList.Add(2);
        tempGrid.WordCrossList.Add(-1);
        tempGrid.WordCrossList.Add(-1);
        SaveMapData();
        LoadMapData();
    }
}
