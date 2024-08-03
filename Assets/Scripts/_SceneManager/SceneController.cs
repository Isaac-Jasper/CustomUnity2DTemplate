using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

//contains a basic black screen fade out and fade in
public class SceneController : MonoBehaviour
{
    public static SceneController i { get; private set; }

    [SerializeField]
    private float transitionSpeed;
    [SerializeField]
    private float sceneStartTransitionDelay;

    [SerializeField]
    private CanvasGroup fadeGroup;
    [SerializeField]
    private GameObject fadeObject;

    private void Awake() {
        if (i != null && i != this && i.enabled) {
            Destroy(gameObject);
        } else {
            i = this;
        }
        DontDestroyOnLoad(this.gameObject);

        InputManager.Instance.OnRestartPerformed += restartScene;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        InputManager.Instance.LockedInput = true;
        //Dont Destroy On Load Objects are stored in scene 0, if we are in scene 0 immediatly go to the first scene of the game
        //Scene 0 could also have intro things like a splash screen or cutscene.
        if (SceneManager.GetActiveScene().buildIndex == 0) {
            SceneManager.LoadScene(1);
            return;
        }

        initializeObjects();
        StartCoroutine(startScene());
    }

    private void initializeObjects() {
        //initialize any on scene start objects here
    }

    public IEnumerator changeScene(int scene) {
        InputManager.Instance.LockedInput = true;
        fadeGroup.alpha = 0;
        while (fadeGroup.alpha < 0.98) {
            fadeGroup.alpha = Mathf.Lerp(1, fadeGroup.alpha, transitionSpeed/100);
            yield return null;
        }
        fadeGroup.alpha = 1;
        SceneManager.LoadScene(scene);
    }

    private void restartScene(float time) {
        StartCoroutine(changeScene(SceneManager.GetActiveScene().buildIndex));
    }

    private IEnumerator startScene() {
        InputManager.Instance.LockedInput = true;
        yield return new WaitForSeconds(sceneStartTransitionDelay);

        fadeGroup.alpha = 1;
        while (fadeGroup.alpha > 0.02) {
            fadeGroup.alpha = Mathf.Lerp(0, fadeGroup.alpha, transitionSpeed/100);
            yield return null;
        }
        fadeGroup.alpha = 0;
        InputManager.Instance.LockedInput = false;
    }
}
