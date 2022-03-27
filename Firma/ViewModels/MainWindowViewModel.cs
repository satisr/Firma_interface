using Firma.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace Firma.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        //Będzie zawierała kolekcję komend,
        //które pojawiąją się w menu lewym oraz kolekcje zakładek
        #region Komendy menu i paska narzędzi
        public ICommand NowyTowarCommand
        {
            get
            {
                return new BaseCommand(createTowar);
            }
        }
        public ICommand TowaryCommand
        {
            get
            {
                return new BaseCommand(showAllTowar);
            }
        }
        #endregion
        #region Przyciski z lewego menu

        private ReadOnlyCollection<CommandViewModel> _Commands; //To jest kolekcja komend w lewym menu
        public ReadOnlyCollection<CommandViewModel> Commands
        {
            get
            {
                if (_Commands is null) // Sprawdzam czy przyciski z lewej strony (menu) nie zostały zainicjalizowane
                {
                    List<CommandViewModel> cmds = this.CreateCommands(); // Tworzę listę przycisków za pomocą funkcji CreateCommand()
                    _Commands = new ReadOnlyCollection<CommandViewModel>(cmds); // Tę listę przypisuję do ReadOnlyCollection (bo w niej można tylko tworzyć, nie można dodawać)
                }
                return _Commands;
            }
        }

        private List<CommandViewModel> CreateCommands() // Tu decydujemy jakie przyciski są w lewym menu
        {
            return new List<CommandViewModel>()
            {
                new CommandViewModel("JW Towary", new BaseCommand(showAllTowar)), //To tworzy pierwszy przycisk o nazwie Towary, który pokaże zakłądkę "WszystkieTowary"
                new CommandViewModel("Nowy towar", new BaseCommand(createTowar))
            };
        }

        #endregion

        #region Zakładki
        private ObservableCollection<WorkspaceViewModel> _Workspaces; //To jest kolekcja zakładek

        public ObservableCollection<WorkspaceViewModel> Workspaces
        {
            get
            {
                if (_Workspaces is null)
                    _Workspaces = new ObservableCollection<WorkspaceViewModel>();

                _Workspaces.CollectionChanged += this.OnWorkspacesChanged;

                return _Workspaces;
            }
        }

        private void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.NewItems)
                    workspace.RequestClose += this.OnWorkspaceRequestClose;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.OldItems)
                    workspace.RequestClose -= this.OnWorkspaceRequestClose;
        }
        private void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            WorkspaceViewModel workspace = sender as WorkspaceViewModel; //workspace.Dispos(); this.Workspaces.Remove(workspace); }

            this.Workspaces.Remove(workspace);
        }

        #endregion

        #region Konstruktor
        public MainWindowViewModel()
        {

        }
        #endregion

        //To jest funkcja, która otwiera nową zakładkę "Towar"
        //za każdym razem tworzy NOWĄ zakładkę do dodawania towaru
        #region Funkcje pomocnicze
        private void createTowar()
        {
            //Tworzymy zakładkę NowyTowar (VM)
            var workspace = new NowyTowarViewModel();
            //dodajemy ją do kolekcji aktywnych zakładek
            this.Workspaces.Add(workspace);
            this.SetActiveWorkspace(workspace);
        }

        //To jest funkcja, która otwiera zakłądkę ze wszystkimi towarami
        //za każdym razem sprawdza czy zakładka z towarami jest już otwarta, jezeli tak, wtedy ją aktywuje
        //... jeżeli nie ma, wtedy ją tworzy.
        private void showAllTowar()
        {
            //Szukamy w kolekcji zakładek, takiej zakłądki, która jest wszystkimi towarami
            //jeżeli takiej zakłądki nie ma
            WszystkieTowaryViewModel workspace = this.Workspaces.FirstOrDefault(vm => vm is WszystkieTowaryViewModel) as WszystkieTowaryViewModel;

            //Tworzymy nową zakładkę Wszystkie Towary...
            if (workspace is null)
            {
                workspace = new WszystkieTowaryViewModel();
                //... I didajemy ją do kolekcji zakładek
                this.Workspaces.Add(workspace);
            }

            // Aktywujemy zakłądkę
            this.SetActiveWorkspace(workspace);
        }

        private void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            Debug.Assert(this.Workspaces.Contains(workspace));

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
            if (collectionView != null)
                collectionView.MoveCurrentTo(workspace);
        }
        #endregion
    }
}