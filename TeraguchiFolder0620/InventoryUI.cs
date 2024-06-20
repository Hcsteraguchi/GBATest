using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    #region //�C���X�y�N�^�[�ϐ�
    [Header("�C���x���g�����W")][SerializeField] private RectTransform[] _arrowPos = default;�@
    [Header("�I�������W")][SerializeField] private Image _selectInventory = default;
    [Header("�v���C���[�C���x���g��")][SerializeField] private Image _nowInventory = default;
    [Header("�X�N���v�g�Q�Ɨp�I�u�W�F�N�g")][SerializeField] private GameObject _gameController = default;
    [Header("�|�[�Y���")][SerializeField] private PauseScene _pauseScene = default;
    [Header("�C���x���g�����")][SerializeField] private SubWeaponUI _subWeaponUI = default;
    [Header("�C���x���g�����e")] [SerializeField] private InventoryChange _inventoryChange = default;
    [Header("�󔠃C���x���g��")] [SerializeField] public Image[] _inventoryes = default;
    #endregion
    // �p�u���b�N�ϐ�
    private int _selectNum = default; // �z��p����

    // Start is called before the first frame update
    void Start()
    {
        _pauseScene = _gameController.GetComponent<PauseScene>();�@
        _subWeaponUI = _gameController.GetComponent<SubWeaponUI>();
        _inventoryChange = _nowInventory.GetComponent<InventoryChange>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_pauseScene._isPause)�@//�|�[�Y��ʂɂȂ��Ă��邩�ǂ���
        {
            InventorySelect();
        }
    }
    private void InventorySelect()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow)) //�E�I����
        {
            _selectNum++;
            InventoryNumber();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))�@//���I����
        {
            _selectNum--;
            if (_selectNum < 0)
            {
                _selectNum += _arrowPos.Length;
            }
            InventoryNumber();
        }
        if (Input.GetKeyDown(KeyCode.Return))�@// ����{�^��
        {
            InventoryDecision();
        }
    }
    private void InventoryNumber()
    {
        // �z��ԍ��ȏ�̐��l�������ꍇ�A�]��ɕϊ�����
        _selectNum %= _arrowPos.Length;
        // �I�����p�摜���C���x���g���̈ʒu�Ɉړ�����
        _selectInventory.rectTransform.anchoredPosition = _arrowPos[_selectNum].anchoredPosition;
       
    }
    public void InventoryDecision()
    {
        _inventoryChange._inventoryes[_selectNum + 1] = _inventoryes[_selectNum].sprite;�@// �I���摜�����݂̃C���x���g���ɓY�t
        Debug.Log(_inventoryes[_selectNum].sprite);
        _subWeaponUI.InventoryDisplay();�@// �����C���x���g����ʂ��폜
    }
}
