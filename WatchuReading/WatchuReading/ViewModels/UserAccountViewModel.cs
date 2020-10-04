using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Windows.Input;
using WatchuReading.Data;
using WatchuReading.Interfaces;
using WhatchaReading.Models;
using WatchuReading.Services;
using WatchuReading.Views;
using Xamarin.Forms;
using static WatchuReading.Enums;

namespace WatchuReading.ViewModels
{
    public class UserAccountViewModel : BaseViewModel
	{
		
        #region Commands
		public ICommand MyBookshelfCommand { get; private set; }
        public Command ShowViewCommand { get; set; }
        public Command LoginCommand { get; set; }
        public Command AddNewCommand { get; set; }
        public Command SendPasswordCommand { get; set; }
        public Command NavigationCommand { get; set; }
        public Command OnRelandingCommand { get; }
       
        IDataStore<User> UserService => DependencyService.Get<IDataStore<User>>();

        #endregion

        //CONSTRUCTOR
        public UserAccountViewModel()
		{

			MyBookshelfCommand = new Command<string>(HandleAction);
			                                       
            ShowViewCommand = new Command<string>(ShowView);
            LoginCommand = new Command(async () => await IsValidLogin());
            AddNewCommand = new Command(async () => await CreateAccount());
            SendPasswordCommand = new Command(async() => await SendPasswordEmail());
            OnRelandingCommand = new Command(async () => await CheckUserStatus());
        }
        
		void HandleAction(string action)
        {
         //Not used for now...might change
        }


        //When logging in for the first time, the user can navigate back to the landing page.
        //If a userid exists, we will push them back to the previous page
        private async Task CheckUserStatus()
        {
            if(UserId != 0)
            {
                await App.Current.MainPage.Navigation.PushAsync(new UserHomePage());   
            }
        }

        //SEND EMAIL TO RESET PASSWORD    //TODO Chadd
        public async Task SendPasswordEmail()
        {
            User u = new User();
            u.Email = "chadd@xamarin.com";
           await  _manager.SendPassWordResetEmail(u);
            AzureDB az = new AzureDB();  
            //get users email  
           // az.SendPassWordResetEmail();

        }

        //CREATE THE ACCOUNT
        public async Task CreateAccount()
        {
            IsBusy = true;
            EnableAddButton = false;
            _manager = new ServiceManager();

            var user = new User() { UserName = NewUserName, PassWord = NewPassword, FirstName = FName, LastName = LName.TrimStart(), Email=Email };
            try
            {
                var rez = await _manager.CreateAccountAsync(user);
                //Show ackowledgement 
                DependencyService.Get<IMessage>().ShowSnackbar("Account Created!  Please Log In To Continue.");

                //Show Login Screen
                ShowView(ViewAction.Login.ToString());

            }

            catch (Exception ex)
            {
                //TODO LOG THIS 
                DependencyService.Get<IMessage>().ShowSnackbar("Sorry, Could not create your account at this time.");
            }

            finally{
                IsBusy = false;
                EnableAddButton = true;
            }

        }

        void ShowView(string vtype)
        {
            ShowNewUser = false;
            ShowLogin = false;
            ShowButtons = false;

            var nav = (ViewAction)Enum.Parse(typeof(ViewAction), vtype);
            switch (nav)
            {
                case ViewAction.Login:
                    ShowLogin = true;
                    break;
                case ViewAction.New:
                    ShowNewUser = true;
                    break;
                case ViewAction.Default:
                ShowButtons = true;
                    break;

            }
        }

        //LOG INTO ACCOUNT
        async Task<bool> IsValidLogin()
        {
            IsBusy = true;

            try
            {
                _manager = new ServiceManager();
                var rez = await _manager.LogInUserAsync(UserName, Password);
                //Success
                if (rez?.Id != 0)
                {
                    //Save Id to settings
                    UserId = rez.Id;
                    Logon = rez.UserName;
                    FirstName = rez.FirstName; 

                    //SEND USER TO HOME PAGE
                    await App.Current.MainPage.Navigation.PushAsync(new UserHomePage());
                  
                }
                else
                {
                    DependencyService.Get<IMessage>().ShowSnackbar("Sorry, Incorrect Login");
                    CheckAttempt();
                }

            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().ShowSnackbar("Sorry, Unable to log you in right now.");;
            }
            finally
            {
                IsBusy = false;
            }

            return true;
        }

        // Validation
        private void CheckFormProps(bool isLogin)
        {
            //Login
            if (isLogin)
            {
                EnableLoginButton = (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password));

            }
            //Add New //TODO VALIDATION
            else{
                EnableAddButton = (!string.IsNullOrEmpty(NewUserName) && !string.IsNullOrEmpty(NewPassword)
                                   && !string.IsNullOrEmpty(LName) && !string.IsNullOrEmpty(FName) &&
                                   !string.IsNullOrEmpty(Email));
            }
        }


        //Login Attemp
        private void CheckAttempt()
        {
            ForgotPassword = _attemptNo++ > 3;
        }

        #region Properties

        private bool _showButtons = true;
        public bool ShowButtons
        {
            get { return _showButtons; }
            set { this.SetProperty(ref _showButtons, value); }
            
        }
        private bool _enableLoginButton = false;
        public bool EnableLoginButton
        {
            get { return _enableLoginButton; }
            set { this.SetProperty(ref _enableLoginButton, value); }

        }

        private bool _enableAddButton = false;
        public bool EnableAddButton
        {
            get { return _enableAddButton; }
            set { this.SetProperty(ref _enableAddButton, value); }

        }
        private bool _showLogin = false;
        public bool ShowLogin
        {
            get { return _showLogin; }
            set { this.SetProperty(ref _showLogin, value); }
        }

        private bool _showNewUser = false;
        public bool ShowNewUser
        {
            get { return _showNewUser; }
            set { this.SetProperty(ref _showNewUser, value); }
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                this.SetProperty(ref _userName, value);
                CheckFormProps(true);
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                this.SetProperty(ref _password, value);
                CheckFormProps(true);
            }
        }

        private string _newUserName;
        public string NewUserName
        {
            get { return _newUserName; }
            set
            {
                this.SetProperty(ref _newUserName, value);
                CheckFormProps(false);
            }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                this.SetProperty(ref _email, value);
                CheckFormProps(false);
            }
        }



        private string _newPassword;
        public string NewPassword
        {
            get { return _newPassword; }
            set
            {
                this.SetProperty(ref _newPassword, value);
                CheckFormProps(false);
            }
        }
       
        private string _fName;
        public string FName
        {
            get { return _fName; }
            set
            {
                this.SetProperty(ref _fName, value);
                CheckFormProps(false);
            }
        }

        private string _lName;
        public string LName
        {
            get { return _lName; }
            set
            {
                this.SetProperty(ref _lName, value);
                CheckFormProps(false);
            }
        }

        private int _attemptNo = 0;

        private bool _forgotPassword = false;
        public bool ForgotPassword {
            get { return _forgotPassword; }
            set
            {
                this.SetProperty(ref _forgotPassword, value);
                
            }
            
        }
        private bool _isFailedLogin;
        public bool IsFailedLogin
        {
            get { return _isFailedLogin; }
            set { this.SetProperty(ref _isFailedLogin, value); }
        }

     
        #endregion
    }
}
