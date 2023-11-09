using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    private Image bg;    
    private GameObject panel;
    public Button continueButton;

    private static bool inTutorial;
    
    void Start()
    {
        bg = GetComponent<Image>();
        panel = transform.GetChild(0).gameObject;

        if (LevelManager.Instance.getLevels()[0].getName().Equals(SceneManager.GetActiveScene().name)) {
            Tutorial();
        }

    }

    void Tutorial() {
        bg.enabled = true;
        panel.SetActive(true);

        inTutorial = true;

        TimeManager.Pause();
    }

    public void Continue() {
        //pauseSFX.Play();

        if (bg == null || panel == null)
            return;

        bg.enabled = false;
        panel.SetActive(false);
        TimeManager.Resume();

        inTutorial = false;
    }

    public static bool isInTutorial() { return inTutorial; }

}
