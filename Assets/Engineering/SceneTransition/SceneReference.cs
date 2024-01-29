using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneReference
{
    public static string HighNoon = "HighNoon";
    public static string MainMenu = "Main Menu";
    public static string TugOfWar = "TugOfWar";
    public static string Ladder = "Ladder";
    public static string WoodChop = "WoodChop";
    public static string Credits = "Credits";
    public static string Score = "Score";
    public static string GameSlot = "Game Slot";

    public static Dictionary<int, string> SceneToSceneName = new Dictionary<int, string> {
        { 0, HighNoon },
        { 1, TugOfWar },
        { 2, Ladder },
        { 3, WoodChop }
    };
}


