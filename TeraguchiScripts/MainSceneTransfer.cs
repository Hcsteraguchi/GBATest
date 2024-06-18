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
    // Start is called before the first frame update
    void Start()
    {
        _upPos = _upPosText.GetComponent<RectTransform>(); //��I�����̃e�L�X�g�ʒu���擾
        _downPos = _downPosText.GetComponent<RectTransform>(); // �����̃e�L�X�g�ʒu���擾
    }

    // Update is called once per frame
    void Update()
    {
        // �I�����ړ�
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            SelectPos();
        }
        // ����{�^��
        if (Input.GetKeyDown(KeyCode.Return))
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
