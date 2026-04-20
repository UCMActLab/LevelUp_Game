using TMPro;
using UnityEngine;

public class TopicMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _familyObject = null;
    [SerializeField]
    private GameObject _friendsObject = null;
    [SerializeField]
    private GameObject _neighboursObject = null;

    [SerializeField]
    private TextMeshProUGUI _familyTopic = null;
    [SerializeField]
    private TextMeshProUGUI _friendsTopic = null;
    [SerializeField]
    private TextMeshProUGUI _neighboursTopic = null;

    public void SetTopic(string groupName, Topics topic)
    {
        if (groupName.Contains("FAMILY"))
        {
            SetValues(_familyObject, _familyTopic, topic);
        }
        if (groupName.Contains("FRIENDS"))
        {
            SetValues(_friendsObject, _friendsTopic, topic);
        }
        if (groupName.Contains("NEIGHBOURS"))
        {
            SetValues(_neighboursObject, _neighboursTopic, topic);
        }
    }

    public void HideTopic(string groupName)
    {
        if (groupName.Contains("FAMILY"))
        {
            _familyObject.SetActive(false);
        }
        if (groupName.Contains("FRIENDS"))
        {
            _friendsObject.SetActive(false);
        }
        if (groupName.Contains("NEIGHBOURS"))
        {
            _neighboursObject.SetActive(false);
        }
    }

    private void SetValues(GameObject holder, TextMeshProUGUI text, Topics topic) 
    {
        holder.SetActive(true);
        text.SetText(
            TranslationManager.Instance.
            GetLocalizedStringValue("Translation", "TOPIC/" + topic.ToString()));
    }
}
