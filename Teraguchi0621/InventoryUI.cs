using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    [Header("TresureBoxスクリプトをアタッチ")]　[SerializeField]
    private TreasureBox _treasureBoxScript = default;
    [Header("TresureBoxオブジェクトをアタッチ")] [SerializeField] 
    GameObject _tresureBoxObject = default;
    #endregion
    // パブリック変数
    //private int _selectNum = default; // 配列用数字

    // Start is called before the first frame update
    void Start()
    {
        _pauseScene = _gameController.GetComponent<PauseScene>();　
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
        Debug.Log(_inventoryes[_treasureBoxScript._item].sprite);
        _subWeaponUI.InventoryDisplay();　// 決定後インベントリ画面を削除
    }
}
