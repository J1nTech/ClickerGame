using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class UIMain : MonoBehaviour
{
    [SerializeField] Button mainButn;

    [SerializeField] Transform mainBtnImg;
    [SerializeField] TextMeshProUGUI lb_Score;
    [SerializeField] TextMeshProUGUI lb_PerScend;
    [SerializeField] TextMeshProUGUI lb_clickPower;

    private System.Numerics.BigInteger displayedScore; // The score displayed on the UI
    public System.Numerics.BigInteger targetScore;    // The target score to be displayed
    private float updateDuration = 1.0f; // The duration over which you want to update the score

    public int perSecIdx =0;
    public int clickIdx = 0;
    private void Start()
    {
        SetButton();
        displayedScore = GameManager.GetInstance.score; // Initialize displayed score
        lb_Score.text = displayedScore.ToString("N0");
        lb_clickPower.text = GameManager.GetInstance.clickPower.ToString("N0");
        string display = $"{GameManager.GetInstance.PerSecPoser.ToString("N0")} Amount Per Sec";
        lb_PerScend.text = display;
    }

    void SetButton()
    {
        mainButn.onClick.AddListener(OnMainButton_Clicked);
    }
    
    void OnMainButton_Clicked()
    {
        
        ScaleAnimation();
        //GameManager.GetInstance.IncreaseAddPerSecScoreNum();
        //GameManager.GetInstance.decreasePerSec();
        IncreseScore();
        UpdateDisplay();
    }

    void IncreseScore()
    {
        GameManager.GetInstance.addScore();
        targetScore = GameManager.GetInstance.score;
    }

    void ScaleAnimation()
    {
        var originalScale = mainBtnImg.localScale;
        Vector2 targetScale = new Vector2(mainBtnImg.localScale.x * .8f, mainBtnImg.localScale.y * .8f);

        mainBtnImg.DOScale(targetScale, .1f).SetEase(Ease.InOutQuart).OnComplete(() =>
        {
            mainBtnImg.DOScale(Vector2.one, .1f).SetEase(Ease.Linear);
        });
    }
    public void UpdateDisplay()
    {
        lb_Score.text = GameManager.GetInstance.score.ToString("N0");
    }
    public void UpdateClickPowerDisplay()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append($"+ { GameManager.GetInstance.clickPower.ToString("N0")}");
        lb_clickPower.text = sb.ToString();
        sb.Clear();
    }
    public void UpdatePerSecPowerDisplay()
    {
        string display = $"{GameManager.GetInstance.PerSecPoser.ToString("N0")} Amount Per Sec";
        lb_PerScend.text = display;
    }


    //problem is simple displayed score not increase gradually it increase maybe simplest way to fix it is call updateText in update??? 
    //IEnumerator UpdateScoreContinuously()
    //{
    //    while (true)
    //    {
    //        float elapsedTime = 0.0f;
    //        int startScore = displayedScore;

    //        while (elapsedTime < updateDuration)
    //        {
    //            float normalizedTime = elapsedTime / updateDuration;
    //            displayedScore = Mathf.RoundToInt(Mathf.Lerp(startScore, targetScore, normalizedTime));
    //            lb_Score.text = displayedScore.ToString("N0");

    //            elapsedTime += Time.deltaTime;
    //            yield return null;
    //        }

    //        displayedScore = targetScore; // Ensure the score is set to the exact target value at the end
    //        lb_Score.text = displayedScore.ToString("N0");

    //        yield return new WaitForSeconds(1.0f); // Wait for one second before the next update
    //    }
    //}
}
