using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DeadRevolver.PixelPrototypePlayer
{
    public class DemoUIManager : MonoBehaviour
    {
        // アニメーション名を表示するテキストUI
        public TextMeshProUGUI label;

        // アニメーションの現在の番号と総数を表示するテキストUI
        public TextMeshProUGUI indexLabel;

        // アニメーションが変更されたときに呼ばれるメソッド
        public void OnAnimationChanged(PlayerPreviewAnimation animation, int currentAnimation, List<PlayerPreviewAnimation> animations)
        {
            // アニメーション名を大文字に変換して表示
            label.text = animation.name.ToUpper();

            // 現在のアニメーション番号（1から開始）と総数を表示
            indexLabel.text = (currentAnimation + 1) + " / " + animations.Count;
        }
    }
}