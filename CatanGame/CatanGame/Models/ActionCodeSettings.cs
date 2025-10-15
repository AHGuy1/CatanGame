namespace CatanGame.Models
{
    internal class ActionCodeSettings
    {
        public ActionCodeSettings()
        {
        }

        public string Url { get; set; }
        public bool HandleCodeInApp { get; set; }
        public string iOSBundleId { get; set; }
        public string AndroidPackageName { get; set; }
        public bool linkDomain { get; set; }
    }
}