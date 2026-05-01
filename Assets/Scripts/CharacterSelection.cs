using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CharacterSelection : MonoBehaviour
{
    [Header("Character Setup")]
    public GameObject[] characters; 
    public TextMeshProUGUI characterNameText;

    [Header("UI Display")]
    public Image characterDisplay;

    private int currentIndex = 0;

    private void Start()
    {
        currentIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        UpdateCharacterDisplay();
    }

    public void NextCharacter()
    {
        currentIndex = (currentIndex + 1) % characters.Length;
        UpdateCharacterDisplay();
    }

    public void PrevCharacter()
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = characters.Length - 1;
        }
        UpdateCharacterDisplay();
    }

    private void UpdateCharacterDisplay()
    {
        GameObject selectedChar = characters[currentIndex];

        if (characterNameText != null)
        {
            characterNameText.text = selectedChar.name;
        }

        SpriteRenderer sr = selectedChar.GetComponent<SpriteRenderer>();

        if (sr != null && characterDisplay != null)
        {
            characterDisplay.sprite = sr.sprite;
        }
    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("SelectedCharacter", currentIndex);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
}