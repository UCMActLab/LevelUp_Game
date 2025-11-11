using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "VerificationFeedback", menuName = "ScriptableObjects/VerificationFeedback")]
public class VerificationFeedback : ScriptableObject
{
    public bool IsTrue;
    public LocalizedString[] Feedback;
}
