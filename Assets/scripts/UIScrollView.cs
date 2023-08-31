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

    public int total_Item_Count;
    public Item itemPf;

    float spacing;

    int viewPortItemCnt;
    private void Start()
    {
        SetItemCnt();
    }
    void OnEnable()
    {
        _scrollRect = GetComponentInChildren<ScrollRect>();
        _scrollRect.viewport.GetWorldCorners(viewPortCorners);
        _scrollRect.onValueChanged.AddListener(OnValueChange);
        spacing = _scrollRect.content.GetComponent<VerticalLayoutGroup>().spacing;
    }
    
    Vector3 GetMaxViewPort()
    {
        return viewPortCorners[2];
    }
    Vector3 GetMinViewPort()
    {
        return viewPortCorners[0];
    }
    void OnValueChange(Vector2 normalizeddir)
    {
        SetScrollDirection();
        switch(scrollDir)
        {
            case EScrollDir.Up:
                RecycleButtomToTop();
                break;
            case EScrollDir.Down:
                RecycleTopToButtom();
                break;
        }
    }
    void SetScrollDirection()
    {
        if (_scrollRect.content.anchoredPosition.y - _prevContentAnchoredPosition.y < 0) // this equation is wrong 
        {
            scrollDir = EScrollDir.Up;
        }
        else
            scrollDir = EScrollDir.Down;

        _prevContentAnchoredPosition = _scrollRect.content.anchoredPosition;
    }
    void RecycleTopToButtom()
    {
        //direction down 
        //get corners
        var topObj = doublyLinkedList.GetObjectFromRoot(2);
        topObj.SetCorners();
        var buttomOBj = doublyLinkedList.GetObjectFromTail(2);
        buttomOBj.SetCorners();
        if(buttomOBj.GetCorners[2].y > GetMinViewPort().y)
        {
            var maxNum = int.Parse(doublyLinkedList.GetObjectFromTail().TitleTxt.text);
            int addNum = 0;
            //if(maxNum > total_Item_Count) 
            //{
            //    Debug.Log("Hmmmm");
            //    _scrollRect.StopMovement();
            //    _scrollRect.movementType = ScrollRect.MovementType.Clamped;
            //    return;
            //}
            //else
            //    _scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
            //Debug.Log(maxNum == total_Item_Count);
            //Debug.Log($"<color=cyan>{maxNum}  /  {total_Item_Count}</color>");
            //if (maxNum >= total_Item_Count)
            //{
            //    Debug.Log("Hum????");
            //    maxNum = -1;
            //}

            for (int i = 0; i < 2; i++)
            {
                ++addNum;

                var targetObj = doublyLinkedList.GetObjectFromRoot();
                var currentButtomObj = doublyLinkedList.GetObjectFromTail();
                var targetYPos = currentButtomObj.rect.anchoredPosition.y - (currentButtomObj.rect.rect.height + spacing);
                Vector2 targetPos = new Vector2(currentButtomObj.rect.anchoredPosition.x, targetYPos);

                targetObj.rect.anchoredPosition = targetPos;

                var currentNum = currentButtomObj.OrderNum +1;

                if (currentNum > total_Item_Count)
                    currentNum = 1;
                targetObj.SetTitle(currentNum.ToString());

                doublyLinkedList.root = doublyLinkedList.root.next;
                doublyLinkedList.tail = doublyLinkedList.tail.next;
            }
        }
    }
    void RecycleButtomToTop()
    {
        //direction down 
        //get corners
        var topObj = doublyLinkedList.GetObjectFromRoot(2);
        topObj.SetCorners();
        var buttomOBj = doublyLinkedList.GetObjectFromTail(2);
        buttomOBj.SetCorners();
        if (buttomOBj.GetCorners[2].y < GetMinViewPort().y) 
        {
            var minNum = int.Parse(doublyLinkedList.GetObjectFromRoot().TitleTxt.text);
            int subNum = 0;
         
            for (int i = 0; i < 2; i++)
            {
                ++subNum;
                    
                var targetObj = doublyLinkedList.GetObjectFromTail();
                var currentTopObjs = doublyLinkedList.GetObjectFromRoot();
                var targetYPos = currentTopObjs.rect.anchoredPosition.y + (currentTopObjs.rect.rect.height + spacing);
                Vector2 targetPos = new Vector2(currentTopObjs.rect.anchoredPosition.x, targetYPos);

                targetObj.rect.anchoredPosition = targetPos;

                var currentNum = currentTopObjs.OrderNum - 1;
                if (currentNum < 1)
                    currentNum = total_Item_Count;
                targetObj.SetTitle(currentNum.ToString());

                doublyLinkedList.root = doublyLinkedList.tail;
                doublyLinkedList.tail = doublyLinkedList.tail.prev;
            }
        }
    }

    void GenerateItem(int itemCnt)
    {
        for (int i = 1; i <= itemCnt; i++)
        {
            var item = Instantiate(itemPf.gameObject,_scrollRect.content);
            item.name = $"item {i}";
            item.SetActive(true);
            doublyLinkedList.Add(item.GetComponent<Item>());
            item.GetComponent<Item>().SetTitle(i.ToString());
        }
    }
    void SetItemCnt()
    {
        var viewHeight = _scrollRect.viewport.rect.height;
        var itemtHeight = itemPf.rect.rect.height;
        var spacing =_scrollRect.content.GetComponent<VerticalLayoutGroup>().spacing;
        var totalSpace = itemtHeight + spacing;

        viewPortItemCnt = Mathf.CeilToInt(viewHeight / totalSpace);
        GenerateItem(viewPortItemCnt +4);

    }

}
