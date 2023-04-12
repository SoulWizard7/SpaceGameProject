using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private Button _startButton;
    private Button _quitButton;

    public EnumScene StartGameScene;

    public GameObject canvas;

    private void Awake()
    {
        _startButton = transform.GetChild(1).GetComponent<Button>();
        _startButton.GetComponentInChildren<TMP_Text>().text = "Start";
        _startButton.onClick.AddListener(StartGame);
        
        _quitButton = transform.GetChild(2).GetComponent<Button>();
        _quitButton.GetComponentInChildren<TMP_Text>().text = "quit";
        _quitButton.onClick.AddListener(QuitGame);
        //_MainMenu = SceneManager.GetActiveScene();
        //SceneManager.LoadSceneAsync("InsideShip", LoadSceneMode.Additive);
        //SceneManager.LoadSceneAsync("SolarSystem", LoadSceneMode.Additive);
    }

    void StartGame()
    {
        SceneLoader.Load(StartGameScene);
        /*
        _insideShip = SceneManager.GetSceneByName("InsideShip");
        canvas.SetActive(false);
        SceneManager.SetActiveScene(_insideShip);*/
    }

    void QuitGame()
    {
        Application.Quit();
    }
}
