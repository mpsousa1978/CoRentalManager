﻿using Caliburn.Micro;
using MPSWPFDesktopUI.Library.Api;
using MPSWPFDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MPSWPFDesktopUI.ViewModels
{
    public class UserDisplayViewModel:Screen
    {
        private readonly StatusInfoViewModel _status;
        private readonly IWindowManager _window;
        private readonly IUserEndPoint _userEndPoint;

        BindingList<UserModel> _users;

        public BindingList<UserModel> Users
        {
            get { return _users; }
            set { _users = value;
                NotifyOfPropertyChange(() => Users);
            }  
        }

        private UserModel _selectedUser;

        public UserModel SelectedUser
        {
            get { return _selectedUser; }
            set {
                _selectedUser = value;
                SelectedUserName = value.Email;
                UserRoles = new BindingList<string>(value.Roles.Select(x => x.Value).ToList());
                LoadRoles();
                NotifyOfPropertyChange(() => SelectedUser);
            }
        }

        private string _selectedUserName;

        public string SelectedUserName
        {
            get { return _selectedUserName; }
            set { _selectedUserName = value;
                NotifyOfPropertyChange(() => SelectedUserName);
            }
        }


        private BindingList<string> _userRoles = new BindingList<string>();

        public BindingList<string> UserRoles
        {
            get { return _userRoles; }
            set {
                _userRoles = value;
                NotifyOfPropertyChange(() => UserRoles);
            }
        }


        private BindingList<string> _availableRoles = new BindingList<string>();

        public BindingList<string> AvailableRoles
        {
            get { return _availableRoles; }
            set
            {
                _availableRoles = value;
                NotifyOfPropertyChange(() => AvailableRoles);
            }
        }

        private async Task LoadRoles()
        {
            var roles = await _userEndPoint.GetAllRoles();

            foreach(var role in roles)
            {
                if (UserRoles.IndexOf(role.Value) < 0)
                {
                    AvailableRoles.Add(role.Value);
                }
            }
        }

        private string _selectedUserRole;

        public string SelectedUserRole
        {
            get { return _selectedUserRole; }
            set {
                _selectedUserRole = value;
                NotifyOfPropertyChange(() => SelectedUserRole);
            }
        }


        private string _selectedAvailableRole;

        public string SelectedAvailableRole
        {
            get { return _selectedAvailableRole; }
            set
            {
                _selectedAvailableRole = value;
                NotifyOfPropertyChange(() => SelectedAvailableRole);
            }
        }


        public async void AddSelectedRole()
        {
            await _userEndPoint.AddUserToRole(SelectedUser.Id, SelectedAvailableRole);
            UserRoles.Add(SelectedAvailableRole);
            AvailableRoles.Remove(SelectedAvailableRole);   
        }
        public async void RemoveSelectedRole()
        {
            await _userEndPoint.RemoveFromRole(SelectedUser.Id, SelectedUserRole);
            AvailableRoles.Add(SelectedUserRole);
            UserRoles.Remove(SelectedUserRole);

        }
        public UserDisplayViewModel(StatusInfoViewModel status,IWindowManager window,IUserEndPoint userEndPoint)
        {
            _status = status;
            _window = window;
            _userEndPoint = userEndPoint;
        }
        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            try
            {
                await LoadUser();
            }
            catch (Exception ex)
            {
                dynamic setting = new ExpandoObject();
                setting.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                setting.ResizeMode = ResizeMode.NoResize;
                setting.Title = "System Error";

                //var info = IoC.Get<StatusInfoViewModel>();

                _status.UpDateMessage("Error", ex.Message);
                await _window.ShowDialogAsync(_status, null, setting);
                await TryCloseAsync();
            }
        }

        private async Task LoadUser()
        {
            var userList = await _userEndPoint.GetAll();
            Users = new BindingList<UserModel>(userList);
        }


    }
}
