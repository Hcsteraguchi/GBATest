using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryChange : MonoBehaviour
{
    [Header("�Ή�������摜")][SerializeField] private Image _nowInventory = default; 
    [Header("�ύX������摜")][SerializeField] private Sprite[] _inventoryes = default; 
    [Header("�摜�ԍ�")]private int _inventoryNum = default;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ����ύX����
        if(Input.GetKeyDown(KeyCode.LeftShift)) 
        {
            _inventoryNum++; //�z��ԍ������炵�ĉ摜��ύX������
            InventoryState();
        }
    }
    private void InventoryState()
    {
        _inventoryNum %= _inventoryes.Length; 
        // ���݂̉摜��ԍ����ɕς���
        _nowInventory.sprite = _inventoryes[_inventoryNum];
    }
}
