using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.PlayerLoop;

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
    // �X�v���C�g���X�g�Ɨ񋓌^���X�g
    [Header("�C���x���g���̃X�v���C�g")][SerializeField] public List<Sprite> _inventorySprite = default;
    // �X�v���C�g�Ɨ񋓌^�̑Ή��֌W�������Ƃ��Ē�`
    [Header("�C���x���g���̃��X�g")]
    [SerializeField]
    public Dictionary<Sprite, Inventory.WeaponSelect> _inventorySpriteList
        = new Dictionary<Sprite, Inventory.WeaponSelect>();
    [Header("inventory�X�N���v�g���A�^�b�`")]
    [SerializeField] private Inventory _inventoryScript = default;
    [Header("TresureBox�X�N���v�g���A�^�b�`")]�@[SerializeField]
    private TreasureBox _treasureBoxScript = default;
    [Header("TresureBox�I�u�W�F�N�g���A�^�b�`")] [SerializeField] 
    GameObject _tresureBoxObject = default;
    #endregion
  

    // Start is called before the first frame update
    void Start()
    {
        _pauseScene = _gameController.GetComponent<PauseScene>();�@
        _subWeaponUI = _gameController.GetComponent<SubWeaponUI>();
        _inventoryChange = _nowInventory.GetComponent<InventoryChange>();
        _treasureBoxScript = _tresureBoxObject.GetComponent<TreasureBox>();
        // �X�v���C�g�Ɨ񋓌^�̑Ή��֌W��������
        SpriteenumInventory();
        // �X�v���C�g���X�g����񋓌^���X�g�ւ̕ϊ�
        _inventoryScript._inventoryList = ConverterSpriteToenums(_inventorySprite);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpriteenumInventory()
    {
        // ���񂱂��C��
        _inventorySpriteList[_inventorySprite[0]] = Inventory.WeaponSelect.Nasi;
        //_inventorySpriteList[_inventorySprite[1]] = Inventory.WeaponSelect.Kunai;
        //_inventorySpriteList[_inventorySprite[2]] = Inventory.WeaponSelect.Boomerang;
        //_inventorySpriteList[_inventorySprite[3]] = Inventory.WeaponSelect.Kamaitachi;
        //_inventorySpriteList[_inventorySprite[4]] = Inventory.WeaponSelect.test1;
        //_inventorySpriteList[_inventorySprite[5]] = Inventory.WeaponSelect.test2;
        //_inventorySpriteList[_inventorySprite[6]] = Inventory.WeaponSelect.test3;
        //_inventorySpriteList[_inventorySprite[7]] = Inventory.WeaponSelect.test4;
    }

    public List<Inventory.WeaponSelect> ConverterSpriteToenums(List<Sprite> sprites)
    {
        List<Inventory.WeaponSelect> enums = new List<Inventory.WeaponSelect>();
        foreach (Sprite sprite in sprites)
        {
            if (_inventorySpriteList.TryGetValue(sprite, out Inventory.WeaponSelect enumValue))
            {
                enums.Add(enumValue);
                Debug.Log(enums);
            }
        }
        return enums;
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
        Debug.Log(_treasureBoxScript._item);
        _subWeaponUI.InventoryDisplay();�@// �����C���x���g����ʂ��폜
    }
}
