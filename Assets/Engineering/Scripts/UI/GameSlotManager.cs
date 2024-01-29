using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ScriptableObjectDependencyInjection;

public class GameSlotManager : MonoBehaviour
{
    [SerializeField] GameObject ladder, ladder2, ladder3, highNoon1, highNoon2, highNoon3, tugOfWar1, tugOfWar2, tugOfWar3, pillar1, pillar2, pillar3;
    [SerializeField] private ScriptableArray gameSequence;
    public NullEvent nullEvent;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(nextSceneCountdown());
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneReference.SceneToSceneName[gameSequence.array[0]] == SceneReference.HighNoon)
        {
            highNoon1.SetActive(true);
        }
        else if (SceneReference.SceneToSceneName[gameSequence.array[0]] == SceneReference.TugOfWar)
        {
            tugOfWar1.SetActive(true);
        }
        else if (SceneReference.SceneToSceneName[gameSequence.array[0]] == SceneReference.Ladder)
        {
            ladder.SetActive(true);
        }
        else if (SceneReference.SceneToSceneName[gameSequence.array[0]] == SceneReference.WoodChop)
        {
            pillar1.SetActive(true);
        }

        if (SceneReference.SceneToSceneName[gameSequence.array[1]] == SceneReference.HighNoon)
        {
            highNoon2.SetActive(true);
        }
        else if (SceneReference.SceneToSceneName[gameSequence.array[1]] == SceneReference.TugOfWar)
        {
            tugOfWar2.SetActive(true);
        }
        else if (SceneReference.SceneToSceneName[gameSequence.array[1]] == SceneReference.Ladder)
        {
            ladder2.SetActive(true);
        }
        else if (SceneReference.SceneToSceneName[gameSequence.array[1]] == SceneReference.WoodChop)
        {
            pillar2.SetActive(true);
        }

        if (SceneReference.SceneToSceneName[gameSequence.array[2]] == SceneReference.HighNoon)
        {
            highNoon3.SetActive(true);
        }
        else if (SceneReference.SceneToSceneName[gameSequence.array[2]] == SceneReference.TugOfWar)
        {
            tugOfWar3.SetActive(true);
        }
        else if (SceneReference.SceneToSceneName[gameSequence.array[2]] == SceneReference.Ladder)
        {
            ladder3.SetActive(true);
        }
        else if (SceneReference.SceneToSceneName[gameSequence.array[2]] == SceneReference.WoodChop)
        {
            pillar3.SetActive(true);
        }

        
    }
    void NextScene()
    {
        nullEvent.Raise(null);
    }
    private IEnumerator nextSceneCountdown()
    {
        yield return new WaitForSeconds(10f);
        NextScene();

    }
}
