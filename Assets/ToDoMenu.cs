using TMPro;
using UnityEngine;

public class ToDoMenu : MonoBehaviour
{
    [Header("TOPICS")]
    [SerializeField]
    private GameObject _generalObjectTopic = null;
    [SerializeField]
    private GameObject _familyObjectTopic = null;
    [SerializeField]
    private GameObject _friendsObjectTopic = null;
    [SerializeField]
    private GameObject _neighboursObjectTopic = null;

    [SerializeField]
    private TextMeshProUGUI _familyTopic = null;
    [SerializeField]
    private TextMeshProUGUI _friendsTopic = null;
    [SerializeField]
    private TextMeshProUGUI _neighboursTopic = null;

    [Header("TO SHARE WITH")]
    [SerializeField]
    private GameObject _generalObjectShareWith = null;
    [SerializeField]
    private GameObject _familyObjectShareWith = null;
    [SerializeField]
    private GameObject _friendsObjectShareWith = null;
    [SerializeField]
    private GameObject _neighboursObjectShareWith = null;

    [SerializeField]
    private TextMeshProUGUI _familyNumber = null;
    [SerializeField]
    private TextMeshProUGUI _friendsNumber = null;
    [SerializeField]
    private TextMeshProUGUI _neighboursNumber = null;

    [Header("GENERAL")]
    [SerializeField]
    private TextMeshProUGUI _minimumArticles = null;
    //[SerializeField]
    //private TextMeshProUGUI _trueNumber = null;
    //[SerializeField]
    //private TextMeshProUGUI _falseNumber = null;

    public void SetValues(Quest quest)
    {
        if (quest.thereAreGroups && !quest.groupsHaveTopics)
        {
            _generalObjectShareWith.SetActive(true);
            _generalObjectTopic.SetActive(false);
            if (quest.toDo.toShareWithFamily > 0)
            {
                SetNumber("FAMILY", quest.toDo.toShareWithFamily);
            }
            else
            {
                HideTopic("FAMILY");
            }
            if (quest.toDo.toShareWithFriends > 0)
            {
                SetNumber("FRIENDS", quest.toDo.toShareWithFriends);
            }
            else
            {
                HideTopic("FRIENDS");
            }
            if (quest.toDo.toShareWithNeighbours > 0)
            {
                SetNumber("NEIGHBOURS", quest.toDo.toShareWithNeighbours);
            }
            else
            {
                HideTopic("NEIGHBOURS");
            }
        }
        else if (quest.thereAreGroups)
        {
            _generalObjectShareWith.SetActive(false);
            _generalObjectTopic.SetActive(true);
        }
        else
        {
            _generalObjectShareWith.SetActive(false);
            _generalObjectTopic.SetActive(false);
        }
        
        _minimumArticles.SetText(string.Format(TranslationManager.Instance.GetLocalizedStringValue("Translation", "TODO/MINIMUM_ARTICLES"), ScoreManager.Instance.MinimumScoreToCompleteQuest(quest)));
        // _falseNumber.SetText(quest.toDo.falseArticlesToSkip.ToString());
    }

    private void SetNumber(string groupName, int number)
    {
        if (groupName.Contains("FAMILY"))
        {
            SetValues(_familyObjectShareWith, _familyNumber, number);
        }
        if (groupName.Contains("FRIENDS"))
        {
            SetValues(_friendsObjectShareWith, _friendsNumber, number);
        }
        if (groupName.Contains("NEIGHBOURS"))
        {
            SetValues(_neighboursObjectShareWith, _neighboursNumber, number);
        }
    }

    public void HideTopic(string groupName)
    {
        if (groupName.Contains("FAMILY"))
        {
            _familyObjectTopic.SetActive(false);
        }
        if (groupName.Contains("FRIENDS"))
        {
            _friendsObjectTopic.SetActive(false);
        }
        if (groupName.Contains("NEIGHBOURS"))
        {
            _neighboursObjectTopic.SetActive(false);
        }
    }

    private void SetValues(GameObject holder, TextMeshProUGUI text, int number)
    {
        holder.SetActive(true);
        text.SetText(number.ToString());
    }

    public void SetTopic(string groupName, Topics topic)
    {
        if (groupName.Contains("FAMILY"))
        {
            SetTopicValue(_familyObjectTopic, _familyTopic, topic);
        }
        if (groupName.Contains("FRIENDS"))
        {
            SetTopicValue(_friendsObjectTopic, _friendsTopic, topic);
        }
        if (groupName.Contains("NEIGHBOURS"))
        {
            SetTopicValue(_neighboursObjectTopic, _neighboursTopic, topic);
        }
    }

    private void SetTopicValue(GameObject holder, TextMeshProUGUI text, Topics topic)
    {
        holder.SetActive(true);
        text.SetText(
            TranslationManager.Instance.
            GetLocalizedStringValue("Translation", "TOPIC/" + topic.ToString()));
    }
}