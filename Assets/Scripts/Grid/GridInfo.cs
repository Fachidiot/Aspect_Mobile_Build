using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInfo
{
    // 그리드
    private bool[,] m_Grid;
    public bool[,] Grid { get { return m_Grid; } set { m_Grid = value; } }
    // 단어 길이 리스트
    private List<int> m_WordLengthList;
    public List<int> WordLenghtList { get { return m_WordLengthList; } set { m_WordLengthList = value; } }
    // 단어 크로스 인덱스 리스트
    private List<int> m_WordCrossList;
    public List<int> WordCrossList { get { return m_WordCrossList; } set { m_WordCrossList = value; } }


    public GridInfo()
    {
    }
    // 생성자
    public GridInfo(int x, int y)
    {
        m_Grid = new bool[x, y];
        WordLenghtList = new List<int>();
        WordCrossList = new List<int>();
    }
}
