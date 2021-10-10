using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    GameManager gameManager = null;

    AudioManager audioManager = null;

    private void Start()
    {
        gameManager = GameManager.Instance;
        if (gameManager == null)
            Debug.LogWarning(gameObject.name + ": GameManager Instance not found.");

        audioManager = GameObject.FindObjectOfType<AudioManager>();
        if (audioManager)
            audioManager.PlaySound("MenuMusic", 3.0f, true);
        else
            Debug.LogWarning(gameObject.name + ": Failed to play music, AudioManager not found.");
    }


    public void ToInGameScene()
    {
        if (gameManager)
            gameManager.ToInGameScene();
        else
            Debug.LogWarning(gameObject.name + ": GameManager is NULL.");
    }

    //  Stop music, called when play button is clicked:
    public void StopMenuMusic()
    {
        if (audioManager)
            audioManager.StopSound("MenuMusic", 1.0f);
    }

}
