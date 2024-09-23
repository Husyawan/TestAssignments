using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class GamePlayController : MonoBehaviour
{
    [Header("Game_Settings")]
    public GameObject[] uniqueCardPrefabs;
    private int rows;
    private int columns;
    public Transform gridParent; 
    public GridLayoutGroup gridLayoutGroup;

    [Header("UI_Elements")]
    public Text scoreText; 

    private List<GameObject> spawnedCards = new List<GameObject>();
    private List<GameObject> flippedCards = new List<GameObject>();
    private int score = 0;

    public GameObject gameCompletedPanel;
    public GameObject Loading;

    private void Start()
    {
        rows = GameManager.Rows;
        columns = GameManager.Columns;
        AdjustGridLayout();
        CreateGrid();
        UpdateScoreText();
       
        Invoke(nameof(waitSomeSecondsthendisable), 1f);
    }
    public void waitSomeSecondsthendisable()
    {

        foreach (var item in spawnedCards)
        {
            item.gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
        }
        Debug.Log("SSS");
    }
    // Adjusts the grid layout dynamically based on rows and columns
    void AdjustGridLayout()
    {
        RectTransform gridRectTransform = gridParent.GetComponent<RectTransform>();
        float cellWidth = (gridRectTransform.rect.width - gridLayoutGroup.padding.left - gridLayoutGroup.padding.right) / columns;
        float cellHeight = (gridRectTransform.rect.height - gridLayoutGroup.padding.top - gridLayoutGroup.padding.bottom) / rows;
        gridLayoutGroup.cellSize = new Vector2(cellWidth, cellHeight);
        gridLayoutGroup.spacing = new Vector2(5, 5);
        gridLayoutGroup.padding = new RectOffset(5, 5, 5, 5);
    }

    // Creates the grid with pairs of cards
    void CreateGrid()
    {
        int totalPairs = (rows * columns) / 2;
        List<GameObject> cardPairs = new List<GameObject>();

      
        for (int i = 0; i < totalPairs; i++)
        {
            GameObject cardPrefab = uniqueCardPrefabs[i % uniqueCardPrefabs.Length];
            cardPairs.Add(cardPrefab);
            cardPairs.Add(cardPrefab);
        }

      
        Shuffle(cardPairs);

       
        foreach (GameObject cardPrefab in cardPairs)
        {
            GameObject cardInstance = Instantiate(cardPrefab, gridParent);
            cardInstance.GetComponent<Button>().onClick.AddListener(() => OnCardClicked(cardInstance));
            spawnedCards.Add(cardInstance);
        }
    }

    // Called when a card is clicked
    void OnCardClicked(GameObject clickedCard)
    {
        AudioManager.instance.SendMessage("play", "Flipping");
        if (flippedCards.Contains(clickedCard) || clickedCard.GetComponent<Card>().isMatched)
        {
            return;
        }

        StartCoroutine(FlipCardAnimation(clickedCard));
    }

    IEnumerator FlipCardAnimation(GameObject card)
    {
      
        card.GetComponent<Card>().Flip(true);
        flippedCards.Add(card);

      
        if (flippedCards.Count == 2)
        {
            yield return new WaitForSeconds(0.5f); 
            CheckMatch();
        }
    }

    // Check if the two flipped cards are a match
    void CheckMatch()
    {
        if (flippedCards.Count < 2) return;

        GameObject firstCard = flippedCards[0];
        GameObject secondCard = flippedCards[1];

      
        if (firstCard.GetComponent<Card>().cardId == secondCard.GetComponent<Card>().cardId)
        {
            firstCard.GetComponent<Card>().SetMatched(true);
            secondCard.GetComponent<Card>().SetMatched(true);
            score += 10;
            AudioManager.instance.SendMessage("play", "MatchedSFx");
        }
        else
        {
            AudioManager.instance.SendMessage("play", "UnMatchSFX");
            StartCoroutine(FlipBackCards(firstCard, secondCard));
        }

        flippedCards.Clear();
       
        UpdateScoreText();

        CheckGameCompleted();
    }
    void CheckGameCompleted()
    {
       
        bool allMatched = spawnedCards.All(card => card.GetComponent<Card>().isMatched);
        if (allMatched)
        {
            AudioManager.instance.SendMessage("play", "Game_Over");
            gameCompletedPanel.SetActive(true); 
        }
    }
    // Flip the cards back if they don't match
    IEnumerator FlipBackCards(GameObject firstCard, GameObject secondCard)
    {
        yield return new WaitForSeconds(0.5f);
        firstCard.GetComponent<Card>().Flip(false);
        secondCard.GetComponent<Card>().Flip(false);
    }

    // Update the score display
    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    // Save the game state
    public void SaveGame()
    {
        PlayerPrefs.SetInt("Score", score);

      
        List<int> matchedIndices = new List<int>();
        List<int> flippedIndices = new List<int>();

        for (int i = 0; i < spawnedCards.Count; i++)
        {
            Card card = spawnedCards[i].GetComponent<Card>();
            if (card.isMatched)
                matchedIndices.Add(i);
            else if (flippedCards.Contains(spawnedCards[i]))
                flippedIndices.Add(i);
        }

        PlayerPrefs.SetString("MatchedCards", string.Join(",", matchedIndices));
        PlayerPrefs.SetString("FlippedCards", string.Join(",", flippedIndices));
        PlayerPrefs.Save();
    }

    // Load the game state
    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("Score"))
        {
            score = PlayerPrefs.GetInt("Score");

            string[] matchedIndices = PlayerPrefs.GetString("MatchedCards").Split(',');
            string[] flippedIndices = PlayerPrefs.GetString("FlippedCards").Split(',');

            foreach (string indexStr in matchedIndices)
            {
                if (int.TryParse(indexStr, out int index))
                {
                    spawnedCards[index].GetComponent<Card>().SetMatched(true);
                }
            }

            foreach (string indexStr in flippedIndices)
            {
                if (int.TryParse(indexStr, out int index))
                {
                    spawnedCards[index].GetComponent<Card>().Flip(true);
                    flippedCards.Add(spawnedCards[index]);
                }
            }
        }
        UpdateScoreText();
        Debug.Log("Game Loaded!");
    }

    // Shuffle method to randomize the cards
    void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
    public void HideAfterSomeSeconds()
    {
        foreach (var item in spawnedCards)
        {
            item.transform.GetChild(0).GetComponent<Image>().enabled = true;
        }
    }
    public void GoToHome()
    {
        Loading.SetActive(true);
        SceneManager.LoadScene(0);
    }
}
