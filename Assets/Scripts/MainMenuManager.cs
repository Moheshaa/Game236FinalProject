using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private Button musicBTN;


    [SerializeField]
    private Sprite soundON, soundOFF;
    public void PlayGame()
    {
        GameManager.instance.gameStartedFromMainMenu = true;
        SceneManager.LoadScene(Tags.GAMEPLAY_SCENE);
    }

    public void ControlMusic()
    {
        if (GameManager.instance.canPLayMusic)
        {
            musicBTN.image.sprite = soundON;
            GameManager.instance.canPLayMusic = false;

        }
        else
        {
            musicBTN.image.sprite = soundOFF;
            GameManager.instance.canPLayMusic = true;
        }
    }


}
