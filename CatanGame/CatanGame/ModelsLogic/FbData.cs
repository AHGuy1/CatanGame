using CatanGame.Models;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Plugin.CloudFirestore;

namespace CatanGame.ModelsLogic
{
    public class FbData : FbDataModel
    {
        public override string DisplayName
        {
            get
            {
                string dn = string.Empty;
                if (facl.User != null)
                    dn = facl.User.Info.DisplayName;

                return dn;
            }
        }

        public override string UserID
        {
            get
            {
                return facl.User.Uid;
            }
        }

        public override async void CreateUserWithEmailAndPasswordAsync(string email, string password, string name, Action<System.Threading.Tasks.Task> OnComplete)
        {
            await facl.CreateUserWithEmailAndPasswordAsync(email, password, name).ContinueWith(OnComplete);
        }

        public override async void SignInWithEmailAndPasswordAsync(string email, string password, Action<System.Threading.Tasks.Task> OnComplete)
        {
            await facl.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(OnComplete);
        }
        public override async void SendSignInLinkToEmail(string email, Action<System.Threading.Tasks.Task> OnComplete)
        {
            var acs = new ActionCodeSettings()
            {
                Url = "https://www.example.com/finishSignUp?cartId=1234",
                HandleCodeInApp = true,
                iOSBundleId = "com.CatanGame.ios",
                AndroidPackageName = "com.CatanGame.ios",
                linkDomain = true,
            };
        }   

    }
}
