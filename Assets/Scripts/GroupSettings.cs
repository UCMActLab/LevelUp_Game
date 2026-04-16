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

    [SerializeField] private GameObject _groupInfo;

    [SerializeField] private int _topOffset = 0;

    public string Topic { get; private set; }

    // Método público que escoge aleatoriamente de una lista proporcionada y consume la opción
    public void AssignRandomTopic(List<string> availableTopicsPool)
    {
        if (availableTopicsPool == null || availableTopicsPool.Count == 0)
        {
            Debug.LogWarning($"[{gameObject.name}] No hay temáticas disponibles en el pool.");
            return;
        }

        // Se escoge un índice aleatorio de las opciones restantes
        int randomIndex = Random.Range(0, availableTopicsPool.Count);
        Topic = availableTopicsPool[randomIndex];

        // Se elimina para que el siguiente grupo no pueda escoger la misma
        availableTopicsPool.RemoveAt(randomIndex);
    }

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
        return null;
    }
}
