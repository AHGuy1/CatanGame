using CatanGame.Models;
using Plugin.CloudFirestore;

namespace CatanGame.ModelsLogic
{
    public class FbData : FbDataModel
    {
        #region Properties
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
        #endregion

        #region Public Methods
        // Converts Firebase error text into a user friendly message.
        public new static string GetErrorMessage(string msg)
        {
            if (msg.Contains(Strings.ContainsINVALID_LOGIN_CREDENTIALS))
                msg = Strings.InvalidCredentialsMessage;
            else if (msg.Contains(Strings.ContainsReason))
            {
                int pos = msg.IndexOf(Strings.ContainsReason);
                msg = msg.Substring((pos + 7), msg.Length - pos - 8);
                for (int i = 1; i < msg.Length; i++)
                    if (char.IsUpper(msg[i]))
                    {
                        msg = string.Concat(msg.AsSpan(pos, i), Strings.EmptySpace, msg.AsSpan(i));
                        pos = i + 1;
                        i++;
                    }
            }
            return msg;
        }

        // Creates a Firebase user with email, password, and display name.
        public override async void CreateUserWithEmailAndPasswordAsync(string email, string password, string name, Action<Task> OnComplete)
        {
            await facl.CreateUserWithEmailAndPasswordAsync(email, password, name).ContinueWith(OnComplete);
        }

        // Signs in a Firebase user with email and password.
        public override async void SignInWithEmailAndPasswordAsync(string email, string password, Action<Task> OnComplete)
        {
            await facl.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(OnComplete);
        }

        // Sends a password reset email.
        public override async void ResetPassword(string email, Action<Task> OnComplete)
        {
            await facl.ResetEmailPasswordAsync(email).ContinueWith(OnComplete);
        }

        // Updates selected Firestore fields and runs a completion callback.
        public override async void UpdateFields(string collectonName, string id, Dictionary<string, object> dict, Action<Task> OnComplete)
        {
            IDocumentReference dr = fdb.Collection(collectonName).Document(id);
            await dr.UpdateAsync(dict).ContinueWith(OnComplete);
        }

        // Updates selected Firestore fields.
        public override async void UpdateFields(string collectonName, string id, Dictionary<string, object> dict)
        {
                IDocumentReference dr = fdb.Collection(collectonName).Document(id);
                await dr.UpdateAsync(dict);
        }

        // Gets one Firestore document by collection and id.
        public override async void GetDocument(string collectonName, string documentName, Action<IDocumentSnapshot> OnComplete)
        {
            IDocumentReference dr = fdb.Collection(collectonName).Document(documentName);
            IDocumentSnapshot ds = await dr.GetAsync();
            OnComplete(ds);
        }

        // Gets Firestore documents where a field equals a value.
        public override async void GetDocumentsWhereEqualTo(string collectonName, string fName, object fValue, Action<IQuerySnapshot> OnComplete)
        {
            ICollectionReference cr = fdb.Collection(collectonName);
            IQuerySnapshot qs = await cr.WhereEqualsTo(fName, fValue).GetAsync();
            OnComplete(qs);
        }

        // Gets Firestore documents where a field is less than a value.
        public override async void GetDocumentsWhereLessThan(string collectonName, string fName, object fValue, Action<IQuerySnapshot> OnComplete)
        {
            ICollectionReference cr = fdb.Collection(collectonName);
            IQuerySnapshot qs = await cr.WhereLessThan(fName, fValue).GetAsync();
            OnComplete(qs);
        }

        // Deletes a Firestore document and runs a completion callback.
        public override async void DeleteDocument(string collectonName, string id, Action<Task> OnComplete)
        {
            IDocumentReference dr = fdb.Collection(collectonName).Document(id);
            await dr.DeleteAsync().ContinueWith(OnComplete);
        }

        // Deletes a Firestore document.
        public override async void DeleteDocument(string collectonName, string id)
        {
            IDocumentReference dr = fdb.Collection(collectonName).Document(id);
            await dr.DeleteAsync();
        }

        // Creates or replaces a Firestore document and returns its id.
        public override string SetDocument(object obj, string collectonName, string id, Action<Task> OnComplete)
        {
            IDocumentReference dr = string.IsNullOrEmpty(id) ? fdb.Collection(collectonName).Document() : fdb.Collection(collectonName).Document(id);
            dr.SetAsync(obj).ContinueWith(OnComplete);
            return dr.Id;
        }

        // Listens for changes on a Firestore collection.
        public override IListenerRegistration AddSnapshotListener(string collectonName, QuerySnapshotHandler OnChange)
        {
            ICollectionReference cr = fdb.Collection(collectonName);
            return cr.AddSnapshotListener(OnChange);
        }

        // Listens for changes on a Firestore document.
        public override IListenerRegistration AddSnapshotListener(string collectonName, string id, DocumentSnapshotHandler OnChange)
        {
            IDocumentReference cr = fdb.Collection(collectonName).Document(id);
            return cr.AddSnapshotListener(OnChange);
        }
        #endregion
    }
}
