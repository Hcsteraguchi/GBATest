using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TitleSceneTransfer : MonoBehaviour
{
    // �C���X�y�N�^�[�ϐ�
    [Header("�I�����p�摜")][SerializeField] private Image _selectImage = default;
    [Header("��I�����p�e�L�X�g")][SerializeField] private TextMeshProUGUI _upPosText = default;
    [Header("���I�����p�e�L�X�g")] [SerializeField] private TextMeshProUGUI _downPosText = default;
    [Header("��I�����p���W")] [SerializeField] private RectTransform _upPos = default;
    [Header("���I�����p�e�L�X�g")] [SerializeField] private RectTransform _downPos = default;
    // �v���C�x�[�g�ϐ�
    private bool _isSelect = default;
    // Start is called before the first frame update
    void Start()
    {
        _upPos = _upPosText.GetComponent<RectTransform>(); //��I����
        _downPos = _downPosText.GetComponent<RectTransform>(); //���I����
    }

    // Update is called once per frame
    void Update()
    {
        // �I���L�[
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            SelectScene();
        }
        // ����L�[
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ChangeScene();
        }
    }
    private void SelectScene()
    {
        // ��I����
        if (_isSelect)
        {
            _selectImage.rectTransform.anchoredPosition = _upPos.anchoredPosition;
            _isSelect = false;
        }
        // ���I����
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
            //���C���V�[���Ɉڍs
            SceneManager.LoadScene("MainScene");�@
        }
        else
        {
            //�^�C�g���V�[���Ɉڍs
            SceneManager.LoadScene("TitleScene");
        }
    }
}
