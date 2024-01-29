using DG.Tweening;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class RandomScene : MonoBehaviour
{

    [SerializeField] private RectTransform clickAnythingTitle;

    Tween t;

    bool start = false;
    bool started = false;
    private void Start() {
        clickAnythingTitle.localScale = Vector3.one * 0.9f;
        t = clickAnythingTitle.DOScale(1.1f, 1f).SetLoops(-1, LoopType.Yoyo);


    }
    private void LateUpdate() {
        if (started) return;
        //bool controllerAny = Gamepad.current.allControls.Any(x => x is ButtonControl button && button.isPressed && !x.synthetic);
        if (Keyboard.current.anyKey.isPressed) {
            start = true;
        }

        if (start) {
            TransitionManager.Instance.SetUpScenesAndRun();
            ButtonControl b;

            t.Kill();
            float tar = clickAnythingTitle.localScale.x - 0.3f;
            clickAnythingTitle.DOScale(tar, 0.3f);

            started = true;
            
        }
    }
   
}
