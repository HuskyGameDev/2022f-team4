using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BallStats), typeof(Rigidbody))]
public class BallMovement : MonoBehaviour {

    private Rigidbody rigidBody;
    private LineRenderer pullLine;
    private BallStats ballStats;
    private PlayerInput input;
    private InputAction input_Aim, input_Launch, input_Cancel;

    private Plane plane;
    public float speed;
    private Vector3 aimStartPos;   // Stores the position of where the mouse drag was started.
    private Vector3 aimCurrPos;    // Stores the position of the mouse during the drag.
    private bool aiming;          // Stores whether or not the player is currently aiming.
    private bool moving;
    private bool launching;
    public Transform sprite;

    [Header("Launch Properties")]
    [SerializeField]
    private float magnitudeMax = 20.0f, magnitudeScalar = 100.0f, movingThreshold = 0.1f;
    [SerializeField]
    private SpriteRenderer readySprite; // Displays when they player is ready to be charged and launched again.
    [Header("Sound Components")]
    [SerializeField] private AudioSource uiAudioSource;
    [SerializeField] private AudioClip launchSound;
    [SerializeField] private AudioClip chargeSound;
    private bool chargeSoundPlayed;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        pullLine = GetComponentInChildren<LineRenderer>();
        ballStats = GetComponent<BallStats>();
        input = GetComponent<PlayerInput>();
        pullLine.enabled = false; 
        readySprite.enabled = false;
        chargeSoundPlayed = false;

        input_Aim = input.actions.FindAction("Aim");
        input_Launch = input.actions.FindAction("Launch");
        input_Cancel = input.actions.FindAction("Cancel");
        StartCoroutine(CalcSpeed());

        input_Cancel.performed += inputContext => {

            if (this == null)
                return;

            if (input.currentControlScheme.Equals("Gamepad") && lockRotation)
                lockRotation = false;

            ResetLaunch();
        };
        
        input_Launch.performed += inputContext => {
            
            if (this == null || !aiming || moving || inputContext.action.bindings[inputContext.action.GetBindingIndexForControl(inputContext.control)].effectivePath.Contains("Mouse"))
                return;

            if (input.currentControlScheme.Equals("Gamepad") && ControlsManager.Instance.getStyle() == ControlStyle.TwoStep && !lockRotation)
                lockRotation = true;
            else
                Launch();
        };
        
        input_Launch.started += inputContext => {

            if (this == null || moving || (input != null && input.currentControlScheme.Equals("Gamepad")))                           // Only allow aiming if ball has stopped moving
                return;

            aimStartPos = this.transform.position;                                                // Store the start aim position
            lockRotation = false;
            aiming = true;                                                                        // Started aiming
        };

