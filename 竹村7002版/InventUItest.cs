using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventUItest : MonoBehaviour
{
    //イメージ配列というやつ
    [SerializeField] private List<Sprite> _itemSprites = default;
    [SerializeField] private int _listNo = default;

    //持ち物リスト
    [SerializeField] private List<Sprite> _nowItem = default;
    private SpriteRenderer _ITemSprite = default;
    private bool _isFirstItem = true;

    // 宝箱リスト
    [SerializeField] private GameObject _tresureBoxUi = default;
    [SerializeField] private GameObject[] _treasureBox =
        new GameObject[3];
    [SerializeField]Inventory _inventorycs;
    private void Start()
    {
        _ITemSprite=GetComponent<SpriteRenderer>();
        _nowItem.Add(_itemSprites[0]);
        _ITemSprite.sprite = _itemSprites[0];
        _tresureBoxUi.SetActive(false);
    }
    public void NowInventry(int indexCnt)
    {
        _ITemSprite.sprite = _nowItem[indexCnt];
    }
    public void AddItem(Inventory.WeaponSelect Select)
    {
        print(Select);
        _tresureBoxUi.SetActive(false);
        int enumNo = default;
        enumNo= (int)Select;
        _nowItem.Add(_itemSprites[enumNo]);
        if(_isFirstItem)
        {
            _nowItem.Remove(_nowItem[0]);
            _ITemSprite.sprite = _nowItem[0];
            _isFirstItem = false;
        }
    }
    public void Tresure(List<Inventory.WeaponSelect>tresureList)
    {
        SpriteRenderer sprite;
        _tresureBoxUi.SetActive(true);
        for(int i = 0; i <=2; i++)
        {       
            sprite = _treasureBox[i].GetComponent<SpriteRenderer>();
            sprite.sprite = _itemSprites[(int)tresureList[i]];
        }
    }
}

