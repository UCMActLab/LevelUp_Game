using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.View;
using System;
using UnityEngine.UI;
using UnityEngine.Events;
using Unity.VisualScripting;

public class TutorialController : MonoBehaviour
{
    [Serializable]
    public struct TutorialStepMessages
    {
        public List<string> messages;
    }

    [SerializeField] private GameObject _articlePrefab;
    [SerializeField] private MessageContainer _messageContainer;
    [SerializeField] private MessageWritingAnimator _writingAnimator;
    [SerializeField] private Transform _chat;

    [SerializeField] private List<TutorialStepMessages> _messageSteps;

    [SerializeField] private ArticleData _data;

    [SerializeField] private GameObject _buttonHolder = null;

    public UnityEvent OnTutorialEnd = new UnityEvent();

    private Button _nextStepButton = null;

    int _currentStep = 0;

    [SerializeField, Range(0.001f, 5.0f)] private float _timeBetweenMessages = 1.0f;

    ArticleDataSetter _article;

    private void Start()
    {
        _data = Instantiate(_data);

        StartCoroutine(ShowMessages());

        _nextStepButton = _buttonHolder.GetComponentInChildren<Button>();
        _nextStepButton.onClick.RemoveAllListeners();
        _nextStepButton.onClick.AddListener(() =>
        {
            DoStep();
            DeactivateButton();
        });

        DeactivateButton();
    }

    IEnumerator ShowMessages()
    {
        List<string> messages = _messageSteps[_currentStep].messages;

        int i = 0;
        while(i < messages.Count)
        {
            _writingAnimator.Enable();
            yield return new WaitForSeconds(_timeBetweenMessages);
            _writingAnimator.Disable();
            _messageContainer.AddMessage(SenderType.Interlocutor, "Tutorial", messages[i], _chat);
            ++i;
        }

        ActivateButton();
    }

    private void ActivateButton()
    {
        _buttonHolder.gameObject.SetActive(true);
    }

    private void DeactivateButton()
    {
        _buttonHolder.gameObject.SetActive(false);
    }

    public void DoStep()
    {
        switch(_currentStep)
        {
            case 0:
                SpawnArticleNoButtons();
                
                break;
            case 1:
                _article = Instantiate(_articlePrefab, _chat.transform).GetComponent<ArticleDataSetter>();
                _article.SetArticleData(_data);
                _article._readButton.onClick.AddListener(() =>
                {
                    UpdateArticleBody();
                });
                break;
        }
        _currentStep++;

        if(_currentStep < _messageSteps.Count)
        {
            StartCoroutine(ShowMessages());
        }
        else
        {
            OnTutorialEnd.Invoke();
        }
    }

    public void SpawnArticleNoButtons()
    {
        _article = Instantiate(_articlePrefab, _chat.transform).GetComponent<ArticleDataSetter>();
        _article.DeactivateButtons();

        _article.SetArticleData(_data);
    }

    public void UpdateArticleBody()
    {
        _data.articleBody = "Aquí es donde podrás leer los detalles de la noticia. Se explicarán cuáles fueron los hechos y se citarán las fuentes de las que vienen (¡si es que existen!).";
        _article.SetArticleData(_data);

        foreach (RectTransform rect in _article.GetComponentsInChildren<RectTransform>())
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
        }
    }
}
