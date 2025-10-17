using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.View;
using DA_Assets.Extensions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class GroupSettings : MonoBehaviour
{
    [SerializeField] private MessageWritingAnimator _messageWritingAnimator = null;

    [SerializeField] private string _name;
    [SerializeField] private Sprite _image;
    [SerializeField] private List<string> _people;

    [SerializeField] private GameObject _groupInfo;

    [SerializeField] private int _topOffset = 0;

    public MessageWritingAnimator GetWritingAnimator() { return _messageWritingAnimator; }

    private void OnEnable()
    {
        _groupInfo.GetComponentInChildren<LocalizeStringEvent>().StringReference.SetReference("Translation", _name);
        _groupInfo.GetComponentInChildren<Image>().sprite = _image;

        transform.parent.GetComponent<RectTransform>().SetTop(_topOffset);
    }

    public void ActivateGroupInfo(bool active)
    {
        _groupInfo.SetActive(active);
        _groupInfo.transform.parent.gameObject.SetActive(active);
    }

    public string GetRandomName()
    {
        return _people[Random.Range(0, _people.Count)];
    }
}
