using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    #region //インスペクター変数
    [Header("インベントリ座標")][SerializeField] private RectTransform[] _arrowPos = default;　
    [Header("選択肢座標")][SerializeField] private Image _selectInventory = default;
    [Header("プレイヤーインベントリ")][SerializeField] private Image _nowInventory = default;
    [Header("スクリプト参照用オブジェクト")][SerializeField] private GameObject _gameController = default;
    [Header("ポーズ画面")][SerializeField] private PauseScene _pauseScene = default;
    [Header("インベントリ画面")][SerializeField] private SubWeaponUI _subWeaponUI = default;
    [Header("インベントリ内容")] [SerializeField] private InventoryChange _inventoryChange = default;
    [Header("宝箱インベントリ")] [SerializeField] public Image[] _inventoryes = default;
    #endregion
    // パブリック変数
    private int _selectNum = default; // 配列用数字

    // Start is called before the first frame update
    void Start()
    {
        _pauseScene = _gameController.GetComponent<PauseScene>();　
        _subWeaponUI = _gameController.GetComponent<SubWeaponUI>();
        _inventoryChange = _nowInventory.GetComponent<InventoryChange>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_pauseScene._isPause)　//ポーズ画面になっているかどうか
        {
            InventorySelect();
        }
    }
    private void InventorySelect()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow)) //右選択肢
        {
            _selectNum++;
            InventoryNumber();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))　//左選択肢
        {
            _selectNum--;
            if (_selectNum < 0)
            {
                _selectNum += _arrowPos.Length;
            }
            InventoryNumber();
        }
        if (Input.GetKeyDown(KeyCode.Return))　// 決定ボタン
        {
            InventoryDecision();
        }
    }
    private void InventoryNumber()
    {
        // 配列番号以上の数値だった場合、余剰に変換する
        _selectNum %= _arrowPos.Length;
        // 選択肢用画像をインベントリの位置に移動する
        _selectInventory.rectTransform.anchoredPosition = _arrowPos[_selectNum].anchoredPosition;
       
    }
    public void InventoryDecision()
    {
        _inventoryChange._inventoryes[_selectNum + 1] = _inventoryes[_selectNum].sprite;　// 選択画像を現在のインベントリに添付
        Debug.Log(_inventoryes[_selectNum].sprite);
        _subWeaponUI.InventoryDisplay();　// 決定後インベントリ画面を削除
    }
}
