using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryChange : MonoBehaviour
{
    [Header("対応させる画像")][SerializeField] private Image _nowInventory = default; 
    [Header("変更させる画像")][SerializeField] private Sprite[] _inventoryes = default; 
    [Header("画像番号")]private int _inventoryNum = default;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 武器変更判定
        if(Input.GetKeyDown(KeyCode.LeftShift)) 
        {
            _inventoryNum++; //配列番号をずらして画像を変更させる
            InventoryState();
        }
    }
    private void InventoryState()
    {
        _inventoryNum %= _inventoryes.Length; 
        // 現在の画像を番号順に変える
        _nowInventory.sprite = _inventoryes[_inventoryNum];
    }
}
