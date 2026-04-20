using TMPro;
using UnityEngine;

public class ToShareWithMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _familyObject = null;
    [SerializeField]
    private GameObject _friendsObject = null;
    [SerializeField]
    private GameObject _neighboursObject = null;

    [SerializeField]
    private TextMeshProUGUI _familyNumber = null;
    [SerializeField]
    private TextMeshProUGUI _friendsNumber = null;
    [SerializeField]
    private TextMeshProUGUI _neighboursNumber = null;

    public void SetValues(Quest quest)
    {
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

    private void SetNumber(string groupName, int number)
    {
        if (groupName.Contains("FAMILY"))
        {
            SetValues(_familyObject, _familyNumber, number);
        }
        if (groupName.Contains("FRIENDS"))
        {
            SetValues(_friendsObject, _friendsNumber, number);
        }
        if (groupName.Contains("NEIGHBOURS"))
        {
            SetValues(_neighboursObject, _neighboursNumber, number);
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

    private void SetValues(GameObject holder, TextMeshProUGUI text, int number)
    {
        holder.SetActive(true);
        text.SetText(number.ToString());
    }
}
