using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    //�v���C���[�ɂ��Ă����N���X
    /// <summary>
    /// ���킲�ƂɐU��ꂽ�ŗL�ԍ��Ǘ��p��Enum
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
    //[Header("���݂̃C���x���g�����")]public WeaponSelect[]_inventory = { 0,0,0 };
    [Header("�C���x���g�����X�g�̍ő�")] private const int _maxIndex = 10;
    public int _maxIndexCnt = -1;
    public int _indexCnt = default;

    // �C���x���g���摜
    [Header("�Ή�������摜")] [SerializeField] private Image _nowInventory = default;

    [SerializeField] private InventoryChange _inventoryChange = default;

    private void Start()
    {
        _inventoryChange = _nowInventory.GetComponent<InventoryChange>();
    }
    public void InventBox(WeaponSelect select)
    {
        //�g�����邩�̊m�F�ϐ��i���ł͕K�v�Ȃ��j
        //int itemCnt = 0;
        //����I�΂ꂽ����

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


        ////�z��̈�ԍŌ�܂Ŕ�r���J��Ԃ�
        //for (int i = 0; i<=_maxIndex; i++)
        //{
        //    itemCnt++;
        //    //�C���x���g������������͊��ɕ�����������Ă����炻���ɕ�����i�[
        //    if (_inventory[i]==0||_inventory[i]==select)
        //    {
        //        _inventory[i] = select;
        //        break;
        //    }
        //}
        ////�C���x���g���ɋ󂫂��Ȃ������ꍇ�����������Ăяo��
        ////�܂������������łɉe���Ȃ�
        //if(itemCnt<=_maxIndex)
        //{
        //    print("�������Ȃ���");
        //}

    }
    public void ChangeWeapon()
    {
        /*R��L�ŃC���x���g�����E���ɑI������
        * �������ݑI�����Ă���C���x���g���ԍ����Ō�����邢�͐擪�̎�
        * �l�����Z�b�g����
        */
        if (Input.GetButtonDown("RightChange")/*Input.GetKeyDown(KeyCode.R)*/)
        {
            if (_indexCnt >= _maxIndexCnt)
            {
                _indexCnt = 0;
                _inventoryChange._inventoryNum = 0;//�z��ԍ������炵�ĉ摜��ύX������
                _inventoryChange.InventoryState();
                print("���݂̃C���x���g���ԍ�" + _indexCnt);
            }
            else
            {
                _inventoryChange._inventoryNum++; //�z��ԍ������炵�ĉ摜��ύX������
                _inventoryChange.InventoryState();
                _indexCnt = _indexCnt + 1;
                print("���݂̃C���x���g���ԍ�" + _indexCnt);
            }
        }
        if (Input.GetButtonDown("LeftChange")/*Input.GetKeyDown(KeyCode.L)*/)
        {
            if (_indexCnt <= 0)
            {
                _inventoryChange._inventoryNum = _maxIndexCnt; //�z��ԍ������炵�ĉ摜��ύX������
                _inventoryChange.InventoryState();
                _indexCnt = _maxIndexCnt;
                print("���݂̃C���x���g���ԍ�" + _indexCnt);
            }
            else
            {
                _inventoryChange._inventoryNum--; //�z��ԍ������炵�ĉ摜��ύX������
                _inventoryChange.InventoryState();
                _indexCnt = _indexCnt - 1;
                print("���݂̃C���x���g���ԍ�" + _indexCnt);
            }
        }
    }
}
