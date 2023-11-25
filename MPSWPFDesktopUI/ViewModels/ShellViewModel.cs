using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Caliburn.Micro;
using MPSWPFDesktopUI.EventsModel;
using MPSWPFDesktopUI.Library.Models;


namespace MPSWPFDesktopUI.ViewModels
{
    public class ShellViewModel:Conductor<object>,IHandle<LogOnEvent>
    {
        private IEventAggregator _events;
        private SalesViewModel _salesVm;
        private ILoggedInUserModel _loggedInUserModel;

        public ShellViewModel(IEventAggregator events,SalesViewModel salesVM,ILoggedInUserModel loggedInUserModel) 
        {
            _events = events;
            _salesVm = salesVM;

            _events.Subscribe(this);

            ActivateItemAsync(IoC.Get<LoginViewModel>());
            _loggedInUserModel=loggedInUserModel;

        }
        public bool IsLoggeIn
        {
            get
            {
                if (!string.IsNullOrWhiteSpace( _loggedInUserModel.Toekn))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void ExitApplication()
        {
            TryCloseAsync();
        }


        public void LogOut()
        {
            _loggedInUserModel.LogOffUser();
            ActivateItemAsync(IoC.Get<LoginViewModel>());
            NotifyOfPropertyChange(() => IsLoggeIn);

        }
        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(_salesVm);
            NotifyOfPropertyChange(() => IsLoggeIn);
        }
    }
}
