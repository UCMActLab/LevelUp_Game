using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

[Serializable]
[CreateAssetMenu]
public class DialogSettings : ScriptableObject
{
    public List<LocalizedString> texts;

    public float speed;

    [Header("Events")]
    public UnityEvent onStartDialog;
    public UnityEvent onFinishDialog;
}