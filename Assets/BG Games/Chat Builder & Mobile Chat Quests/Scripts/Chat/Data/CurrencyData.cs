using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.Data
{
   [CreateAssetMenu(fileName = "CurrencyData", menuName = "Chat/CurrencyData", order = 0)]
   public class CurrencyData : ScriptableObject
   {
      [SerializeField] private int _startedBalance;
      [SerializeField] private int _answerCost;
      [SerializeField] private int _imageCost;

      public int StartedBalance => _startedBalance;

      public int AnswerCost => _answerCost;
      public int ImageCost => _imageCost;
   }
}
