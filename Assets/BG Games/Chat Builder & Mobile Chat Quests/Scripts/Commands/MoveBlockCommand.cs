using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Commands
{
    public class MoveBlockCommand:ICommand
    {
        private readonly Transform _blockTransform;
        private readonly Vector3 _positionFrom;
        private readonly Vector3 _positionTo;

        public  MoveBlockCommand(Transform blockTransform, Vector3 positionFrom, Vector3 positionTo)
        {
            _blockTransform = blockTransform;
            _positionFrom = positionFrom;
            _positionTo = positionTo;
        }
        
        public void Execute()
        {
            _blockTransform.position = _positionTo;
        }

        public void Undo()
        {
            _blockTransform.position = _positionFrom;
        }
    }
}