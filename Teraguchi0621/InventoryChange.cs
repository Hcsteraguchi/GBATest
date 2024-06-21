using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryChange : MonoBehaviour
{
    [Header("対応させる画像")][SerializeField] private Image _nowInventory = default; 
    [Header("変更させる画像")][SerializeField] public List<Sprite> _inventoryes = default;
    [Header("頭上画像")] [SerializeField] private SpriteRenderer _playerSprite = default;
    [Header("画像番号")] public int _inventoryNum = default;
    [Header("頭上表示")] [SerializeField] private GameObject _playerInventory = default;
    [Header("頭上インベントリ")]　[SerializeField] private InventoryActive _inventoryActive = default;
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
        // 現在の画像を番号順に変える
        _nowInventory.sprite = _inventoryes[_inventoryNum];
        // プレイヤーの頭上に選択されている画像を表示
        _playerSprite.sprite = _inventoryes[_inventoryNum];
        // 表示するための制限時間をリセットする
        _inventoryActive._deleteTime = 0;
        // 頭上インベントリを表示する
        _playerInventory.SetActive(true);
    }
}
