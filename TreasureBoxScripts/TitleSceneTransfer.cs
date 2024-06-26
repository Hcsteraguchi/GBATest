using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TitleSceneTransfer : MonoBehaviour
{
    // インスペクター変数
    [Header("選択肢用画像")][SerializeField] private Image _selectImage = default;
    [Header("上選択肢用テキスト")][SerializeField] private TextMeshProUGUI _upPosText = default;
    [Header("下選択肢用テキスト")] [SerializeField] private TextMeshProUGUI _downPosText = default;
    [Header("上選択肢用座標")] [SerializeField] private RectTransform _upPos = default;
    [Header("下選択肢用テキスト")] [SerializeField] private RectTransform _downPos = default;
    // プライベート変数
    private bool _isSelect = default;

    private float _verticalPower;//ジョイスティック上下入力の数値化（上が正）
    bool _canSelect = true;
    // Start is called before the first frame update
    void Start()
    {
        _upPos = _upPosText.GetComponent<RectTransform>(); //上選択肢
        _downPos = _downPosText.GetComponent<RectTransform>(); //下選択肢
    }

    // Update is called once per frame
    void Update()
    {
        // ジョイスティック右・左の数値化
        _verticalPower = Input.GetAxisRaw("Vertical");
        // 選択キー
        if ((_verticalPower > 0.5f || _verticalPower < -0.5f) && _canSelect/*Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)*/)
        {
            _canSelect = false;
            SelectScene();
        }
        else if (_verticalPower == 0 && !_canSelect)
    {
            _canSelect = true;
    }
        // 決定キー
        if (Input.GetButtonDown("Attack"))
        {
            ChangeScene();
        }
    }
    private void SelectScene()
    {
        // 上選択肢
        if (_isSelect)
        {
            _selectImage.rectTransform.anchoredPosition = _upPos.anchoredPosition;
            _isSelect = false;
        }
        // 下選択肢
        else
        {
            _selectImage.rectTransform.anchoredPosition = _downPos.anchoredPosition;
            _isSelect = true;
        }
    }
    private void ChangeScene()
    {
        if(_isSelect)
        {
            Time.timeScale = 1f;
            //メインシーンに移行
            SceneManager.LoadScene("MainScene");　
        }
        else
        {
            Time.timeScale = 1f;
            //タイトルシーンに移行
            SceneManager.LoadScene("TitleScene");
        }
    }
}
