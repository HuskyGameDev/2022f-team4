using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleScreenManager : MonoBehaviour {

    private TextMeshProUGUI playBtnLabel;

    void Start() {

        playBtnLabel = GameObject.Find("Play").GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update() {

        if (LevelManager.Instance.getLevels()[0].isComplete() && !LevelManager.Instance.getLevels()[LevelManager.Instance.getLevels().Length - 1].isComplete())
            playBtnLabel.SetText("Continue: " + LevelManager.Instance.getNextLevel().getName());
    }

    public void Play() {

        if (!LevelManager.Instance.getLevels()[LevelManager.Instance.getLevels().Length - 1].isComplete())
            LevelManager.Instance.LoadNextLevel();
        else
            LevelManager.Instance.getLevels()[0].load();
    }

    public void Quit() {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
