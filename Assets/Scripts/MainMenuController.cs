using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public InputField RowsField; // Assign from the Inspector
    public InputField ColumnsField; // Assign from the Inspector
    public Button targetButton; // Assign from the Inspector

    public GameObject LoadingPanel;

    void Start()
    {
        // Add listeners to call the function when the input field values change
        RowsField.onValueChanged.AddListener(delegate { CheckInputFields(); });
        ColumnsField.onValueChanged.AddListener(delegate { CheckInputFields(); });

        // Initially check input fields in case they already have values
        CheckInputFields();
    }

    // This method checks if both input fields are filled
    void CheckInputFields()
    {
        // Check if both input fields are not empty
        bool isInputFilled = !string.IsNullOrEmpty(RowsField.text) && !string.IsNullOrEmpty(ColumnsField.text);

        // Enable or disable the button based on the input fields
        targetButton.interactable = isInputFilled;
    }
    public void PlayBtn()
    {
        GameManager.Rows = Convert.ToInt32(RowsField.text);
        GameManager.Columns= Convert.ToInt32(ColumnsField.text);
        LoadingPanel.SetActive(true);
        SceneManager.LoadScene(1);
    }
}
