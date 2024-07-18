using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Fire = 4,
    }
    [Header("現在持っている武器")]
    [SerializeField]
    public List<WeaponSelect> _inventoryList =
       new List<WeaponSelect>();
    [Header("まだ入手していない武器")] public List<WeaponSelect> _notHaveWeapon;
    [Header("インベントリリストの最大")] private const int _maxIndex = 10;
    private int _maxIndexCnt = -1;
    public int _getMaxIndex
    {
        get { return _maxIndexCnt; }
    }
    private bool _isNothing = default;
    [SerializeField] public int _indexCnt = default;

    // インベントリ画像
    [SerializeField] private InventUItest _invebtUI;

    private void Start()
    {
        _inventoryList.Add(WeaponSelect.Nasi);
    }
    public void InventBox(WeaponSelect select)
    {
        //今回選ばれた武器select
        _maxIndexCnt++;
        _inventoryList.Add(select);
        _notHaveWeapon.Remove(select);
        if (!_isNothing)
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
                _invebtUI.NowInventry(_indexCnt);
                //_inventoryChange._inventoryNum = 0;//配列番号をずらして画像を変更させる
                //_inventoryChange.InventoryState();
                print("現在のインベントリ番号" + _indexCnt);
            }
            else
            {
                //_inventoryChange._inventoryNum++; //配列番号をずらして画像を変更させる
                //_inventoryChange.InventoryState();
                _indexCnt = _indexCnt + 1;
                _invebtUI.NowInventry(_indexCnt);
                print("現在のインベントリban号" + _indexCnt);
            }
        }
        if (Input.GetButtonDown("LeftChange")/*Input.GetKeyDown(KeyCode.L)*/)
        {
            if (_indexCnt <= 0)
            {
                if (_maxIndexCnt != -1)
                {
                    _indexCnt = _maxIndexCnt;
                    _invebtUI.NowInventry(_indexCnt);
                    print("現在のインベントリ番号" + _indexCnt);
                }
                //_inventoryChange._inventoryNum = _maxIndexCnt; //配列番号をずらして画像を変更させる
                //_inventoryChange.InventoryState();            
            }
            else
            {
                //_inventoryChange._inventoryNum--; //配列番号をずらして画像を変更させる
                //_inventoryChange.InventoryState();
                _indexCnt = _indexCnt - 1;
                _invebtUI.NowInventry(_indexCnt);
                print("現在のインベントリ番号" + _indexCnt);
            }
        }
    }
}
