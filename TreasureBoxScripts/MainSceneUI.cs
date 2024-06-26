using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneUI : MonoBehaviour
{
    private PauseScene _pauseScene = default;
    [Header("インベントリ画面に移動")][SerializeField] private SubWeaponUI _subWeaponUI;
    [Header("対応するインベントリ画面")][SerializeField] private GameObject _inventoryCanvas;
    // Start is called before the first frame update
    void Start()
    {
        _pauseScene = GetComponent<PauseScene>();
        _subWeaponUI = GetComponent<SubWeaponUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Menu")) //ポーズ画面用スクリプトを呼び出す
        {
            _pauseScene.SelectPause();
        }
        if(Input.GetKeyDown(KeyCode.RightShift)) //サブウェポン用スクリプトを呼びだす
        {
            _subWeaponUI.InventoryDisplay();
        }

    }
}
