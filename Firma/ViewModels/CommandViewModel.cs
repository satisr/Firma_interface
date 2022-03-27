using System;
using System.Windows.Input;

namespace Firma.ViewModels
{
    //To jest klasa, która jest po to żeby tworzyć komendy,
    //które pojawiają się w menu z lewej strony 
    public class CommandViewModel : BaseViewModel
    {
        #region Właściwości
        public string DisplayName { get; set; } //To jest nazwa przycisku w menu z lewej strony
        public ICommand Command { get; set; } //Każdy przycisk zawiera komendę, która wywołuje funkcję, która otwiera zakłądkę
        #endregion

        #region Konstruktor
        public CommandViewModel(string displayName, ICommand command)
        {
            if (command is null)
                throw new ArgumentNullException("Command");

            this.DisplayName = displayName;
            this.Command = command;
        }
        #endregion
    }
}
