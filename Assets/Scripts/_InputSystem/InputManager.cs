using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour {
    private static InputManager instance;
    public static InputManager Instance {
        get { return instance; }
    }

    public delegate void RestartPerformed(float time);
    public event RestartPerformed OnRestartPerformed;
    public delegate void PausePerformed(float time);
    public event PausePerformed OnPausePerformed;

    private PlayerActions inputs;

    public bool LockedInput { get; set; }

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        } else {
            instance = this;
        }
        inputs = new PlayerActions();

        inputs.Gameplay.Pause.performed += Gameplay_Pause_Performed;
        inputs.Gameplay.Restart.performed += Gameplay_Restart_Performed;
    }
    /// <param name="name"></param>
    /// <returns>True if the input can be done at that location and time</returns>
    public bool CheckIfCanInput() {
        if (Time.timeScale == 0) return false;
        if (LockedInput) return false;

        return true;
    }
    private void Gameplay_Pause_Performed(InputAction.CallbackContext ctx) {
        Debug.Log("Gameplay Pause Performed");
        if (!CheckIfCanInput()) return;

        GameSoundController.i.PlayMusic(GameSoundController.Sound.Music_TestMusic1);

        OnPausePerformed?.Invoke((float)ctx.startTime);
    }
    private void Gameplay_Restart_Performed(InputAction.CallbackContext ctx) {
        Debug.Log("Gameplay Restart Performed");
        if (!CheckIfCanInput()) return;
        GameSoundController.i.PlayMusic(GameSoundController.Sound.Music_TestMusic2);
        OnRestartPerformed?.Invoke((float)ctx.startTime);
    }

    private void OnEnable() {
        inputs.Gameplay.Enable();
    }

    private void OnDisable() {
        inputs.Gameplay.Disable();
    }
}