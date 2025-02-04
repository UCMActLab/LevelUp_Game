using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.Data;
using TMPro;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.System
{
   public class CurrencyService : MonoBehaviour
   {
      [SerializeField] private CurrencyData _currencyData;
      [SerializeField] private TMP_Text textCurrency;

      private int _currentCurrency;
   
      private void Start()
      {
         SetupCurrency();
      }

      public int GetAnswerCost() => _currencyData.AnswerCost;

      public int GetImageCost() => _currencyData.ImageCost;
 
   
      public bool Pay(int value)
      {
         if (_currentCurrency >= value)
         {
            _currentCurrency -= value;
            textCurrency.text = _currentCurrency.ToString();
            return true;
         }
         return false;
      }

      private void SetupCurrency()
      {
         _currentCurrency = _currencyData.StartedBalance;
         textCurrency.text = _currentCurrency.ToString();
      }
   }
}
