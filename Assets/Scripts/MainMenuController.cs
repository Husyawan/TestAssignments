using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public InputField RowsField;
    public InputField ColumnsField; 
    public Button targetButton;

    public GameObject LoadingPanel;

    void Start()
    {
        RowsField.onValueChanged.AddListener(delegate { CheckInputFields(); });
        ColumnsField.onValueChanged.AddListener(delegate { CheckInputFields(); });
        CheckInputFields();
    }

    
    void CheckInputFields()
    { 
        bool isInputFilled = !string.IsNullOrEmpty(RowsField.text) && !string.IsNullOrEmpty(ColumnsField.text);
        targetButton.interactable = isInputFilled;
    }
    public void PlayBtn()
    {
        GameManager.Rows = Convert.ToInt32(RowsField.text);
        GameManager.Columns= Convert.ToInt32(ColumnsField.text);
        LoadingPanel.SetActive(true);
        SceneManager.LoadScene(1);
    }
    public void LoadGame()
    {

        GameManager.LoadGame = true;
        LoadingPanel.SetActive(true);
        SceneManager.LoadScene(1);
    }
}