        input_Launch.canceled += inputContext => {

            if (this == null || !aiming || moving || input.currentControlScheme.Equals("Gamepad"))
                return;

            Launch();
        };
    }

    // Update is called once per frame
    void Update() {

        bool ip = PauseManager.isPaused();
        bool ic = LevelCompletedManager.isCompleted();
        bool it = TutorialManager.isInTutorial();

        if (ip || ic || it)
            return;
        
        if (input.currentControlScheme.Equals("Keyboard&Mouse") || input.currentControlScheme.Equals("Touch"))
            PointerInput();
        else if (input.currentControlScheme.Equals("Gamepad"))
            GamepadInput();

        if(aiming) {
            sprite.GetComponent<Animator>().SetBool("squash",true);
            pullLine.enabled = true;
            pullLine.SetPositions(new Vector3[] {aimStartPos, aimCurrPos});
            if(!uiAudioSource.isPlaying && !chargeSoundPlayed) {
                chargeSoundPlayed = true;
                uiAudioSource.clip = chargeSound;
                uiAudioSource.Play();
            }
        } else {
            sprite.GetComponent<Animator>().SetBool("squash",false);
            sprite.GetComponent<Animator>().SetFloat("speed",speed);
            pullLine.enabled = false;
        }

        if(rigidBody.velocity.sqrMagnitude > movingThreshold) {
            moving = true;
            launching = false;
            lockRotation = false;
            pullLine.enabled = false;
        } else {
            moving = false;
            launching = false;
            rigidBody.velocity = Vector3.zero;
        }

        readySprite.enabled = !moving;
    }

    private void PointerInput() {

        if(isMoving() || isLaunching())
            return;

        Vector2 position = input_Aim.ReadValue<Vector2>();

        plane = new Plane(Vector3.up, -this.transform.position.y);

        if (input_Launch.inProgress && aiming) {

            float planeEnter;
            Ray ray = Camera.main.ScreenPointToRay(position);
            plane.Raycast(ray, out planeEnter);    // Update the current mouse position while dragging
            aimCurrPos = ray.GetPoint(planeEnter);
        }
    }

    // Used for two-step gamepad controls
    private bool lockRotation;
    private Vector2 rotation;
    private float distance = 1.0f;

    private void GamepadInput() {

        Vector2 aim = input_Aim.ReadValue<Vector2>();

        if (isMoving() || isLaunching() || input_Launch.inProgress)
            return;

        if (input_Aim.inProgress) {

            aimStartPos = this.transform.position;
            aiming = true;

            if (ControlsManager.Instance.getStyle() == ControlStyle.OneStep) {

                rotation = aim.normalized;
                distance = aim.magnitude * magnitudeMax * magnitudeMax;

            } else {

                if (!lockRotation) {

                distance = 2.0f;
                rotation = aim.normalized;

                } else {

                    distance = aim.magnitude * magnitudeMax * magnitudeMax;
                }
            }

            aimCurrPos = transform.position + new Vector3(rotation.x, 0, rotation.y).normalized * distance;

        } else {

            aiming = false;
        }
    }

    private void Launch() {

        bool ip = PauseManager.isPaused();
        bool ic = LevelCompletedManager.isCompleted();
        bool it = TutorialManager.isInTutorial();

        if (ip || ic || it) {
            ResetLaunch();
            return;
        }

        Vector3 launchVector = aimCurrPos - aimStartPos;                                        // Calculate the desired launch vector
        float magnitude = Mathf.Min(launchVector.magnitude * magnitudeScalar, magnitudeMax);    // Scale the magnitude and ensure it is capped at a maximum.
        Vector3 cappedVector = launchVector.normalized * magnitude * -1.0f;                     // Calculate the capped vector with the adjusted magnitude.
        uiAudioSource.clip = launchSound;
        uiAudioSource.Play();

        rigidBody.AddForce(cappedVector, ForceMode.Impulse);
        ResetLaunch();
        launching = true;
        Time.timeScale = 1.0f;
        ballStats.addStroke();
    }

    private void ResetLaunch() {

        aiming = false;                                                                       // Stopped dragging
        aimStartPos = Vector3.zero;                                                            // Reset positions
        aimCurrPos = Vector3.zero;
        rotation = Vector2.zero;
        distance = 1.0f;
        pullLine.enabled = false;
        chargeSoundPlayed = false;
    }

    private Vector3 screenToWorld(Vector3 vec)  {   
        vec.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(vec);
    }

    public bool isMoving() { return moving; }

    public bool isAiming() { return aiming; }

    public bool isLaunching() { return launching; }

    public Vector3 getDirection() {

        if(moving)
            return rigidBody.velocity.normalized;
        else 
            return (aimStartPos - aimCurrPos).normalized;
    }

    public float getMagnitude() {

        Vector3 dir = aimCurrPos - aimStartPos;
        return Mathf.Min(dir.magnitude * magnitudeScalar, magnitudeMax);
    }

    public float getMovingThreshold() { return movingThreshold; }

    IEnumerator CalcSpeed(){
        bool isPlaying = true;

        while(isPlaying)
        {
            Vector3 prevPos = transform.position;

            yield return new WaitForFixedUpdate();

            speed = Mathf.RoundToInt(Vector3.Distance(transform.position, prevPos) / Time.fixedDeltaTime);

        }
    }
}
