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
    }
    [SerializeField]
    public List<Inventory.WeaponSelect> _inventoryList =
        new List<Inventory.WeaponSelect>();
    //[Header("現在のインベントリ情報")]public WeaponSelect[]_inventory = { 0,0,0 };
    [Header("インベントリリストの最大")] private const int _maxIndex = 10;
    public int _maxIndexCnt = -1;
    public int _indexCnt = default;

    // インベントリ画像
    [Header("対応させる画像")] [SerializeField] private Image _nowInventory = default;

    [SerializeField] private InventoryChange _inventoryChange = default;

    private void Start()
    {
        _inventoryChange = _nowInventory.GetComponent<InventoryChange>();
    }
    public void InventBox(WeaponSelect select)
    {
        //枠があるかの確認変数（α版は必要なし）
        //int itemCnt = 0;
        //今回選ばれた武器

        print(select);
        _maxIndexCnt++;
        _inventoryList.Add(select);
     
        //for (int i = 0; i <= _maxIndex; i++)
        //{
        //    if (_inventoryList[i] == select)
        //    {
        //        _inventoryList[i] = select;
        //        _maxIndexCnt++;
        //       // break;
        //    }
        //}


        ////配列の一番最後まで比較を繰り返す
        //for (int i = 0; i<=_maxIndex; i++)
        //{
        //    itemCnt++;
        //    //インベントリが空もしくは既に武器を所持していたらそこに武器を格納
        //    if (_inventory[i]==0||_inventory[i]==select)
        //    {
        //        _inventory[i] = select;
        //        break;
        //    }
        //}
        ////インベントリに空きがなかった場合交換処理を呼び出す
        ////まだ未完成＆α版に影響なし
        //if(itemCnt<=_maxIndex)
        //{
        //    print("交換しなさい");
        //}

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
                _inventoryChange._inventoryNum = 0;//配列番号をずらして画像を変更させる
                _inventoryChange.InventoryState();
                print("現在のインベントリ番号" + _indexCnt);
            }
            else
            {
                _inventoryChange._inventoryNum++; //配列番号をずらして画像を変更させる
                _inventoryChange.InventoryState();
                _indexCnt = _indexCnt + 1;
                print("現在のインベントリ番号" + _indexCnt);
            }
        }
        if (Input.GetButtonDown("LeftChange")/*Input.GetKeyDown(KeyCode.L)*/)
        {
            if (_indexCnt <= 0)
            {
                _inventoryChange._inventoryNum = _maxIndexCnt; //配列番号をずらして画像を変更させる
                _inventoryChange.InventoryState();
                _indexCnt = _maxIndexCnt;
                print("現在のインベントリ番号" + _indexCnt);
            }
            else
            {
                _inventoryChange._inventoryNum--; //配列番号をずらして画像を変更させる
                _inventoryChange.InventoryState();
                _indexCnt = _indexCnt - 1;
                print("現在のインベントリ番号" + _indexCnt);
            }
        }
    }
}
