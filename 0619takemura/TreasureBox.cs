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
        //�󔠂��J�����ꂽ��N������
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
        /*�󔠓��̃A�C�e���ƃC���x���g�����̃A�C�e�����r����
       * �d��������̂���������󔠂��炻�̃A�C�e�������O����
       */
        if (listCnt<startListCnt)
        {
            //�󔠂ɓ����Ă�A�C�e�������O����
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

            //����Ȃ������[����
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
        //�I�ԃI�u�W�F�N�g���E��
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
        //�I�ԃI�u�W�F�N�g������
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
        //�I�𒆂̕�����C���x���g���ɓ����悤��
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
                    print("�󔠊J��");
                    TreasureSet();
                    _isopenBox = true;
                    _playerMove._isPlayeropenBox = true;
                }                   
        }
        /*�󔠂̃I�u�W�F�N�g�Ƀv���C���[���G��Ă����
         * �󔠂��J���邩�ǂ������󂯕t����
         * �����J�����ꍇ_openBox��^�ɂ�
         * �A�b�v�f�[�g�����Ăяo��
         */
    }
}
