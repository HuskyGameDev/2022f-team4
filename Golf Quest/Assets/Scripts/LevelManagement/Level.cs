using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class Level {

    public string sceneName;
    private float bestTime;
    private int bestStrokes;
    private bool unlocked, complete;    

    public void load() {

        SceneManager.LoadScene(sceneName);
    }

    public void unlock() { unlocked = true; }

    public void completeLevel(float time, int strokes) {

        if(!complete) {

            bestTime = time;
            bestStrokes = strokes;
        } else {

            bestTime = Mathf.Min(bestTime, time);
            bestStrokes = (int) Mathf.Min(bestStrokes, strokes);
        }

        complete = true;
    }

    public string getName() { return sceneName; }
    public float getBestTime() { return bestTime; }
    public int getBestStrokes() { return bestStrokes; }
    public bool isUnlocked() { return unlocked; }
    public bool isComplete() { return complete; }
}