﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InputMgr : MonoBehaviour
{
    [Header("Veiwer")]
    public GameObject RectScroll;
    [Header("Effect Image")]
    public Image TimeOver;
    public Image Congratulations;
    public Image Correct;
    public Image Wrong;
    [Header("MainMenu")]
    public Text RewardCount;
    public Text TimeSet;
    public Text CountSet;
    [Header("Text UI")]
    public Text CurrentHint;
    public Text QuestionCount;
    public Text Ready;
    public Text Start;
    [Header("Timer")]
    public Text TimerText;
    public float MaxTime;
    [Header("Count")]
    public int MaxCount;
    [HideInInspector]
    public bool Assistant;
    [Header("Effect Time")]
    public Text WinTime;
    public Text OverCount;
    public GameObject EffectNoTime;
    [Header("Effect Sound")]
    public AudioSource CorrectSource;
    public AudioSource SelectSource;
    public AudioSource BKSpaceSource;
    public AudioSource NotimeSource;
    [Header("Buttons")]
    public Button BT_prev;
    public Button BT_next;

    private float m_CurrentTime;
    private float m_EffectTime;
    private bool m_o = true;
    private bool m_x = true;
    private string m_Difficult;
    private int m_Score = -1;
    private int Checkindex;
    public int GetScore(string difficult)
    {
        if(PlayerPrefs.HasKey(difficult))
        {
            return PlayerPrefs.GetInt(difficult);
        }
        else
        {
            return -1;
        }
    }

    [HideInInspector]
    public bool m_Power;
    public void PowerOn()
    {
        m_Power = true;
    }
    public void PowerOff()
    {
        Start.color = new Color(0.196f, 0.196f, 0.196f, 1);
        Ready.GetComponent<Outline>().effectDistance = new Vector2(0, 0);
        Ready.gameObject.SetActive(true);
        Start.gameObject.SetActive(false);
        m_WordList = new List<Word>();
        m_DoneList = new List<Word>();
        m_InputList = new List<string>();
        m_InputIndexList = new List<int>();
        m_IsClicked = false;
        m_CurrentTime = 0;
        m_IsStart = false;
        m_IsList = false;
        m_IsEnd = false;
        m_IsInput = false;
        m_o = true;
        m_x = true;
        BT_next.interactable = false;
        BT_prev.interactable = false;
        Correct.gameObject.SetActive(false);
        Wrong.gameObject.SetActive(false);
        m_IsCheck = false;
        Correct.color += new Color(0, 0, 0, 255);
        Wrong.color += new Color(0, 0, 0, 255);
        CurrentHint.text = "";
        EffectNoTime.SetActive(false);
        TimerText.fontSize = 90;
        NotimeSource.gameObject.SetActive(false);

        if (m_ObjectList != null)
        {
            for (int i = 0; i < m_ObjectList.Length; i++)
            {
                if(m_ObjectList[i] != null)
                {
                    m_ObjectList[i].GetComponent<Outline>().enabled = false;
                }
            }
        }

        m_Power = false;
    }
    public void PowerReboot()
    {
        PowerOff();
        PowerOn();
    }

    private bool m_IsStart;
    private bool m_IsList;
    private bool m_IsEnd;
    public bool End { get { return m_IsEnd; } set { m_IsEnd = value; } }
    private bool m_IsClicked;
    private bool m_IsInput;
    private bool m_IsCheck;
    // 정답
    private Word m_Answer;
    // 사용자 입력
    private List<string> m_InputList = new List<string>();
    private List<int> m_InputIndexList = new List<int>();
    private int CurrentIndex = 0;
    private int m_MaxCount = 10;
    private GameObject TempInput;
    private GameObject WordManager;
    private GameObject CrossManager;
    private List<Word> m_WordList;
    private List<Word> m_DoneList;
    private GameObject[] m_ObjectList;

    private void Awake()
    {
        Assistant = true;
        m_IsEnd = false;
        m_IsList = false;
        m_IsStart = false;
        m_IsCheck = false;
        m_IsInput = false;
        m_IsClicked = false;
        m_WordList = new List<Word>();
        m_DoneList = new List<Word>();
    }

    GameObject CheckCross()
    {
        bool vertical;
        if (TempInput.tag == "Vertical")
        {
            vertical = true;
        }
        else
        {
            vertical = false;
        }
        var temp = CheckDouble(TempInput.transform.localPosition, vertical);

        return temp;
    }

    public void KeyInput(string _input, bool ishint = false)
    {
        if (m_IsClicked)
        {
            if(ishint)
            {
                if (m_InputIndexList[CurrentIndex] >= m_Answer.Answer.Length)
                {
                    return;
                }

                int CalCount = 0;
                for (int i = 0; i < m_Answer.Answer.Length; i++)
                {
                    string input_temp = "";
                    TempInput = GameObject.Find(m_Answer.Answer + " " + (m_InputIndexList[CurrentIndex] + i));
                    if (TempInput.GetComponent<InputWord>().Answer.text == "")
                    {
                        input_temp = _input[i].ToString();
                        if(TempInput.GetComponent<InputWord>().Hint.text == "")
                        {
                            CalCount++;
                        }
                        TempInput.GetComponent<InputWord>().SetHint(input_temp);
                    }
                    var temp = CheckCross();

                    if (temp != null)
                    { // 겹치는 부분이 존재함
                        if (temp.GetComponent<InputWord>().Answer.text != "")
                        { // 겹쳐있는 단어에 이미 다른 알파벳이 들어가 있을때
                            continue;
                        }

                        TempInput.GetComponent<InputWord>().SetHint(input_temp);
                        temp.GetComponent<InputWord>().SetHint(input_temp);

                        continue;
                    }
                }

                if (CalCount <= 0)
                {
                    return;
                }
                Debug.Log("힌트 사용!");
            }

            else if (_input == "BK_SPACE")
            {
                if(m_InputIndexList[CurrentIndex] <= 0)
                {
                    m_IsInput = false;
                    return;
                }

                BKSpaceSource.Play();

                TempInput = GameObject.Find(m_Answer.Answer + " " + (m_InputIndexList[CurrentIndex] - 1));
                var temp1 = CheckCross();

                if (temp1 != null)
                { // 겹치는 부분이 존재함
                    if (temp1.GetComponent<InputWord>().IsCorrect)
                    { // 겹치는 단어가 정답일때
                        if (m_InputList[CurrentIndex].Length <= 1)
                        {
                            return;
                        }
                        string var_string = temp1.GetComponent<InputWord>().GetInput();
                        m_InputList[CurrentIndex] = m_InputList[CurrentIndex].Remove(m_InputList[CurrentIndex].Length - 1);
                        m_InputList[CurrentIndex] = m_InputList[CurrentIndex].Remove(m_InputList[CurrentIndex].Length - 1);

                        TempInput = GameObject.Find(m_Answer.Answer + " " + (m_InputIndexList[CurrentIndex] - 2));
                        TempInput.GetComponent<InputWord>().Wrong();
                        Debug.Log(_input);
                        m_InputIndexList[CurrentIndex]--;
                        m_InputIndexList[CurrentIndex]--;
                        return;
                    }
                    m_InputList[CurrentIndex] = m_InputList[CurrentIndex].Remove(m_InputList[CurrentIndex].Length - 1);
                    Debug.Log(_input);
                    TempInput.GetComponent<InputWord>().Wrong();
                    temp1.GetComponent<InputWord>().Wrong();
                    m_InputIndexList[CurrentIndex]--;
                    return;
                }

                m_InputList[CurrentIndex] = m_InputList[CurrentIndex].Remove(m_InputList[CurrentIndex].Length - 1);
                Debug.Log(_input);
                TempInput.GetComponent<InputWord>().Wrong();
                m_InputIndexList[CurrentIndex]--;
                return;
            }

            else
            {
                if (m_InputIndexList[CurrentIndex] >= m_Answer.Answer.Length)
                {
                    Debug.Log("더이상 입력할 수 없습니다.");
                    return;
                }
                TempInput = GameObject.Find(m_Answer.Answer + " " + m_InputIndexList[CurrentIndex]);

                if (Assistant)
                {
                    Vector3 Pos = new Vector3(-TempInput.transform.localPosition.y, TempInput.transform.localPosition.x, 0);
                    RectScroll.transform.localPosition = Pos;
                }

                var temp2 = CheckCross();
                SelectSource.Play();

                if (temp2 != null)
                { // 겹치는 부분이 존재함
                    if (temp2.GetComponent<InputWord>().GetInput() != "")
                    { // 겹쳐있는 단어에 이미 다른 알파벳이 들어가 있을때
                        string var_string = temp2.GetComponent<InputWord>().GetInput();
                        Debug.Log(var_string);
                        m_InputList[CurrentIndex] += string.Join("", var_string);
                        m_InputIndexList[CurrentIndex]++;

                        TempInput = GameObject.Find(m_Answer.Answer + " " + m_InputIndexList[CurrentIndex]);
                        TempInput.GetComponent<InputWord>().Input(_input);
                        m_InputIndexList[CurrentIndex]++;
                        m_IsInput = true;
                        Debug.Log(_input);
                        m_InputList[CurrentIndex] += string.Join("", _input);
                        return;
                    }
                    Debug.Log(_input);
                    TempInput.GetComponent<InputWord>().Input(_input);
                    temp2.GetComponent<InputWord>().Input(_input);
                    m_InputIndexList[CurrentIndex]++;
                    m_IsInput = true;
                    m_InputList[CurrentIndex] += string.Join("", _input);

                    return;
                }

                Debug.Log(_input);
                TempInput.GetComponent<InputWord>().Input(_input);
                m_InputIndexList[CurrentIndex]++;
                m_IsInput = true;
                m_InputList[CurrentIndex] += string.Join("", _input);

                // 마지막 인덱스 전에서 마지막 인덱스가 정답인 경우에 채워 넣어준다.
                if (m_Answer.Length - 1 == m_InputIndexList[CurrentIndex])
                {
                    TempInput = GameObject.Find(m_Answer.Answer + " " + m_InputIndexList[CurrentIndex]);
                    if (TempInput.GetComponent<InputWord>().GetInput() != "")
                    { // 비어 있지 않을때
                        string var_string = TempInput.GetComponent<InputWord>().GetInput();
                        Debug.Log(var_string);
                        m_InputList[CurrentIndex] += string.Join("", var_string);
                        m_InputIndexList[CurrentIndex]++;
                    }
                }

                return;
            }

        }
    }

    GameObject CheckDouble(Vector3 Pos, bool Vertical)
    {
        if (Vertical)
        {
            var temp = GameObject.FindGameObjectsWithTag("Horizontal");
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i].transform.localPosition == Pos)
                {
                    var obj = temp[i];
                    return obj;
                }
            }
        }
        else
        {
            var temp = GameObject.FindGameObjectsWithTag("Vertical");
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i].transform.localPosition == Pos)
                {
                    var obj = temp[i];
                    return obj;
                }
            }
        }

        return null;
    }

    IEnumerator FadeOut(float fadeOutTime, Image image)
    {
        Color tempColor = image.color;
        while (tempColor.a > 0f)
        {
            tempColor.a -= Time.deltaTime / fadeOutTime;
            image.color = tempColor;

            if (tempColor.a <= 0.1f)
            {
                tempColor.a = 1f;

                image.gameObject.SetActive(false);

                StopAllCoroutines();
            }
            yield return null;
        }
        image.color = tempColor;
    }

    void DiffuseEffect()
    {
        if (Correct.IsActive())
        { // 정답 이펙트
            StartCoroutine(FadeOut(1.2f, Correct));
            m_o = false;
        }
        else
        {
            if(!m_o)
            {
                m_IsCheck = false;

                RESET();
                m_o = true;
                // 문제를 맞혔을때만 다음으로 넘어가거나 뒤로 돌아가게 해줘야함
                if (Assistant)
                {
                    if (Checkindex < m_WordList.Count)
                    {
                        Next(true);
                    }
                }
            }
        }
        if (Wrong.IsActive())
        { // 오답 이펙트
            StartCoroutine(FadeOut(1.2f, Wrong));
            m_x = false;
        }
        else
        {
            if(!m_x)
            {
                m_IsCheck = false;
                RESET();

                // 문제를 틀렸을때는 다시 해당 문제로 가게 해줘야함
                m_IsClicked = true;
                HighLight();
                m_x = true;
            }
        }
        return;
    }

    private void FixedUpdate()
    {
        if (m_Power)
        {
            // 시작했을때 한번만
            if (!m_IsList)
            {
                WordManager = GameObject.Find("WordMgr");
            }
            if (WordManager.GetComponent<WordMgr>().IsMake)
            {
                // 시작했을때 한번만
                if (!m_IsStart)
                {
                    Ready.gameObject.SetActive(false);
                    Start.gameObject.SetActive(true);
                    m_IsStart = true;
                }
                else if (!m_IsEnd)
                {
                    Timer();
                    ////////////////////
                    if(Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        Prev();
                    }
                    if(Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        Next();
                    }
                }
                if(Start.color.a <= 0)
                {
                    Start.gameObject.SetActive(false);
                }
                Start.color -= new Color(0, 0, 0, 1f) * Time.deltaTime;
            }
            else
            { // 게임 시작 전
                Ready.GetComponent<Outline>().effectDistance += new Vector2(150.0f, 0.0f) * Time.deltaTime;
            }

            if (m_IsCheck)
            {
                DiffuseEffect();
            }
        }
    }

    void Timer()
    {
        if(m_DoneList.Count >= m_MaxCount)
        { // 모든 정답을 맞춘 상황
            Debug.Log("게임 종료");
            m_Score = (int)(m_CurrentTime);
            m_IsEnd = true;
            Congratulations.gameObject.SetActive(true);
            WinTime.text = ((int)m_CurrentTime).ToString() + " 초";
            return;
        }
        if(m_CurrentTime >= MaxTime)
        { // 시간 종료
            Debug.Log("시간 종료");
            m_IsEnd = true;
            // 게임 시작 화면으로
            TimeOver.gameObject.SetActive(true);
            OverCount.text = m_DoneList.Count.ToString() + " 개";
            NotimeSource.Stop();
            return;
        }
        m_CurrentTime += Time.deltaTime;
        if(MaxTime - m_CurrentTime <= 15)
        { // 시간 없음 이펙트
            if(!NotimeSource.gameObject.activeSelf)
            {
                NotimeSource.gameObject.SetActive(true);
            }
            float DeltaZ = (Mathf.Sin(m_EffectTime + Time.deltaTime) - Mathf.Sin(m_EffectTime));
            TimerText.fontSize += (int)(DeltaZ * 52);
            if (DeltaZ >= 0)
                EffectNoTime.SetActive(true);
            else
                EffectNoTime.SetActive(false);
            m_EffectTime += Time.deltaTime;
        }
        TimerText.text = ((int)(MaxTime - m_CurrentTime)).ToString();
    }

    private void Update()
    {
        if (m_Power)
        {
            // 시작했을때 한번만
            if (!m_IsList)
            {
                CrossManager = GameObject.Find("CrossMgr");
                CrossManager.GetComponent<CrossWord>().SetRandomList(MaxCount);
                WordManager = GameObject.Find("WordMgr");
                m_IsList = true;
            }
            // 처음 한번만 단어 목록 생성
            if (m_WordList.Count <= 0 && WordManager.GetComponent<WordMgr>().IsMake)
            {
                var temp = WordManager.GetComponent<WordMgr>().GetList();
                for (int i = 0; i < temp.Length; i++)
                {
                    m_WordList.Add(temp[i]);
                    m_InputList.Add("");
                    m_InputIndexList.Add(0);
                }
                m_MaxCount = m_WordList.Count;
                BT_next.interactable = true;
                BT_prev.interactable = true;
            }

            QuestionCount.text = (m_DoneList.Count + " / " + m_MaxCount);

            // 표를 눌렀을 때
            if(!m_IsEnd)
            {
                if (m_IsClicked)
                {
                    if (m_IsInput && m_InputList[CurrentIndex].Length == m_Answer.Answer.Length)
                    {
                        if (m_InputList[CurrentIndex] == m_Answer.Answer)
                        {
                            // 정답
                            CorrectSource.Play();
                            Correct.gameObject.SetActive(true);
                            SetCorrect();
                            m_DoneList.Add(m_Answer);
                            Checkindex = FindWord(m_Answer);
                            Word TempWord = new Word();
                            if (Checkindex + 1 != m_WordList.Count)
                            {
                                TempWord = m_WordList[Checkindex + 1];
                                if (Assistant)
                                {
                                    Checkindex = FindWord(m_Answer);
                                    CurrentIndex = Checkindex;
                                }
                            }
                            m_WordList.Remove(m_Answer);
                            m_InputList.Remove(m_Answer.Answer);
                            m_InputIndexList.Remove(m_Answer.Length);
                            if (Assistant)
                            {
                                m_Answer = TempWord;
                            }
                            m_IsCheck = true;
                            m_IsClicked = false;
                        }
                        else
                        {
                            // 오답
                            Wrong.gameObject.SetActive(true);
                            m_IsCheck = true;
                            for (int i = 0; i < m_Answer.Answer.Length; i++)
                            {
                                m_InputIndexList[CurrentIndex] = 0;
                                m_InputList[CurrentIndex] = "";
                                TempInput = GameObject.Find(m_Answer.Answer + " " + i);
                                TempInput.GetComponent<InputWord>().Wrong();
                                var temp = CheckCross();
                                if (temp != null)
                                {
                                    if (temp.GetComponent<InputWord>().IsCorrect)
                                    {
                                        TempInput.GetComponent<InputWord>().Input(temp.GetComponent<InputWord>().GetInput());
                                    }
                                    else
                                    {
                                        temp.GetComponent<InputWord>().Wrong();
                                    }
                                }
                            }
                            TempInput = GameObject.Find(m_Answer.Answer + " " + 0);
                            Vector3 Pos = new Vector3(-TempInput.transform.localPosition.y, TempInput.transform.localPosition.x, 0);
                            RectScroll.transform.localPosition = Pos;
                        }
                    }
                }
            }
        }
    }

    public void Next(bool auto = false)
    {
        if(auto)
        {
            RESET();
            CurrentHint.text = m_Answer.Meaning;
        }
        else
        {
            RESET();
            int index = FindWord(m_Answer);
            if (index <= m_WordList.Count - 1)
            {
                if(index != m_WordList.Count - 1)
                {
                    m_Answer = m_WordList[index + 1];
                    index = FindWord(m_Answer);
                    CurrentHint.text = m_Answer.Meaning;
                    CurrentIndex = index;
                }
            }
            else
            {
                return;
            }
        }
        Debug.Log(m_Answer.Answer);
        HighLight();

        var temp = GameObject.Find(m_Answer.Answer + " 0");
        Vector3 Pos = new Vector3(-temp.transform.localPosition.y, temp.transform.localPosition.x, 0);
        RectScroll.transform.localPosition = Pos;

        m_IsClicked = true;
    }

    public void Prev()
    {
        RESET();
        int index = FindWord(m_Answer);
        if (index >= 0)
        {
            if (index != 0)
            {
                m_Answer = m_WordList[index - 1];
                index = FindWord(m_Answer);
                CurrentHint.text = m_Answer.Meaning;
                CurrentIndex = index;
            }
        }
        else
        {
            return;
        }
        Debug.Log(m_Answer.Answer);
        HighLight();

        var temp = GameObject.Find(m_Answer.Answer + " 0");
        Vector3 Pos = new Vector3(-temp.transform.localPosition.y, temp.transform.localPosition.x, 0);
        RectScroll.transform.localPosition = Pos;

        m_IsClicked = true;
    }

    public void QuestionClick(string _answer)
    {
        RESET();
        for (int i = 0; i < m_DoneList.Count; i++)
        {
            if(m_DoneList[i].Answer == _answer)
            {
                return;
            }
        }
        for (int i = 0; i < m_WordList.Count; i++)
        {
            if (m_WordList[i].Answer == _answer)
            { // 정답 정보 입력
                m_Answer = m_WordList[i];
                CurrentIndex = i;
            }
        }
        Debug.Log(m_Answer.Answer);
        HighLight();
        CurrentHint.text = m_Answer.Meaning;
        m_IsClicked = true;
    }

    void HighLight()
    { // 클릭시 하이라이팅 처리
        m_ObjectList = new GameObject[m_Answer.Length];
        for (int i = 0; i < m_Answer.Length; i++)
        {
            m_ObjectList[i] = GameObject.Find(m_Answer.Answer + " " + i);
            m_ObjectList[i].GetComponent<Outline>().enabled = true;
        }
    }

    void SetCorrect()
    {
        for (int i = 0; i < m_Answer.Length; i++)
        {
            var temp = GameObject.Find(m_Answer.Answer + " " + i);
            temp.GetComponent<InputWord>().IsCorrect = true;
        }
    }

    void RESET()
    {
        if(m_ObjectList != null)
        {
            for (int i = 0; i < m_ObjectList.Length; i++)
            {
                if (m_ObjectList[i] != null)
                {
                    m_ObjectList[i].GetComponent<Outline>().enabled = false;
                }
            }
        }
        m_o = true;
        m_x = true;
        m_IsClicked = false;
        m_IsInput = false;
        Correct.gameObject.SetActive(false);
        Wrong.gameObject.SetActive(false);
        m_IsCheck = false;
        Correct.color = new Color(1, 1, 1, 1);
        Wrong.color = new Color(1, 1, 1, 1);
    }

    int FindWord(Word word)
    {
        for (int i = 0; i < m_WordList.Count; i++)
        {
            if(m_WordList[i] == word)
            {
                return i;
            }
        }

        return -1;
    }

    int FindDoneWord(string _answer)
    {
        for (int i = 0; i < m_DoneList.Count; i++)
        {
            if(m_DoneList[i].Answer == _answer)
            {
                return i;
            }
        }

        return -1;
    }

    public void ShowHint()
    {
        if (m_Answer == null)
            return;

        KeyInput(m_Answer.Answer, true);
    }

    public void Pause(bool power)
    {
        m_Power = power;
    }

    public void CountUp()
    {
        if (MaxCount >= 30)
            return;
        MaxCount = MaxCount + 5;
        RewardCount.text = ((int)((MaxCount * (510 - MaxTime)) / 60)).ToString() + " 개";
        CountSet.text = MaxCount.ToString() + " 개";
    }

    public void CountDown()
    {
        if (MaxCount <= 5)
            return;
        MaxCount = MaxCount - 5;
        RewardCount.text = ((int)((MaxCount * (510 - MaxTime)) / 60)).ToString() + " 개";
        CountSet.text = MaxCount.ToString() + " 개";
    }

    public void TimeUp()
    {
        if (MaxTime >= 480)
            return;
        MaxTime = MaxTime + 30;
        RewardCount.text = ((int)((MaxCount * (510 - MaxTime)) / 60)).ToString() + " 개";
        TimeSet.text = (MaxTime / 60).ToString() + " 분";
    }

    public void TimeDown()
    {
        if (MaxTime <= 30)
            return;
        MaxTime = MaxTime - 30;
        RewardCount.text = ((int)((MaxCount * (510 - MaxTime)) / 60)).ToString() + " 개";
        TimeSet.text = (MaxTime / 60).ToString() + " 분";
    }
}