using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Caliburn.Micro;
using MPSWPFDesktopUI.EventsModel;
using MPSWPFDesktopUI.Library.Api;
using MPSWPFDesktopUI.Library.Models;


namespace MPSWPFDesktopUI.ViewModels
{
    public class ShellViewModel:Conductor<object>,IHandle<LogOnEvent>
    {
        private IEventAggregator _events;
        private ILoggedInUserModel _user;
        private readonly ApiHelper _apiHelper;

        public ShellViewModel(IEventAggregator events, ILoggedInUserModel user, ApiHelper apiHelper) 
        {
            _events = events;

            _events.SubscribeOnPublishedThread(this);

            ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken());
            _user= user;
            _apiHelper = apiHelper;
        }
        public bool IsLoggeIn
        {
            get
            {
                if (!string.IsNullOrWhiteSpace( _user.Toekn))
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

        public async Task UserManagement()
        {
            await ActivateItemAsync(IoC.Get<UserDisplayViewModel>(), new CancellationToken());
        }
        public async Task LogOut()
        {
            _user.ResetUser();
            //_apiHelper.LogOffUser();
            await ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken());
            NotifyOfPropertyChange(() => IsLoggeIn);

        }
        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(IoC.Get<SalesViewModel>(), cancellationToken);
            NotifyOfPropertyChange(() => IsLoggeIn);
        }
    }
}
