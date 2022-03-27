using Firma.Helpers;
using System;
using System.Windows.Input;

namespace Firma.ViewModels
{
    //To jest klasa, z któej będą dziedziczyc wszystkie ViewModel'e zakładek
    public class WorkspaceViewModel : BaseViewModel
    {
        #region Pola i Komendy
        //Każda zakładka ma minimum "nazwę" i "zamknij"
        // W tym propertisie będzie przechowywana nazwa zakładki
        public string DisplayName { get; set; }

        //To jest komenda do zamykania okna:
        private BaseCommand _CloseCommand;

        public ICommand CloseCommand
        {
            get
            {
                if (_CloseCommand == null)
                    _CloseCommand = new BaseCommand(() => this.OnRequestClose());

                return _CloseCommand;
            }
        }
        #endregion

        #region RequestClose [event] 
        public event EventHandler RequestClose; 
        private void OnRequestClose() 
        { 
            EventHandler handler = this.RequestClose; 
            if (handler != null) 
                handler(this, EventArgs.Empty); 
        } 
        #endregion
    }
}
