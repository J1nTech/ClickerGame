using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _titletxt;
    [SerializeField] TextMeshProUGUI _itemInfotxt;
    [SerializeField] TextMeshProUGUI _costTxt;
    [SerializeField] Image _coinImg;
    [SerializeField] Image _itemImg;
    [SerializeField] Button itemPurchaseBtn;

    Vector3[] corners = new Vector3[4];
    public RectTransform rect;
    private void Awake()
    {
        SetCorners();
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
    void OnButton_Click()
    {
        Debug.Log("Do Somethign");
    }
    public Vector3 GetMaxCorner()
    {
        return corners[2];
    }
    public Vector3 GetMinCorners()
    {
        return corners[0];
    }

    void SetCorners()
    {
        rect.GetWorldCorners(corners);
    }
    
    
}
