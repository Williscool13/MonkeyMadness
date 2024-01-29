using DG.Tweening;
using ScriptableObjectDependencyInjection;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{

    public static TransitionManager instance;

    [SerializeField] private ScriptableArray gameSequence;
    [SerializeField] private IntegerVariable gameIndex;
    [SerializeField] int gameCount = 3;

    public static TransitionManager Instance {
        get {
            if (instance == null) {
                Debug.LogError("No scene manager exists in this scene, add one please");
                return null;
            }
            return instance;
        }
    }


    private void Awake() {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }


        instance = this;
        DontDestroyOnLoad(gameObject);

        circleWipeMaterial.SetFloat("_Radius", 0);
        DOTween.To(() => circleWipeMaterial.GetFloat("_Radius"), x => circleWipeMaterial.SetFloat("_Radius", x), 1, transitionDuration);
    }




    [Title("Transition Properties")]
    [SerializeField] float transitionDuration = 1f;
    [SerializeField] Material circleWipeMaterial;



    Sequence currentTransition;
    // make this more elaborate/sophisticated when transition style is determined

    public void LoadScene(string sceneName, Vector2 transitionFocus) {
        if (currentTransition != null) {
            Debug.Log("Canceled previous ongoing transition and started new one");
            currentTransition.Kill();
        }

        currentTransition = DOTween.Sequence()
            .Append(DOTween.To(() => circleWipeMaterial.GetFloat("_Radius"), x => circleWipeMaterial.SetFloat("_Radius", x), 0, transitionDuration))
            .AppendInterval(0.25f)
            .AppendCallback(() => SceneManager.LoadScene(sceneName))
            .AppendInterval(1.0f)
            .Append(DOTween.To(() => circleWipeMaterial.GetFloat("_Radius"), x => circleWipeMaterial.SetFloat("_Radius", x), 1, transitionDuration));
    }
    public void LoadScene(string sceneName) {
        LoadScene(sceneName, Vector2.zero);
    }

    [Title("Debug Mode")]
    [SerializeField] bool debugMode;
    [ShowIf("debugMode")]
    [SerializeField] string targetSceneName;
    [Button("Test Transition")]
    public void TestTransition() {
        LoadScene(targetSceneName);
    }


    public void GameOverLoadNextScene() {
        if (Score.Instance.Player1Score == 2 || Score.Instance.Player2Score == 2) {
            LoadCredits();
            return;
        }
        if (gameIndex.Value == gameCount) {
            LoadCredits();
            return; 
        }
        
        
        
        int currentGame = gameSequence.array[gameIndex.Value];
        
        gameIndex.Value++;
        Instance.LoadScene(SceneReference.SceneToSceneName[currentGame]);

    }

    void LoadCredits() {
        Instance.LoadScene(SceneReference.Credits);

    }


    public void SetUpScenesAndRun() {
        gameIndex.Value = 0;

        List<int> gameBacklog = new();
        List<int> gameFuture = new();
        foreach (KeyValuePair<int, string> entry in SceneReference.SceneToSceneName) {
            gameBacklog.Add(entry.Key);
        }

        for (int i = 0; i < gameCount; i++) {
            int index = Random.Range(0, gameBacklog.Count);
            gameFuture.Add(gameBacklog[index]);
            gameBacklog.RemoveAt(index);
        }



        gameSequence.array = gameFuture.ToArray();

        LoadScene(SceneReference.GameSlot);
    }
}
