using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class GameManager : MonoBehaviour
{
    public List<string> clickCostList = new List<string>(9);
    public List<string> SecCostList = new List<string>(9);

    private static GameManager _instance;
    public static GameManager GetInstance => _instance;

    public UIMain uiMain;

    public BigInteger score=0;

    public int clickPower = 1;
    public float duraion = 1f;
    public int addNumPerSec = 0;
    public int PerSecPoser => addNumPerSec;

    Action StartCount;


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
    }
    private void Start()
    {
        //StartCoroutine(AddScorePerSec());
        StartCount += AsyncAddScorePerSec;
        StartGame();
    }
    void StartGame()
    {
        StartCount.Invoke();
    }
    public void addScore()
    {
        score += clickPower;
    }
    void addPerSec()
    {
        score++;
    }

    public void IncreaseAddPerSecScoreNum(int addPerSecPower)
    {
        addNumPerSec += addPerSecPower;
        uiMain.UpdatePerSecPowerDisplay();
        //Debug.Log($"increase addPerSecNum = {addNumPerSec}");
    }
    public void IncreaseClickPower(int addPower)
    {
        clickPower += addPower;
        //update MainUI
        uiMain.UpdateClickPowerDisplay();
    }
    public bool BuyItem(BigInteger price)
    {
        if (score < price) return false;
        else
        {
            score -= price;
            uiMain.UpdateDisplay();
            return true;
        }


    }
    //IEnumerator AddPerSec()
    //{
    //    while(true)
    //    {
    //        addPerSec();
    //        uiMain.UpdateDisplay();
    //        yield return new WaitForSecondsRealtime(duraion);
    //    }
    //}
    IEnumerator AddScorePerSec()
    {
        while(true)
        {
            var waitTime = 1000f / addNumPerSec;

            //yield return StartCoroutine(AddPerSec(waitTime));
          
        }
        yield return null;
    }
 
    IEnumerator AddPerSec(float waitTime)
    {
        if (addNumPerSec == 0) yield break;
        var times = addNumPerSec;
        float milisecToSec = waitTime / 1000f;
        var targetNum = score + addNumPerSec;
        for (int i = 0; i < addNumPerSec; i++)
        {
            addPerSec();
            uiMain.UpdateDisplay();
            yield return new WaitForSeconds(milisecToSec);

        }
        score = targetNum;
    }
    public async void AsyncAddScorePerSec()
    {
        while (true)
        {
            var start = DateTime.Now;
            if (addNumPerSec == 0)
                await Task.Delay(1000);
            else
            {
                var waitTime = 1000f / addNumPerSec;
                for (int i = 0; i < addNumPerSec; i++)
                {
                    addPerSec();
                    uiMain.UpdateDisplay(); // need to change this to wait more 
                    await Task.Delay((int)waitTime);
                }
            }
        }
    }
    public async Task AsyncAddPerSec(float waitTime)
    {
        if (addNumPerSec == 0) return;
        var times = addNumPerSec;
        int numToAdd = addNumPerSec;
        int milisecToSec = Mathf.CeilToInt(waitTime / 1000f);
        var targetNum = score + addNumPerSec;
        for (int i = 0; i < addNumPerSec; i++)
        {
            addPerSec();
            uiMain.UpdateDisplay();
            if (i < numToAdd - 1) // Don't delay after the last iteration
            {
                await Task.Delay((int)waitTime);
            }

        }
        score = targetNum;
    }
    private void OnEnable()
    {
        StartCount -= AsyncAddScorePerSec;
    }

}
