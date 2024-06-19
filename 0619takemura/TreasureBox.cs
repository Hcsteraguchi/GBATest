using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBox : MonoBehaviour
{
    [Header("�v���C���[������")] [SerializeField] private GameObject _player = default;
    [Header("���̕󔠂ɓ����A�C�e�����i�[���郊�X�g")]
    [SerializeField] private List<Inventory.WeaponSelect> _itemBoxList =
        new List<Inventory.WeaponSelect>();
    [Header("���I�΂�Ă���A�C�e��")] [SerializeField] private int _item = 0;

    //�󔠊J�������ǂ����̔���
    public bool _isopenBox = default;
    //�󔠂����U���Ȃ��悤�ɂ��Ă�e�X�g����

    //�K�v�ȑ��N���X
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
        print("����");

       

    }
    private void WeaponGet()
    {
        //if (Input.GetButtonDown("Open"))
        //{       
        //        //���ڂ͂�����ʂ�
        //        print("�󔠕���");
        //        _isopenBox = false;
        //        _playerMove._isPlayeropenBox = false;
        //}
        //�I�ԃI�u�W�F�N�g���E��
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
        //�I�ԃI�u�W�F�N�g������
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
        //�I�𒆂̕�����C���x���g���ɓ����悤��
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
                    print("�󔠊J��");
                    TreasureSet();
                    _isopenBox = true;
                    _playerMove._isPlayeropenBox = true;
                 //   _subWeapon._isOpenBox = true;
                }                   
        }
        /*�󔠂̃I�u�W�F�N�g�Ƀv���C���[���G��Ă����
         * �󔠂��J���邩�ǂ������󂯕t����
         * �����J�����ꍇ_openBox��^�ɂ�
         * �A�b�v�f�[�g�����Ăяo��
         */
    }
}
