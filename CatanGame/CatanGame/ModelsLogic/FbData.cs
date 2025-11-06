using CatanGame.Models;
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

        public override async void CreateUserWithEmailAndPasswordAsync(string email, string password, string name, Action<Task> OnComplete)
        {
            await facl.CreateUserWithEmailAndPasswordAsync(email, password, name).ContinueWith(OnComplete);
        }

        public override async void SignInWithEmailAndPasswordAsync(string email, string password, Action<Task> OnComplete)
        {
           await facl.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(OnComplete);
        }
        public override async void ResetPassword(string email, Action<Task> OnComplete)
        {
           await facl.ResetEmailPasswordAsync(email).ContinueWith(OnComplete);
        }

        public override string SetDocument(object obj, string collectonName, string id, Action<Task> OnComplete)
        {
            IDocumentReference dr = string.IsNullOrEmpty(id) ? fdb.Collection(collectonName).Document() : fdb.Collection(collectonName).Document(id);
            dr.SetAsync(obj).ContinueWith(OnComplete);
            return dr.Id;
        }

        public void GetDocument(string collectonName, string documentName, Action<Task> OnComplete)
        {
            IDocumentReference dr = fdb.Collection(collectonName).Document(documentName);

        }

        public override IListenerRegistration AddSnapshotListener(string collectonName, QuerySnapshotHandler OnChange)
        {
            ICollectionReference cr = fdb.Collection(collectonName);
            return cr.AddSnapshotListener(OnChange);
        }
        public override IListenerRegistration AddSnapshotListener(string collectonName, string id, DocumentSnapshotHandler OnChange)
        {
            IDocumentReference cr = fdb.Collection(collectonName).Document(id);
            return cr.AddSnapshotListener(OnChange);
        }
        public async void GetDocumentsWhereEqualTo(string collectonName, string fName, object fValue, Action<IQuerySnapshot> OnComplete)
        {
            ICollectionReference cr = fdb.Collection(collectonName);
            IQuerySnapshot qs = await cr.WhereEqualsTo(fName, fValue).GetAsync();
            OnComplete(qs);
        }

        public static string GetErrorMessage(string msg)
        {
            if (msg.Contains(Strings.ContainsINVALID_LOGIN_CREDENTIALS))
                msg = Strings.InvalidCredentialsMessage;
            else if (msg.Contains(Strings.ContainsReason))
            {
                int pos = msg.IndexOf(Strings.ContainsReason);
                msg = msg.Substring((pos + 7), msg.Length - pos - 8);
                for (int i = 1; i < msg.Length; i++)
                {
                    pos = 0;
                    if (char.IsUpper(msg[i]))
                    {
                        msg = string.Concat(msg.AsSpan(pos, i), Strings.EmptySpace, msg.AsSpan(i));
                        pos = i + 1;
                        i++;
                    }
                }
            }
            return msg;
        }
    }
}
