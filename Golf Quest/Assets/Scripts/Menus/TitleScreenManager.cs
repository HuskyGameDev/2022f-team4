using System;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.InputSystem;

[Serializable]
public struct Page {

    public GameObject page, backPage, defaultSelection;
    public bool defaultToPreviouslySelected;
}

public class TitleScreenManager : MonoBehaviour {

    private TextMeshProUGUI playBtnLabel;

    private InputAction input_Cancel;

    public Page[] pages;
    public int currentPageIndex;

    void Start() {

        playBtnLabel = GameObject.Find("Play").GetComponentInChildren<TextMeshProUGUI>();
        input_Cancel = EventSystem.current.GetComponent<InputSystemUIInputModule>().actionsAsset.FindAction("UI/Cancel");
    }

    void Update() {

        if (playBtnLabel != null && LevelManager.Instance.getLevels()[0].isComplete() && !LevelManager.Instance.getLevels()[LevelManager.Instance.getLevels().Length - 1].isComplete())
            playBtnLabel.SetText("Continue: " + LevelManager.Instance.getNextLevel().getName());

        if (input_Cancel != null && input_Cancel.inProgress) {
            Back();
        }
    }

    public void Play() {

        if (!LevelManager.Instance.getLevels()[LevelManager.Instance.getLevels().Length - 1].isComplete())
            LevelManager.Instance.LoadNextLevel();
        else
            LevelManager.Instance.getLevels()[0].load();
    }

    public void Back() {
        
        if(pages[currentPageIndex].backPage != null)
            ChangePage(pages[currentPageIndex].backPage.name);
    }

    public void ChangePage(string pageName) {

        if(pages[currentPageIndex].defaultToPreviouslySelected)
            pages[currentPageIndex].defaultSelection = EventSystem.current.currentSelectedGameObject;

        for (int i = 0; i < pages.Length; i++) {
            
            Page page = pages[i];

            if(!page.page.name.Equals(pageName))
                page.page.SetActive(false);
            else {

                currentPageIndex = i;
                page.page.SetActive(true);
                EventSystem.current.SetSelectedGameObject(page.defaultSelection.gameObject);
            }
        }
    }

    public void Quit() {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void setPageDefaultSelection(String pageName, GameObject newDefaultSelection) {

        for (int i = 0; i < pages.Length; i++) {
            if(pages[i].page.name.Equals(pageName) && pages[i].defaultSelection == null) {
                pages[i].defaultSelection = newDefaultSelection;
                EventSystem.current.SetSelectedGameObject(newDefaultSelection);
            }
        }
    }
}
