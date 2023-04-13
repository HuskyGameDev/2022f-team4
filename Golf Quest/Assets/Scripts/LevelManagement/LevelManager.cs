using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour {

    public static LevelManager Instance;

    [SerializeField]
    private Button btnTemplate;
    
    private GameObject levelList;

    [SerializeField]
    private Color lockedColor, unlockedColor, completeColor;

    public bool autoUnlockAll = false;

    [SerializeField]
    private Level[] levels;
    private Button[] levelBtns = new Button[0];
    private int[] Par;

    void Awake() {
        
        if(Instance != null) {

            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        levels[0].unlock();
    }

    void Update() {

        if(SceneManager.GetActiveScene().name.Equals("Title Screen")) {

            if(levelList == null)
                levelList = GameObject.Find("LevelList");

            if(levelList != null) {
                PopulateLevelList();

                if(getNextLevelIndex() >= 0)
                    levelList.GetComponentInParent<TitleScreenManager>().setPageDefaultSelection("LevelsPage", levelBtns[getNextLevelIndex()].gameObject);
            }
        }
    }

    public void CompleteLevel(string sceneName, int strokes) {

        for (int i = 0; i < levels.Length; i++) {

            if(levels[i].getName().Equals(sceneName)) {

                levels[i].completeLevel(strokes);

                if(i < levels.Length - 1)
                    levels[i + 1].unlock();
            }
        } 
    }

    public void LoadNextLevel() {

        getNextLevel().load();
    }

    public Level getNextLevel() {

        foreach (Level lvl in levels) {

            if (lvl.isComplete())
                continue;

            if (lvl.isUnlocked())
                return lvl;
        }

        return null;
    }

    public int getNextLevelIndex() {

        for (int i = 0; i < levels.Length; i++) {

            if (levels[i].isComplete())
                continue;

            if (levels[i].isUnlocked())
                return i;
        }

        return 0;
    }

    private void PopulateLevelList() {

        Par = new int[levels.Length];
        for (int i = 0; i < Par.Length; i++) {
            Par[i] = 5;
        }

        if (levelBtns.Length != levels.Length) {
            foreach (Transform btn in levelList.GetComponentsInChildren<Transform>())
                if(btn.name.Contains("Button"))
                    Destroy(btn.gameObject);

            levelBtns = new Button[levels.Length];
        }

        for (int i = 0; i < levels.Length; i++) {
            
            if(autoUnlockAll)
                levels[i].unlock();

            if(levelBtns[i] == null)
                levelBtns[i] = Instantiate(btnTemplate, levelList.transform);

            Button btn = levelBtns[i];
            Level lvl = levels[i];

            btn.name = lvl.getName() + " Button";
            btn.transform.Find("LevelName").GetComponent<TextMeshProUGUI>().SetText(lvl.getName());
            btn.transform.Find("Par").GetComponent<TextMeshProUGUI>().SetText("Par: " + Par[i]);
            btn.transform.Find("BestStrokes").GetComponent<TextMeshProUGUI>().SetText("Strokes:\n" + lvl.getBestStrokes().ToString());
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(delegate { lvl.load(); });

            if(lvl.isUnlocked()) {
                btn.interactable = true;
                btn.image.color = unlockedColor;
            } else {
                btn.interactable = false;
                btn.image.color = lockedColor;
            }

            if(lvl.isComplete())
                btn.image.color = completeColor;
        }
    }

    public Level[] getLevels() { return levels; }
}