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
        //�ȉ��e�X�g�p�폜�\
        test1=4,
        test2=5,
        test3=6,
        test4=7,
    }
    [Header("���ݎ����Ă��镐��")]
    [SerializeField]
    public List<WeaponSelect> _inventoryList =
        new List<WeaponSelect>();
    [Header("�܂����肵�Ă��Ȃ�����")] public List<WeaponSelect> _notHaveWeapon;
    [Header("�C���x���g�����X�g�̍ő�")] private const int _maxIndex = 10;
    public int _maxIndexCnt = -1;
    private bool _isNothing = default;
    [SerializeField]public int _indexCnt = default;

    // �C���x���g���摜
    [Header("�Ή�������摜")] [SerializeField] private Image _nowInventory = default;

    [SerializeField] private InventoryChange _inventoryChange = default;

    private void Start()
    {
        //_inventoryChange = _nowInventory.GetComponent<InventoryChange>();
        _inventoryList.Add(WeaponSelect.Nasi);
    }
    public void InventBox(WeaponSelect select)
    {

        //int itemCnt = 0;
        //����I�΂ꂽ����

        print(select);
        _maxIndexCnt++;
        _inventoryList.Add(select);
        _notHaveWeapon.Remove(select);
        if(!_isNothing)
        {
            _isNothing = true;
            _inventoryList.Remove(WeaponSelect.Nasi);
        }

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
                //_inventoryChange._inventoryNum = 0;//�z��ԍ������炵�ĉ摜��ύX������
                //_inventoryChange.InventoryState();
                print("���݂̃C���x���g���ԍ�" + _indexCnt);
            }
            else
            {
                //_inventoryChange._inventoryNum++; //�z��ԍ������炵�ĉ摜��ύX������
                //_inventoryChange.InventoryState();
                _indexCnt = _indexCnt + 1;
                print("���݂̃C���x���g���ԍ�" + _indexCnt);
            }
        }
        if (Input.GetButtonDown("LeftChange")/*Input.GetKeyDown(KeyCode.L)*/)
        {
            if (_indexCnt <= 0)
            {
                //_inventoryChange._inventoryNum = _maxIndexCnt; //�z��ԍ������炵�ĉ摜��ύX������
                //_inventoryChange.InventoryState();
                _indexCnt = _maxIndexCnt;
                print("���݂̃C���x���g���ԍ�" + _indexCnt);
            }
            else
            {
                //_inventoryChange._inventoryNum--; //�z��ԍ������炵�ĉ摜��ύX������
                //_inventoryChange.InventoryState();
                _indexCnt = _indexCnt - 1;
                print("���݂̃C���x���g���ԍ�" + _indexCnt);
            }
        }
    }
}
