using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBox : MonoBehaviour
{
    [Header("プレイヤーを入れる")] [SerializeField] private GameObject _player = null;
    [Header("この宝箱に入れるアイテムを格納するリスト")]
    [SerializeField] private List<Inventory.WeaponSelect> _itemBoxList =
        new List<Inventory.WeaponSelect>();
    [Header("今選ばれているアイテム")] [SerializeField] private int _item = 0;

    //宝箱開封中かどうかの判定
    public bool _openBox = default;
    //宝箱が速攻閉じないようにしてるテスト判定
    private bool _isCnt = default;

    //必要な他クラス
    Inventory _inventorycs;
    SubWeapon _subWeapon;
    private void Start()
    {
        _subWeapon = _player.GetComponent<SubWeapon>();
        _inventorycs = _player.GetComponent<Inventory>();
    }

    void Update()
    {
        if(_openBox)
        {
            WeaponGet();
        }
    }

    private void WeaponGet()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(_isCnt)
            {
                //二回目はここを通る
                print("宝箱閉じる");
                _isCnt = false;
                _subWeapon._isOpenBox = false;
                _openBox = false;
            }
          else
            {
                //一回目はここを通る
                _isCnt = true;
            }
        }
        //選ぶオブジェクトを右に
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (_item == _itemBoxList.Count-1)
            {
                _item = 0;
            }
            else
            {
                _item = _item+1;
            }
        }
        //選ぶオブジェクトを左に
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (_item == 0)
            {
                _item = _itemBoxList.Count-1;
            }
            else
            {
                _item = _item-1;
            }
        }
        //選択中の武器をインベントリに入れるように
        if (Input.GetKeyDown(KeyCode.G))
        {
            _inventorycs = _player.GetComponent<Inventory>();
            _inventorycs.InventBox(_itemBoxList[_item]);
        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")&&!_openBox)
        {
      
                if (Input.GetKey(KeyCode.F))
                {
                    print("宝箱開封");
                    _openBox = true;
                    _subWeapon._isOpenBox = true;
                }                   
        }
        /*宝箱のオブジェクトにプレイヤーが触れている間
         * 宝箱を開けるかどうかを受け付ける
         * もし開けた場合_openBoxを真にし
         * アップデート文を呼び出す
         */
    }
}
