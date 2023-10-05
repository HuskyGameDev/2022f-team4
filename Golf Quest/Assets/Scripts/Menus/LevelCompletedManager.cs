using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class LevelCompletedManager : MonoBehaviour {

    private Image bg;
    private GameObject panel;
    public TextMeshProUGUI nameLabel, timeLabel, strokesLabel;
    public Button nextLevelBtn, exitBtn;

    private BallStats stats;

    void Start() {

        bg = GetComponent<Image>();
        panel = transform.GetChild(0).gameObject;

        stats = GameObject.Find("Player Ball").GetComponent<BallStats>();

        if(LevelManager.Instance.getLevels()[LevelManager.Instance.getLevels().Length - 1].getName().Equals(SceneManager.GetActiveScene().name))
            nextLevelBtn.gameObject.SetActive(false);

        Disable();
    }

    public void LevelCompleted() {

        string name = SceneManager.GetActiveScene().name;
        float time = stats.getElapsedTime();
        int strokes = stats.getStrokeCount();

        nameLabel.SetText(name);
        timeLabel.SetText(TimeManager.formatTime(time));
        strokesLabel.SetText(strokes.ToString());

        bg.enabled = true;
        panel.SetActive(true);

        LevelManager.Instance.CompleteLevel(name, time, strokes);
        TimeManager.Pause();

        if(nextLevelBtn.IsActive())
            EventSystem.current.SetSelectedGameObject(nextLevelBtn.gameObject);
        else
            EventSystem.current.SetSelectedGameObject(exitBtn.gameObject);
    }

    public void NextLevel() {

        for (int i = 0; i < LevelManager.Instance.getLevels().Length - 1; i++) {

            Level[] levels = LevelManager.Instance.getLevels();

            if(levels[i].getName().Equals(SceneManager.GetActiveScene().name))
                levels[i + 1].load();
        }
    }

    public void Disable() {

        bg.enabled = false;
        panel.SetActive(false);
    }
}
