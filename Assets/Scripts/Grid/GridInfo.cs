using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInfo
{
    // 그리드
    private string[,] m_Grid;
    public string[,] Grid { get { return m_Grid; } set { m_Grid = value; } }
    private int m_GridSize;
    public int GridSize { get { return m_GridSize; } set { m_GridSize = value; } }
    // 단어 길이 리스트
    private List<int> m_WordLengthList;
    public List<int> WordLenghtList { get { return m_WordLengthList; } set { m_WordLengthList = value; } }
    // 단어 크로스 인덱스 리스트
    private List<string> m_WordCrossList;
    public List<string> WordCrossList { get { return m_WordCrossList; } set { m_WordCrossList = value; } }


    public GridInfo()
    {
    }
    // 생성자
    public GridInfo(int x, int y)
    {
        m_Grid = new string[x, y];
        GridSize = x;
        WordLenghtList = new List<int>();
        WordCrossList = new List<string>();
    }
}
