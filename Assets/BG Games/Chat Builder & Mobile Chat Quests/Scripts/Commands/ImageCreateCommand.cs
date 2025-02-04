using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.ImageServices;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Commands
{
    public class ImageCreateCommand : ICommand
    {
        private readonly MessageImage _messageImage;
        private readonly CashedImage _oldImage;
        private readonly CashedImage _newImage;

        public ImageCreateCommand(MessageImage messageImage, CashedImage newImage)
        {
            _messageImage = messageImage;
            _oldImage = messageImage.CashedImage;
            _newImage = newImage;
        }

        public void Execute()
        {
            _messageImage.SetCashedImage(_newImage);
        }

        public void Undo()
        {
            _messageImage.SetCashedImage(_oldImage);
        }
    }
}