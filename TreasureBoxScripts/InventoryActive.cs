using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryActive : MonoBehaviour
{
    [Header("表示場所")] [SerializeField] private Vector3 _displayPos = default;
    [Header("消去時間")][SerializeField] private float _activeTime = default;
    public float _deleteTime = default;
    private GameObject _playerObject = default;
    // Start is called before the first frame update
    void Start()
    {
        _playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // プレイヤーの位置座標を更新し続ける
        Vector3 playerPos = _playerObject.transform.localPosition;
        // インベントリを入れ替えた瞬間に計測開始
        _deleteTime += Time.deltaTime;
        // プレイヤーの頭上に配置
        gameObject.transform.position = playerPos + _displayPos;
        if(_activeTime < _deleteTime) // 時間経過後インベントリをfalseにする
        {
            this.gameObject.SetActive(false);
        }
    }
  
}
