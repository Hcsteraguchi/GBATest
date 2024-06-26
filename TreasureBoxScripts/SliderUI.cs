using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SliderUI : MonoBehaviour
{
    private Slider _slider = default;
    [Header("�Ή�����X���C�_�[")] [SerializeField] private GameObject _nowSlider = default;
    [Header("����������l")] [SerializeField] private int _downValue = default;
    // Start is called before the first frame update
    void Start()
    {
        _slider = _nowSlider.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        //�{���̓v���C���[��HP�܂��͕���G�l���M�[���Q�Ƃ�����
        if (Input.GetKeyDown(KeyCode.A)) 
        {
            DownSlider();
        }
    }
    public void DownSlider()
    {
        // �X���C�_�[�̒��g������������
        _slider.value -= _downValue;
    }
}
