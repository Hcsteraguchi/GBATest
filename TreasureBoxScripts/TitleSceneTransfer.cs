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

    private float _verticalPower;//�W���C�X�e�B�b�N�㉺���͂̐��l���i�オ���j
    bool _canSelect = true;
    // Start is called before the first frame update
    void Start()
    {
        _upPos = _upPosText.GetComponent<RectTransform>(); //��I����
        _downPos = _downPosText.GetComponent<RectTransform>(); //���I����
    }

    // Update is called once per frame
    void Update()
    {
        // �W���C�X�e�B�b�N�E�E���̐��l��
        _verticalPower = Input.GetAxisRaw("Vertical");
        // �I���L�[
        if ((_verticalPower > 0.5f || _verticalPower < -0.5f) && _canSelect/*Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)*/)
        {
            _canSelect = false;
            SelectScene();
        }
        else if (_verticalPower == 0 && !_canSelect)
    {
            _canSelect = true;
    }
        // ����L�[
        if (Input.GetButtonDown("Attack"))
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
            Time.timeScale = 1f;
            //���C���V�[���Ɉڍs
            SceneManager.LoadScene("MainScene");�@
        }
        else
        {
            Time.timeScale = 1f;
            //�^�C�g���V�[���Ɉڍs
            SceneManager.LoadScene("TitleScene");
        }
    }
}
