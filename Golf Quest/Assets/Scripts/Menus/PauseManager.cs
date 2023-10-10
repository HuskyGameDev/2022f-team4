using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour {

    private BallMovement ball;
    private InputActionAsset input;
    private Image bg, levelCompleteMenu, deathMenu;
    private GameObject panel, resumeButton;
    private static bool paused;
    [SerializeField] AudioSource pauseSFX;

    void Awake() {

        ball = GameObject.Find("Player Ball").GetComponent<BallMovement>();
        input = EventSystem.current.GetComponent<InputSystemUIInputModule>().actionsAsset;
        bg = GetComponent<Image>();
        panel = transform.GetChild(0).gameObject;
        resumeButton = panel.transform.GetChild(0).gameObject;
        levelCompleteMenu = GameObject.Find("LevelCompletedMenu").GetComponent<Image>();
        deathMenu = GameObject.Find("DeathMenu").GetComponent<Image>();
        Resume();

        input.FindAction("Cancel").performed += input => { if (paused && input.control.path != "/Keyboard/escape") { Resume(); } };
        input.FindAction("Pause").performed += input => { 
            
            if (this == null || ball.isAiming() && !ball.isMoving() && !ball.isLaunching())
                return;

            if (paused)
                Resume();
            else
                Pause();
        };
    }
    
    public void Pause() {

        pauseSFX.Play();

        if(levelCompleteMenu.IsActive() || deathMenu.IsActive())
            return;

        if (bg == null || panel == null)
            return;

        bg.enabled = true;
        panel.SetActive(true);
        TimeManager.Pause();
        paused = true;
        EventSystem.current.SetSelectedGameObject(resumeButton);
    }

    public void Resume() {

        if (bg == null || panel == null)
            return;

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

    public static bool isPaused() { return paused; }
}
