using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class treasureBox : MonoBehaviour
{
    [SerializeField] GameObject _Invent = null;
    [SerializeField]
    public List<SubWeapon.WeaponSelect> _itemBoxList =
        new List<SubWeapon.WeaponSelect>();
    [SerializeField] private int _item = 0;
    private bool _openBox = default;

    Inventory inventorycs;
    //インベントリの配列[0]＝１はソードを格納
    //public int[] _inventory = { 1, 0, 0 };
    //[SerializeField] int item = default;

    void Update()
    {
        if(_openBox)
        {
            WeaponGet();
        }
    }

    private void WeaponGet()
    {
        if (Input.GetKeyDown(KeyCode.X))
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
        if (Input.GetKeyDown(KeyCode.Z))
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
        if (Input.GetKeyDown(KeyCode.G))
        {
            inventorycs = _Invent.GetComponent<Inventory>();
            print(_itemBoxList[_item]);
            inventorycs.InventBox(_itemBoxList[_item]);
        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!_openBox)
            {
                if (Input.GetKey(KeyCode.F))
                {
                    print("宝箱");
                    print(_itemBoxList.Count);
                    _openBox = true;
                }           
            }
        }
    }
}
