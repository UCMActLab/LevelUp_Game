using Ink.Parsed;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WordGameLogic : MonoBehaviour
{
    [SerializeField]
    private WordMiniGame wordMiniGame;

    [SerializeField]
    private TMP_Text hint;

    [SerializeField]
    private GameObject optionPrefab;

    [SerializeField]
    private GameObject solutionPiecePrefab;

    [SerializeField]
    private GameObject solutionContainer;

    [SerializeField]
    private GameObject optionsRow0;

    [SerializeField]
    private GameObject optionsRow1;

    [SerializeField]
    private string defaultSolutionText = "_";

    [SerializeField]
    private string optionsSeparator = "/";

    [SerializeField]
    private int maxAttempts = 3;

    [SerializeField]
    private TMP_Text attemptsText;

    private int currentAttempts = 0;

    private WordGameOptionLogic[] refOptionsInSolution;

    private WordGameSolutionPieceLogic[] solutionPieces;

    private int currentSolutionIndex = 0;

    private string submittedSolution = "";

    [Header("Localization")]
    [SerializeField]
    private TMP_Text hintHeader;

    [SerializeField]
    private TMP_Text submitButtonText;

    [SerializeField]
	private TMP_Text clearButtonText;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        SetUp();
    }

    public void SetUp()
    {
        submittedSolution = TranslationManager.Instance.GetLocalizedStringValue("WORD_MINIGAME", wordMiniGame.targetWordTableKey).ToLowerInvariant();

		hint.text = TranslationManager.Instance.GetLocalizedStringValue("WORD_MINIGAME", wordMiniGame.hintTableKey);

        int wordLength = submittedSolution.Length;

		solutionPieces = new WordGameSolutionPieceLogic[wordLength];

		for (int i = 0; i < wordLength; i++)
        {
			GameObject letterSlot = Instantiate(solutionPiecePrefab, solutionContainer.transform);

			solutionPieces[i] = letterSlot.GetComponentInChildren<WordGameSolutionPieceLogic>();
            solutionPieces[i].SetUp(defaultSolutionText, i, this);
		}

        refOptionsInSolution = new WordGameOptionLogic[wordLength];

        string translatedOptions = TranslationManager.Instance.GetLocalizedStringValue("WORD_MINIGAME", wordMiniGame.optionsTableKey);

        string[] options = translatedOptions.Split(optionsSeparator.ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);

        int numOptions = options.Length;

        for (int i = 0; i < numOptions; i++)
        {
            GameObject parent = optionsRow0;
            if(i+1 > numOptions / 2)
            {
                parent = optionsRow1;
            }
			GameObject option = Instantiate(optionPrefab, parent.transform);

			option.GetComponentInChildren<WordGameOptionLogic>().SetUp(options[i], this);
		}

		attemptsText.text = TranslationManager.Instance.GetLocalizedStringValue("WORD_MINIGAME", "ATTEMPTS") + currentAttempts + " / " + maxAttempts;

        hintHeader.text = TranslationManager.Instance.GetLocalizedStringValue("WORD_MINIGAME", "HINT");
		submitButtonText.text = TranslationManager.Instance.GetLocalizedStringValue("WORD_MINIGAME", "SUBMIT");
        clearButtonText.text = TranslationManager.Instance.GetLocalizedStringValue("WORD_MINIGAME", "CLEAR");
	}

	public bool OnOptionSelected(string selectedLetter, WordGameOptionLogic wgOptionLogic)
    {
        if (currentSolutionIndex >= submittedSolution.Length)
        {
            Debug.LogWarning("All solution slots are already filled.");
            return false;
        }
		refOptionsInSolution[currentSolutionIndex] = wgOptionLogic;

		solutionPieces[currentSolutionIndex].updateValue(selectedLetter);

        currentSolutionIndex++;

		return true;
	}

    public bool RemoveOptionFromSolution(int index)
    {
        if(index < 0 || index >= submittedSolution.Length)
        {
			Debug.LogWarning("Index outside of boundaries.");
			return false;
		}

        if(index >= currentSolutionIndex)
        {
            Debug.LogWarning("Cannot remove option from an empty solution slot.");
			return false;
        }
        
        refOptionsInSolution[index].Reactivate();

        int limit = currentSolutionIndex - 1;

		for (int i = index; i < limit; i++)
        {
            solutionPieces[i].updateValue(solutionPieces[i + 1].GetValue());
            refOptionsInSolution[i] = refOptionsInSolution[i + 1];
        }

        solutionPieces[limit].updateValue(defaultSolutionText);
        refOptionsInSolution[limit] = null;

        currentSolutionIndex--;

        return true;
    }

    public void ClearSolution()
    {
		for (int i = 0; i < currentSolutionIndex; i++)
        {
            solutionPieces[i].updateValue(defaultSolutionText);
            refOptionsInSolution[i].Reactivate();
            refOptionsInSolution[i] = null;
		}

		currentSolutionIndex = 0;
	}

    public void SubmitSoultion()
    {
        // TODO: must it be filled completely to submit?
        if(currentSolutionIndex < submittedSolution.Length)
        {
            Debug.LogWarning("Solution is not complete.");
            return;
        }

        if(currentAttempts >= maxAttempts)
        {
            Debug.LogWarning("Maximum number of attempts reached. Cannot submit solution.");
            return;
        }

        string result = "";

        for(int i = 0; i < submittedSolution.Length; i++)
        {
			result += solutionPieces[i].GetValue();
		}

        result = result.ToLower();

        Debug.Log("Submitted Solution: " + result);

        if(result == submittedSolution)
        {
            Debug.Log("Correct Solution!");
        }
        else
        {
            currentAttempts++;
            attemptsText.text = TranslationManager.Instance.GetLocalizedStringValue("WORD_MINIGAME", "ATTEMPTS") + currentAttempts + " / " + maxAttempts;

            if(currentAttempts >= maxAttempts)
            {
				Debug.Log("Maximum number of attempts reached. The correct word was: " + submittedSolution);
			}
			else
            {
				Debug.Log("Incorrect Solution. Try again.");
			}
        }
    }
}
