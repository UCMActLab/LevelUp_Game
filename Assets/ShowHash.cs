using System;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ShowHash : MonoBehaviour
{
    TextMeshProUGUI _text = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _ShowHash();
    }

    private void _ShowHash()
    {
        SHA1 sha = SHA1.Create();

        string hash = "#" + HashString(SystemInfo.deviceUniqueIdentifier + DateTime.Now.ToString(), 5);

        AnalyticsManager.Instance.SetHash(hash);

        _text.SetText(hash);
    }

    public static string HashString(string text, int length)
    {
        const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        byte[] bytes = Encoding.UTF8.GetBytes(text);

        SHA256Managed hashstring = new SHA256Managed();
        byte[] hash = hashstring.ComputeHash(bytes);

        char[] hash2 = new char[length];

        // Note that here we are wasting bits of hash! 
        // But it isn't really important, because hash.Length == 32
        for (int i = 0; i < hash2.Length; i++)
        {
            hash2[i] = chars[hash[i/*+ Random.Range(0, hash2.Length - i)*/] % chars.Length];
        }

        return new string(hash2);
    }
}
