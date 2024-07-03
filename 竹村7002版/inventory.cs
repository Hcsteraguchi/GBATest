using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Fire = 4,
    }
    [Header("���ݎ����Ă��镐��")]
    [SerializeField]
    public List<WeaponSelect> _inventoryList =
       new List<WeaponSelect>();
    [Header("�܂����肵�Ă��Ȃ�����")] public List<WeaponSelect> _notHaveWeapon;
    [Header("�C���x���g�����X�g�̍ő�")] private const int _maxIndex = 10;
    private int _maxIndexCnt = -1;
    public int _getMaxIndex
    {
        get { return _maxIndexCnt; }
    }
    private bool _isNothing = default;
    [SerializeField] public int _indexCnt = default;

    // �C���x���g���摜
    [SerializeField] private InventUItest _invebtUI;

    private void Start()
    {
        _inventoryList.Add(WeaponSelect.Nasi);
    }
    public void InventBox(WeaponSelect select)
    {
        //����I�΂ꂽ����select
        _maxIndexCnt++;
        _inventoryList.Add(select);
        _notHaveWeapon.Remove(select);
        if (!_isNothing)
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
                _invebtUI.NowInventry(_indexCnt);
                //_inventoryChange._inventoryNum = 0;//�z��ԍ������炵�ĉ摜��ύX������
                //_inventoryChange.InventoryState();
                print("���݂̃C���x���g���ԍ�" + _indexCnt);
            }
            else
            {
                //_inventoryChange._inventoryNum++; //�z��ԍ������炵�ĉ摜��ύX������
                //_inventoryChange.InventoryState();
                _indexCnt = _indexCnt + 1;
                _invebtUI.NowInventry(_indexCnt);
                print("���݂̃C���x���g��ban��" + _indexCnt);
            }
        }
        if (Input.GetButtonDown("LeftChange")/*Input.GetKeyDown(KeyCode.L)*/)
        {
            if (_indexCnt <= 0)
            {
                if (_maxIndexCnt != -1)
                {
                    _indexCnt = _maxIndexCnt;
                    _invebtUI.NowInventry(_indexCnt);
                    print("���݂̃C���x���g���ԍ�" + _indexCnt);
                }
                //_inventoryChange._inventoryNum = _maxIndexCnt; //�z��ԍ������炵�ĉ摜��ύX������
                //_inventoryChange.InventoryState();            
            }
            else
            {
                //_inventoryChange._inventoryNum--; //�z��ԍ������炵�ĉ摜��ύX������
                //_inventoryChange.InventoryState();
                _indexCnt = _indexCnt - 1;
                _invebtUI.NowInventry(_indexCnt);
                print("���݂̃C���x���g���ԍ�" + _indexCnt);
            }
        }
    }
}
