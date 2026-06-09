using Firebase.Auth;
using Firebase.Auth.Providers;
using Plugin.CloudFirestore;

namespace CatanGame.Models
{
    public abstract class FbDataModel
    {
        #region Fields
        protected FirebaseAuthClient facl;
        protected IFirestore fdb;
        #endregion

        #region Properties
        public abstract string DisplayName { get; }
        public abstract string UserID { get; }
        #endregion

        #region Constructor
        public FbDataModel()
        {
            FirebaseAuthConfig fac = new()
            {
                ApiKey = Keys.FbApiKey,
                AuthDomain = Keys.FbApiAuthDomain,
                Providers = [new EmailProvider()]
            };
            facl = new FirebaseAuthClient(fac);
            fdb = CrossCloudFirestore.Current.Instance;
        }
        #endregion

        #region PublicMethods
        // Converts Firebase error text into a user friendly message.
        public static string GetErrorMessage(string msg) => msg;
        // Creates a user account with Firebase auth.
        public abstract void CreateUserWithEmailAndPasswordAsync(string email, string password, string name, Action<Task> OnComplete);
        // Signs in a user with Firebase auth.
        public abstract void SignInWithEmailAndPasswordAsync(string email, string password, Action<Task> OnComplete);
        // Signs out the current user.
        public abstract void SignOut();
        // Sends a password reset request.
        public abstract void ResetPassword(string email, Action<Task> OnComplete);
        // Deletes a document and reports completion.
        public abstract void DeleteDocument(string collectonName, string id, Action<Task> onComplete);
        // Deletes a document.
        public abstract void DeleteDocument(string collectonName, string id);
        // Updates document fields and reports completion.
        public abstract void UpdateFields(string collectonName, string id, Dictionary<string, object> dict, Action<Task> OnComplete);
        // Updates document fields.
        public abstract void UpdateFields(string collectonName, string id, Dictionary<string, object> dict);
        // Gets one document by collection and id.
        public abstract void GetDocument(string collectonName, string documentName, Action<IDocumentSnapshot> OnComplete);
        // Gets documents whose field is less than a value.
        public abstract void GetDocumentsWhereLessThan(string collectonName, string fName, object fValue, Action<IQuerySnapshot> OnComplete);
        // Gets documents whose field equals a value.
        public abstract void GetDocumentsWhereEqualTo(string collectonName, string fName, object fValue, Action<IQuerySnapshot> OnComplete);
        // Saves a document and returns its id.
        public abstract string SetDocument(object obj, string collectonName, string id, Action<Task> OnComplete);
        // Adds a collection snapshot listener.
        public abstract IListenerRegistration AddSnapshotListener(string collectonName, Plugin.CloudFirestore.QuerySnapshotHandler OnChange);
        // Adds a document snapshot listener.
        public abstract IListenerRegistration AddSnapshotListener(string collectonName, string id, Plugin.CloudFirestore.DocumentSnapshotHandler OnChange);
        #endregion
    }
}
