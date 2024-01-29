using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToScoreScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ScoreScene()
    {
        TransitionManager.instance.LoadScene("Score");
        Debug.Log("ScoreScene");
    }
}
