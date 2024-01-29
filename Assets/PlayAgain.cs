using DG.Tweening;
using ScriptableObjectDependencyInjection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayAgain : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private RectTransform playagain;
    TransitionManager TransitionManager;
    Tween t;


    private void Start()
    {
        playagain.localScale = Vector3.one * 0.9f;
        t = playagain.DOScale(1.1f, 1f).SetLoops(-1, LoopType.Yoyo);


    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.anyKey.isPressed)
        {
            TransitionManager.instance.LoadScene(SceneReference.MainMenu);
        }
    }

   
}
