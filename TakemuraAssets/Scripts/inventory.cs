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
        nasi = 0,
        Kunai = 1,
        _boomerang = 2,
        kamaitachi = 3,
    }
    [Header("現在のインベントリ情報")]public WeaponSelect[]_inventory = { 0,0,0 };
    [Header("インベントリ配列のの最後尾")] public int _maxIndex = 2;
    public void InventBox(WeaponSelect select)
    {
        //枠があるかの確認変数（α版は必要なし）
        int itemCnt = 0;
        //今回選ばれた武器
        print(select);

        //配列の一番最後まで比較を繰り返す
        for (int i = 0; i<=_maxIndex; i++)
        {
            itemCnt++;
            //インベントリが空もしくは既に武器を所持していたらそこに武器を格納
            if (_inventory[i]==0||_inventory[i]==select)
            {
                _inventory[i] = select;
                break;
            }
        }
        //インベントリに空きがなかった場合交換処理を呼び出す
        //まだ未完成＆α版に影響なし
        if(itemCnt<=_maxIndex)
        {
            print("交換しなさい");
        }
     
    }
}
