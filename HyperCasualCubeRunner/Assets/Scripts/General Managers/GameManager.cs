using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //  Singleton manager:
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
            DestroyImmediate(this.gameObject);
        else
            _instance = this;

        //  Dont destroy across scenes:
        DontDestroyOnLoad(gameObject);

        MoreAwake();
    }

    //  Continue normal class code.

    [SerializeField] private SoundSettings soundSettings = new SoundSettings();

    //  Scene transition handling:
    SceneTransitionManager sceneTransitionManager = null;

    private void MoreAwake()
    {
        sceneTransitionManager = GetComponent<SceneTransitionManager>();
    }

    //  Restart current scene:
    public void RestartCurrentScene()
    {
        sceneTransitionManager.LoadAScene(SceneManager.GetActiveScene().name);
    }

    //  Main menu:
    public void ToMainMenu()
    {
        sceneTransitionManager.LoadAScene("SceneMainMenu");
    }

    //  In game scene:
    public void ToInGameScene()
    {
        sceneTransitionManager.LoadAScene("SceneInGame");
    }

    //  Get sound settings:
    public SoundSettings GetSoundSettings() { return soundSettings; }
}
