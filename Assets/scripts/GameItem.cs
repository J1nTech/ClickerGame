using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameItem : Item
{
    EScrollButtonState state;
    [SerializeField] TextMeshProUGUI _titletxt;
    public TextMeshProUGUI TitleTxt => _titletxt;
    int orderNum = 0;
    public int OrderNum => orderNum;
    [SerializeField] TextMeshProUGUI _itemInfotxt;
    [SerializeField] TextMeshProUGUI _costTxt;
    [SerializeField] Image _coinImg;
    [SerializeField] Image _itemImg;
    [SerializeField] Button itemPurchaseBtn;

    Vector3[] corners = new Vector3[4];
    public Vector3[] GetCorners => corners;
    public RectTransform rect;
    System.Numerics.BigInteger cost;
    private void Awake()
    {
        itemPurchaseBtn.onClick.AddListener(OnButton_Click);
    }
    void SetItem(string titleTxt, string itemInfoTxt, int cost, Sprite coinImg, Sprite itemImg)
    {
        _titletxt.text = titleTxt;
        _itemInfotxt.text = itemInfoTxt;
        _costTxt.text = string.Format(cost.ToString(), "N0");
        _coinImg.sprite = coinImg;
        _itemImg.sprite = itemImg;
    }

    public void SetTitle(string titleString)
    {
        orderNum = int.Parse(titleString);

        if (orderNum % 2 == 0)
        {
            state = EScrollButtonState.PerSec;
            SetPerSecCost();
            _itemInfotxt.text = $"Increase {1} Per Second";
        }
        else
        {
            state = EScrollButtonState.Click;
            SetClickCost();
            _itemInfotxt.text = $"Increase {1} click Power";
        }
        SetCostTxt();
        _titletxt.text = titleString;

    }

    void SetPerSecCost()
    {
        /*odernum / index num
         
            1 / 0 1
            2 / 0

            3 / 1 1
            4 / 1

            5 /2  1
            6/2

            7/3 1
            8/3 

            9/4
            10/4

            11/5
            12/5
            13/6
            14/6
         */
        //var costStr = GameManager.GetInstance.SecCostList[GameManager.GetInstance.uiMain.perSecIdx++];
        var costStr = GameManager.GetInstance.SecCostList[orderNum / 2 - 1];
        System.Numerics.BigInteger bigInteger = System.Numerics.BigInteger.Parse(costStr);
        cost = bigInteger;
    }
    void SetClickCost()
    {
        var costStr = GameManager.GetInstance.clickCostList[orderNum / 2];
        System.Numerics.BigInteger bigNum = System.Numerics.BigInteger.Parse(costStr);
        cost = bigNum;
    }

    void SetCostTxt()
    {
        _costTxt.text = cost.ToString("N0");
    }
    void OnButton_Click()
    {
        GameManager.GetInstance.BuyItem(cost);
        switch (state)
        {
            case EScrollButtonState.Click:
                GameManager.GetInstance.IncreaseClickPower(1);
                break;
            case EScrollButtonState.PerSec:
                GameManager.GetInstance.IncreaseAddPerSecScoreNum(1);
                break;
        }
    }
}
