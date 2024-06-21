using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    //プレイヤーにつけていいクラス
    /// <summary>
    /// 武器ごとに振られた固有番号管理用のEnum
    /// </summary>
    public enum WeaponSelect
    {
        Nasi = 0,
        Kunai = 1,
        Boomerang = 2,
        Kamaitachi = 3,
        //以下テスト用削除可能
        test1=4,
        test2=5,
        test3=6,
        test4=7,
    }
    [Header("現在持っている武器")]
    [SerializeField]
    public List<WeaponSelect> _inventoryList =
        new List<WeaponSelect>();
    [Header("まだ入手していない武器")] public List<WeaponSelect> _notHaveWeapon;
    [Header("インベントリリストの最大")] private const int _maxIndex = 10;
    public int _maxIndexCnt = -1;
    private bool _isNothing = default;
    [SerializeField]public int _indexCnt = default;

    // インベントリ画像
    [Header("対応させる画像")] [SerializeField] private Image _nowInventory = default;

    [SerializeField] private InventoryChange _inventoryChange = default;

    private void Start()
    {
        //_inventoryChange = _nowInventory.GetComponent<InventoryChange>();
        _inventoryList.Add(WeaponSelect.Nasi);
    }
    public void InventBox(WeaponSelect select)
    {

        //int itemCnt = 0;
        //今回選ばれた武器

        print(select);
        _maxIndexCnt++;
        _inventoryList.Add(select);
        _notHaveWeapon.Remove(select);
        if(!_isNothing)
        {
            _isNothing = true;
            _inventoryList.Remove(WeaponSelect.Nasi);
        }

    }
    public void ChangeWeapon()
    {
        /*RとLでインベントリを右左に選択する
        * もし現在選択しているインベントリ番号が最後尾あるいは先頭の時
        * 値をリセットする
        */
        if (Input.GetButtonDown("RightChange")/*Input.GetKeyDown(KeyCode.R)*/)
        {
            if (_indexCnt >= _maxIndexCnt)
            {
                _indexCnt = 0;
                //_inventoryChange._inventoryNum = 0;//配列番号をずらして画像を変更させる
                //_inventoryChange.InventoryState();
                print("現在のインベントリ番号" + _indexCnt);
            }
            else
            {
                //_inventoryChange._inventoryNum++; //配列番号をずらして画像を変更させる
                //_inventoryChange.InventoryState();
                _indexCnt = _indexCnt + 1;
                print("現在のインベントリ番号" + _indexCnt);
            }
        }
        if (Input.GetButtonDown("LeftChange")/*Input.GetKeyDown(KeyCode.L)*/)
        {
            if (_indexCnt <= 0)
            {
                //_inventoryChange._inventoryNum = _maxIndexCnt; //配列番号をずらして画像を変更させる
                //_inventoryChange.InventoryState();
                _indexCnt = _maxIndexCnt;
                print("現在のインベントリ番号" + _indexCnt);
            }
            else
            {
                //_inventoryChange._inventoryNum--; //配列番号をずらして画像を変更させる
                //_inventoryChange.InventoryState();
                _indexCnt = _indexCnt - 1;
                print("現在のインベントリ番号" + _indexCnt);
            }
        }
    }
}
