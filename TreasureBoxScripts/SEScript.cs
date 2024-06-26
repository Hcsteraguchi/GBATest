using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEScript : MonoBehaviour
{
    AudioSource _audioSourceSE;
    [Header("SE�\�[�h")] [SerializeField] AudioClip _audioClipSword;
    [Header("SE�W�����v")] [SerializeField] AudioClip _audioClipJump;
    [Header("SE�m�b�N�o�b�N")] [SerializeField] AudioClip _audioClipKnockBack;
    // Start is called before the first frame update
    void Start()
    {
        _audioSourceSE = GetComponent<AudioSource>();
    }
    public void SwordSE() {
        _audioSourceSE.PlayOneShot(_audioClipSword);
    }
    public void JumpSE() {
        _audioSourceSE.PlayOneShot(_audioClipJump);
    }
    public void KnockBackSE() {
        _audioSourceSE.PlayOneShot(_audioClipKnockBack);
    }
}
