using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.PlayerLoop;

public class InventoryUI : MonoBehaviour
{
    #region //インスペクター変数
    [Header("インベントリ座標")][SerializeField] private List<RectTransform> _arrowPos = default;　
    [Header("選択肢座標")][SerializeField] private Image _selectInventory = default;
    [Header("プレイヤーインベントリ")][SerializeField] private Image _nowInventory = default;
    [Header("GameControllerをアタッチ")][SerializeField] private GameObject _gameController = default;
    [Header("ポーズ画面")][SerializeField] private PauseScene _pauseScene = default;
    [Header("インベントリ画面")][SerializeField] private SubWeaponUI _subWeaponUI = default;
    [Header("インベントリ内容")] [SerializeField] private InventoryChange _inventoryChange = default;
    [Header("宝箱インベントリ")] [SerializeField] public List<Image> _inventoryes = default;
    // スプライトリストと列挙型リスト
    [Header("インベントリのスプライト")][SerializeField] public List<Sprite> _inventorySprite = default;
    // スプライトと列挙型の対応関係を辞書として定義
    [Header("インベントリのリスト")]
    [SerializeField]
    public Dictionary<Sprite, Inventory.WeaponSelect> _inventorySpriteList
        = new Dictionary<Sprite, Inventory.WeaponSelect>();
    [Header("inventoryスクリプトをアタッチ")]
    [SerializeField] private Inventory _inventoryScript = default;
    [Header("TresureBoxスクリプトをアタッチ")]　[SerializeField]
    private TreasureBox _treasureBoxScript = default;
    [Header("TresureBoxオブジェクトをアタッチ")] [SerializeField] 
    GameObject _tresureBoxObject = default;
    #endregion
  

    // Start is called before the first frame update
    void Start()
    {
        _pauseScene = _gameController.GetComponent<PauseScene>();　
        _subWeaponUI = _gameController.GetComponent<SubWeaponUI>();
        _inventoryChange = _nowInventory.GetComponent<InventoryChange>();
        _treasureBoxScript = _tresureBoxObject.GetComponent<TreasureBox>();
        // スプライトと列挙型の対応関係を初期化
        SpriteenumInventory();
        // スプライトリストから列挙型リストへの変換
        _inventoryScript._inventoryList = ConverterSpriteToenums(_inventorySprite);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpriteenumInventory()
    {
        // 次回ここ修正
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
        // 選択肢用画像をインベントリの位置に移動する
        _selectInventory.rectTransform.anchoredPosition = 
            _arrowPos[_treasureBoxScript._item].anchoredPosition;
    }
    public void InventoryDecision()
    {
        // インベントリ配列をスタック
        _inventoryChange._inventoryes.Add(_inventoryChange._inventoryes[_inventoryChange._inventoryNum]);
        // 選択画像を現在のインベントリに添付
        _inventoryChange._inventoryes[_inventoryChange._inventoryNum + 1] =
            _inventoryes[_treasureBoxScript._item].sprite;　
        Debug.Log(_treasureBoxScript._item);
        _subWeaponUI.InventoryDisplay();　// 決定後インベントリ画面を削除
    }
}
