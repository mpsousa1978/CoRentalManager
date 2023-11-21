using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Caliburn.Micro;
using MPSWPFDesktopUI.EventsModel;


namespace MPSWPFDesktopUI.ViewModels
{
    public class ShellViewModel:Conductor<object>,IHandle<LogOnEvent>
    {
        private IEventAggregator _events;
        private SalesViewModel _salesVm;
        private SimpleContainer _container;

        [Obsolete]
        public ShellViewModel(IEventAggregator events,SalesViewModel salesVM,SimpleContainer container) 
        {
            _events = events;
            _salesVm = salesVM;
            _container = container;

            _events.Subscribe(this);

            ActivateItemAsync(_container.GetInstance<LoginViewModel>());

        }


        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(_salesVm);
        }
    }
}
