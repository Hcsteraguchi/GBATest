using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainSceneTransfer : MonoBehaviour
{
    private bool _isSelect = default;
    [Header("選択肢用画像")] [SerializeField] private Image _selectImage = default;
    [Header("上選択肢用テキスト")] [SerializeField] private TextMeshProUGUI _upPosText = default;
    [Header("下選択肢用テキスト")] [SerializeField] private TextMeshProUGUI _downPosText = default;
    [Header("上選択肢用座標")] [SerializeField] private RectTransform _upPos = default;
    [Header("下選択肢用テキスト")] [SerializeField] private RectTransform _downPos = default;

    private float _verticalPower;//ジョイスティック上下入力の数値化（上が正）
    bool _canSelect = true;
    // Start is called before the first frame update
    void Start()
    {
        _upPos = _upPosText.GetComponent<RectTransform>(); //上選択肢のテキスト位置を取得
        _downPos = _downPosText.GetComponent<RectTransform>(); // 下側のテキスト位置を取得
    }

    // Update is called once per frame
    void Update()
    {
        // ジョイスティック右・左の数値化
        _verticalPower = Input.GetAxisRaw("Vertical");

        // 選択肢移動
        if ((_verticalPower > 0.5f || _verticalPower < -0.5f) && _canSelect)
        {
            _canSelect = false;
            SelectPos();
        }
        else if (_verticalPower == 0 && !_canSelect)
        {
            _canSelect = true;
        }

        // 決定ボタン
        if (Input.GetButtonDown("Attack"))
        {
            ChangeScene();
        }
    }
    
    private void SelectPos()
    {
       
        if (_isSelect) // 上選択肢
        {
            print("Title");
            _selectImage.rectTransform.anchoredPosition = _upPos.anchoredPosition;
            _isSelect = false;
        }
        else // 下選択肢
        {
            print("reset");
            _selectImage.rectTransform.anchoredPosition = _downPos.anchoredPosition;
            _isSelect = true;
        }
    }
    private void ChangeScene()
    {
        // ゲーム本編に移行
        if (!_isSelect)
        {
            SceneManager.LoadScene("MainScene");
        }
         // ゲーム終了
        else
        {
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
