using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainSceneTransfer : MonoBehaviour
{
    private bool _isSelect = default;
    [Header("�I�����p�摜")] [SerializeField] private Image _selectImage = default;
    [Header("��I�����p�e�L�X�g")] [SerializeField] private TextMeshProUGUI _upPosText = default;
    [Header("���I�����p�e�L�X�g")] [SerializeField] private TextMeshProUGUI _downPosText = default;
    [Header("��I�����p���W")] [SerializeField] private RectTransform _upPos = default;
    [Header("���I�����p�e�L�X�g")] [SerializeField] private RectTransform _downPos = default;

    private float _verticalPower;//�W���C�X�e�B�b�N�㉺���͂̐��l���i�オ���j
    bool _canSelect = true;
    // Start is called before the first frame update
    void Start()
    {
        _upPos = _upPosText.GetComponent<RectTransform>(); //��I�����̃e�L�X�g�ʒu���擾
        _downPos = _downPosText.GetComponent<RectTransform>(); // �����̃e�L�X�g�ʒu���擾
    }

    // Update is called once per frame
    void Update()
    {
        // �W���C�X�e�B�b�N�E�E���̐��l��
        _verticalPower = Input.GetAxisRaw("Vertical");

        // �I�����ړ�
        if ((_verticalPower > 0.5f || _verticalPower < -0.5f) && _canSelect)
        {
            _canSelect = false;
            SelectPos();
        }
        else if (_verticalPower == 0 && !_canSelect)
        {
            _canSelect = true;
        }

        // ����{�^��
        if (Input.GetButtonDown("Attack"))
        {
            ChangeScene();
        }
    }
    
    private void SelectPos()
    {
       
        if (_isSelect) // ��I����
        {
            print("Title");
            _selectImage.rectTransform.anchoredPosition = _upPos.anchoredPosition;
            _isSelect = false;
        }
        else // ���I����
        {
            print("reset");
            _selectImage.rectTransform.anchoredPosition = _downPos.anchoredPosition;
            _isSelect = true;
        }
    }
    private void ChangeScene()
    {
        // �Q�[���{�҂Ɉڍs
        if (!_isSelect)
        {
            SceneManager.LoadScene("MainScene");
        }
         // �Q�[���I��
        else
        {
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
