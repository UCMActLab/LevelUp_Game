using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Localisation.SO;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Commands
{
    public class ChangeLanguageCommand:ICommand
    {
        private readonly LanguageSwitch _languageSwitch;
        private readonly LanguageType _previousLanguageType;
        private readonly LanguageType _nextLanguageType;

        public ChangeLanguageCommand(LanguageSwitch languageSwitch,LanguageType previousLanguageType,LanguageType nextLanguageType)
        {
            _languageSwitch = languageSwitch;
            _previousLanguageType = previousLanguageType;
            _nextLanguageType = nextLanguageType;
        }
        public void Execute()
        {
            _languageSwitch.ChangeLanguage(_nextLanguageType);
        }

        public void Undo()
        {
            _languageSwitch.ChangeLanguage(_previousLanguageType);
        }
    }
}