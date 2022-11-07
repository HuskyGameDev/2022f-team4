using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour {

    private Image bg;
    private GameObject panel;
    private static bool paused;

    void Start() {

        bg = GetComponent<Image>();
        panel = transform.GetChild(0).gameObject;
        Resume();
    }

    void Update() {

        if (Input.GetKeyDown("escape")) {

            if (paused)
                Resume();
            else
                Pause();
        }
    }
    
    public void Pause() {

        bg.enabled = true;
        panel.SetActive(true);
        TimeManager.Pause();
        paused = true;
    }

    public void Resume() {

        bg.enabled = false;
        panel.SetActive(false);
        TimeManager.Resume();
        paused = false;
    }

    public void Restart() {

        TimeManager.Resume();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Exit() {

        TimeManager.Resume();
        SceneManager.LoadScene("Title Screen");
    }
}
