using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinItem : Item
{
    Image _itemImg;

   void SetItem(Sprite itemImg)
    {
        _itemImg.sprite = itemImg;
    }

}
