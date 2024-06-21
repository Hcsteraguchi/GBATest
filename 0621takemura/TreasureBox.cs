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
        //宝箱が開封されたら起動する
        if(_isopenBox)
        {
            WeaponGet();
        }
    }

    private void TreasureSet()
    {
        int listCnt = _itemBoxList.Count;
        listCnt = listCnt - 1;
        int startListCnt = listCnt;
        List<Inventory.WeaponSelect> copyList =
            new List<Inventory.WeaponSelect>(_inventorycs._notHaveWeapon);
        for (int i = 0; i <= _inventorycs._maxIndexCnt; i++)
        {
            for (int j = 0; j <= listCnt; j++)
            {
                if (_itemBoxList[j] == _inventorycs._inventoryList[i])
                {
                    _itemBoxList.Remove(_itemBoxList[j]);
                   
                    j = j - 1;
                    listCnt = listCnt - 1;
                }
            }
        }
        /*宝箱内のアイテムとインベントリ内のアイテムを比較して
       * 重複するものがあったら宝箱からそのアイテムを除外する
       */
        if (listCnt<startListCnt)
        {
            //宝箱に入ってるアイテムを除外する
            for (int i = 0; i < _itemBoxList.Count; i++)
            {
                for (int j = 0; j < copyList.Count; j++)
                {
                    if (_itemBoxList[i] == copyList[j])
                    {
                        copyList.Remove(copyList[j]);
                    }
                }
            }

            //足りない分を補充する
            for (int i = listCnt; i < startListCnt; i++)
            {
                int rndWeapon = Random.Range(0, copyList.Count);
                _itemBoxList.Add(copyList[rndWeapon]);
                copyList.Remove(copyList[rndWeapon]);
            }
        }
    }
    private void WeaponGet()
    {
        //選ぶオブジェクトを右に
        if (Input.GetButtonDown("RightChange"))
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
        if (Input.GetButtonDown("LeftChange"))
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
            this.gameObject.SetActive(false);
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
                }                   
        }
        /*宝箱のオブジェクトにプレイヤーが触れている間
         * 宝箱を開けるかどうかを受け付ける
         * もし開けた場合_openBoxを真にし
         * アップデート文を呼び出す
         */
    }
}
