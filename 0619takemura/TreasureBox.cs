using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBox : MonoBehaviour
{
    [Header("プレイヤーを入れる")] [SerializeField] private GameObject _player = default;
    [Header("この宝箱に入れるアイテムを格納するリスト")]
    [SerializeField] private List<Inventory.WeaponSelect> _itemBoxList =
        new List<Inventory.WeaponSelect>();
    [Header("今選ばれているアイテム")] [SerializeField] private int _item = 0;

    //宝箱開封中かどうかの判定
    public bool _isopenBox = default;
    //宝箱が速攻閉じないようにしてるテスト判定

    //必要な他クラス
    Inventory _inventorycs;
    PlayerMove_KM _playerMove;
    private void Start()
    {
      //  _subWeapon = _player.GetComponent<SubWeapon>();
        _inventorycs = _player.GetComponent<Inventory>();
        _playerMove = _player.GetComponent<PlayerMove_KM>();

       
    }

    void Update()
    {
        if(_isopenBox)
        {
            WeaponGet();
        }
    }

    private void TreasureSet()
    {
        int listCnt = _itemBoxList.Count;
        listCnt = listCnt - 1;
        for (int j = 0; j <= _inventorycs._maxIndexCnt; j++)
        {
            for (int i = 0; i <= listCnt; i++)
            {
                if (_itemBoxList[i] == _inventorycs._inventoryList[j])
                {
                    _itemBoxList.Remove(_itemBoxList[i]);
                    i = i - 1;
                    listCnt = listCnt - 1;
                }
            }
        }
        print("成功");

       

    }
    private void WeaponGet()
    {
        //if (Input.GetButtonDown("Open"))
        //{       
        //        //二回目はここを通る
        //        print("宝箱閉じる");
        //        _isopenBox = false;
        //        _playerMove._isPlayeropenBox = false;
        //}
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
        if (Input.GetButtonDown("Attack"))
        {
            _inventorycs = _player.GetComponent<Inventory>();
            _inventorycs.InventBox(_itemBoxList[_item]);
            _playerMove._isPlayeropenBox = false;
            _isopenBox = false;
        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")&&!_isopenBox)
        {    
                if (Input.GetButton("Open"))
                {
                    print("宝箱開封");
                    TreasureSet();
                    _isopenBox = true;
                    _playerMove._isPlayeropenBox = true;
                 //   _subWeapon._isOpenBox = true;
                }                   
        }
        /*宝箱のオブジェクトにプレイヤーが触れている間
         * 宝箱を開けるかどうかを受け付ける
         * もし開けた場合_openBoxを真にし
         * アップデート文を呼び出す
         */
    }
}
