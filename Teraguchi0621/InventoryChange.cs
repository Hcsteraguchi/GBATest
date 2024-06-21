using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryChange : MonoBehaviour
{
    [Header("�Ή�������摜")][SerializeField] private Image _nowInventory = default; 
    [Header("�ύX������摜")][SerializeField] public List<Sprite> _inventoryes = default;
    [Header("����摜")] [SerializeField] private SpriteRenderer _playerSprite = default;
    [Header("�摜�ԍ�")] public int _inventoryNum = default;
    [Header("����\��")] [SerializeField] private GameObject _playerInventory = default;
    [Header("����C���x���g��")]�@[SerializeField] private InventoryActive _inventoryActive = default;
    // Start is called before the first frame update
    void Start()
    {
        _playerSprite = _playerInventory.GetComponent<SpriteRenderer>();
        _inventoryActive = _playerInventory.GetComponent<InventoryActive>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InventoryState()
    {
        // ���݂̉摜��ԍ����ɕς���
        _nowInventory.sprite = _inventoryes[_inventoryNum];
        // �v���C���[�̓���ɑI������Ă���摜��\��
        _playerSprite.sprite = _inventoryes[_inventoryNum];
        // �\�����邽�߂̐������Ԃ����Z�b�g����
        _inventoryActive._deleteTime = 0;
        // ����C���x���g����\������
        _playerInventory.SetActive(true);
    }
}
