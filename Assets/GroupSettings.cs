using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.View;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GroupSettings : MonoBehaviour
{
    [SerializeField] private MessageWritingAnimator _messageWritingAnimator = null;

    [SerializeField] private string _name;
    [SerializeField] private Sprite _image;
    [SerializeField] private List<string> _people;

    [SerializeField] private GameObject _groupInfo;

    public MessageWritingAnimator GetWritingAnimator() { return _messageWritingAnimator; }

    private void OnEnable()
    {
        _groupInfo.GetComponentInChildren<TextMeshProUGUI>().text = _name;
        _groupInfo.GetComponentInChildren<Image>().sprite = _image;
    }

    public string GetRandomName()
    {
        return _people[Random.Range(0, _people.Count)];
    }
}
