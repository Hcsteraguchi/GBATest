using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class SubWeaponUI : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryCanvas = default;
    private PauseScene _pauseScene = default;
    private bool _isInventory = default;
    // Start is called before the first frame update
    void Start()
    {
        _pauseScene = gameObject.GetComponent<PauseScene>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void InventoryDisplay()
    {
        if (!_isInventory &&!_pauseScene._isPause) //ポーズ画面、またはインベントリ画面を開いてない場合
        {
            _inventoryCanvas.SetActive(true);
            Time.timeScale = 0f;
            _isInventory = true;
        }
        else
        {
            _inventoryCanvas.SetActive(false);
            Time.timeScale = 1f;
            _isInventory = false;
        }
    }
}
