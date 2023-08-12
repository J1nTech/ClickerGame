using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class UIScrollView : MonoBehaviour
{
    enum EScrollDir
    {
        Up,
        Down
    }

    Vector3[] viewPortCorners = new Vector3[4];
    ScrollRect _scrollRect;
    Vector2 _prevContentAnchoredPosition = Vector2.zero;
    EScrollDir scrollDir;

    DoublyLinkedList<Item> doublyLinkedList = new DoublyLinkedList<Item>();
    
    public DoublyLinkedList<Item> GetItemList => doublyLinkedList;

    public const int TOTAL_ITEM_COUNT = 18;
    public Item itemPf;

    private void Start()
    {
        SetItemCnt();
    }
    void OnEnable()
    {
        _scrollRect = GetComponentInChildren<ScrollRect>();
        _scrollRect.viewport.GetWorldCorners(viewPortCorners);
    }
    
    Vector3 GetMaxViewPort()
    {
        return viewPortCorners[2];
    }
    Vector3 GetMinViewPort()
    {
        return viewPortCorners[0];
    }
    void OnValueChange()
    {
        SetScrollDirection();
        switch(scrollDir)
        {
            case EScrollDir.Up:
                RecycleTopToButtom();
                break;
            case EScrollDir.Down:
                RecycleButtomToTop();
                break;
        }
    }
    void SetScrollDirection()
    {
        if (_scrollRect.content.anchoredPosition.y - _prevContentAnchoredPosition.y < 0)
            scrollDir = EScrollDir.Up;
        else
            scrollDir = EScrollDir.Down;
    }
    void RecycleTopToButtom()
    {

    }
    void RecycleButtomToTop()
    {

    }

    void GenerateItem(int itemCnt)
    {
        for (int i = 0; i < itemCnt; i++)
        {
            var item = Instantiate(itemPf.gameObject,_scrollRect.content);
            item.name = $"{item} {i}";
            item.SetActive(true);
            doublyLinkedList.Add(item.GetComponent<Item>());
        }
    }
    void SetItemCnt()
    {
        var viewHeight = _scrollRect.viewport.rect.height;
        var itemtHeight = itemPf.rect.rect.height;
        var spacing =_scrollRect.content.GetComponent<VerticalLayoutGroup>().spacing;
        var totalSpace = itemtHeight + spacing;

        int totalGeneratedItemCnt = Mathf.CeilToInt(viewHeight / totalSpace);
        Debug.Log(totalGeneratedItemCnt);
        GenerateItem(totalGeneratedItemCnt + 4);
    }

}
