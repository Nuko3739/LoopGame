using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DeadRevolver.PixelPrototypePlayer
{
    public class DemoUIManager : MonoBehaviour
    {
        // �A�j���[�V��������\������e�L�X�gUI
        public TextMeshProUGUI label;

        // �A�j���[�V�����̌��݂̔ԍ��Ƒ�����\������e�L�X�gUI
        public TextMeshProUGUI indexLabel;

        // �A�j���[�V�������ύX���ꂽ�Ƃ��ɌĂ΂�郁�\�b�h
        public void OnAnimationChanged(PlayerPreviewAnimation animation, int currentAnimation, List<PlayerPreviewAnimation> animations)
        {
            // �A�j���[�V��������啶���ɕϊ����ĕ\��
            label.text = animation.name.ToUpper();

            // ���݂̃A�j���[�V�����ԍ��i1����J�n�j�Ƒ�����\��
            indexLabel.text = (currentAnimation + 1) + " / " + animations.Count;
        }
    }
}