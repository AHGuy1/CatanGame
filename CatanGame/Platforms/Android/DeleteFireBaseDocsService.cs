using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using CatanGame.Models;
using CatanGame.ModelsLogic;
using Plugin.CloudFirestore;


namespace CatanGame.Platforms.Android
{
    [Service]
    public class DeleteFireBaseDocsService : Service
    {
        private bool isRuning = true;
        private readonly FbData fbd = new();

        [return: GeneratedEnum]

        private void DeleteFBDocs()
        {
            while (isRuning)
            {
                fbd.GetDocumentsWhereLessThan(Keys.GamesCollection, nameof(Game.Created), DateTime.Now.AddDays(-1), OnComplete);
                Thread.Sleep(Keys.TheredSleepTime);
            }
            StopSelf();
        }

        private void OnComplete(IQuerySnapshot qs)
        {
            foreach(IDocumentSnapshot doc in qs.Documents)
            {
                fbd.DeleteDocument(Keys.GamesCollection, doc.Id, (task) => { });
                fbd.DeleteDocument(Keys.GameCodesCollection, doc.ToObject<Game>()!.GameCode, (task) => { });
            }
        }

        public override StartCommandResult OnStartCommand(Intent? intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            ThreadStart threadStart = new(DeleteFBDocs);
            Thread thread = new(threadStart);
            thread.Start();
            return base.OnStartCommand(intent, flags, startId);
        }
        public override IBinder? OnBind(Intent? intent)
        {
            return null;
        }
        public override void OnDestroy()
        {
            isRuning = false;
            base.OnDestroy();
        }
    }
}
