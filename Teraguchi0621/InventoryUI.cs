using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    #region //�C���X�y�N�^�[�ϐ�
    [Header("�C���x���g�����W")][SerializeField] private List<RectTransform> _arrowPos = default;�@
    [Header("�I�������W")][SerializeField] private Image _selectInventory = default;
    [Header("�v���C���[�C���x���g��")][SerializeField] private Image _nowInventory = default;
    [Header("GameController���A�^�b�`")][SerializeField] private GameObject _gameController = default;
    [Header("�|�[�Y���")][SerializeField] private PauseScene _pauseScene = default;
    [Header("�C���x���g�����")][SerializeField] private SubWeaponUI _subWeaponUI = default;
    [Header("�C���x���g�����e")] [SerializeField] private InventoryChange _inventoryChange = default;
    [Header("�󔠃C���x���g��")] [SerializeField] public List<Image> _inventoryes = default;
    [Header("TresureBox�X�N���v�g���A�^�b�`")]�@[SerializeField]
    private TreasureBox _treasureBoxScript = default;
    [Header("TresureBox�I�u�W�F�N�g���A�^�b�`")] [SerializeField] 
    GameObject _tresureBoxObject = default;
    #endregion
    // �p�u���b�N�ϐ�
    //private int _selectNum = default; // �z��p����

    // Start is called before the first frame update
    void Start()
    {
        _pauseScene = _gameController.GetComponent<PauseScene>();�@
        _subWeaponUI = _gameController.GetComponent<SubWeaponUI>();
        _inventoryChange = _nowInventory.GetComponent<InventoryChange>();
        _treasureBoxScript = _tresureBoxObject.GetComponent<TreasureBox>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    public void InventoryNumber()
    {
       
        // �I�����p�摜���C���x���g���̈ʒu�Ɉړ�����
        _selectInventory.rectTransform.anchoredPosition = 
            _arrowPos[_treasureBoxScript._item].anchoredPosition;
       
    }
    public void InventoryDecision()
    {
        // �C���x���g���z����X�^�b�N
        _inventoryChange._inventoryes.Add(_inventoryChange._inventoryes[_inventoryChange._inventoryNum]);
        // �I���摜�����݂̃C���x���g���ɓY�t
        _inventoryChange._inventoryes[_inventoryChange._inventoryNum + 1] =
            _inventoryes[_treasureBoxScript._item].sprite;�@
        Debug.Log(_inventoryes[_treasureBoxScript._item].sprite);
        _subWeaponUI.InventoryDisplay();�@// �����C���x���g����ʂ��폜
    }
}
