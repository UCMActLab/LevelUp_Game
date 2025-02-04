using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.ImageServices;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Commands
{
    public class ImageRemoveCommand:ICommand
    {
        private readonly MessageImage _messageImage;
        private readonly CashedImage _oldImage;

        public ImageRemoveCommand(MessageImage messageImage)
        {
            _messageImage = messageImage;
            _oldImage = messageImage.CashedImage;
        }

        public void Execute()
        {
            _messageImage.RemoveImage();
        }

        public void Undo()
        {
            _messageImage.SetCashedImage(_oldImage);
        }
    }
}