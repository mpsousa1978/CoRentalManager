using Caliburn.Micro;
using MPSWPFDesktopUI.Helper;
using MPSWPFDesktopUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSWPFDesktopUI.ViewModels
{
    public class LoginViewModel:Screen
    {

        private string _userName;
        private string _password;
        private IApiHelper _apiHelper;
        public LoginViewModel(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }
        public string UserName
        {
            get { return _userName; }
            set { 
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }


        
        public bool CanLogIn
        {
            get
            {
                if (UserName?.Length > 0 && Password?.Length > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private bool _isErrorVisible;

        public bool IsErrorVisible
        {
            get 
            {
                if (ErrorMessage?.Length > 0)
                {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                NotifyOfPropertyChange(() => ErrorMessage);
                NotifyOfPropertyChange(() => IsErrorVisible);

            }
        }

        public async Task LogIn()
        {

            try
            {
                ErrorMessage = "";
                var result = await _apiHelper.Authenticate(UserName, Password);
            }
            catch (Exception ex)
            {

                ErrorMessage = ex.Message;
            }

        }
    }
}
