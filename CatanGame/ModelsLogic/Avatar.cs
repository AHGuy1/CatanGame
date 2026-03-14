using CatanGame.Models;

namespace CatanGame.ModelsLogic
{
    public class Avatar : AvatarModel
    {
        #region Constructor
        public Avatar()
        {
        }
        #endregion

        #region Public Methods
        public static string LowercaseFirstChar(string value)
        {
            return char.ToLower(value[0]) + value[1..];
        }

        public override string GetUrlWithString(string seed)
        {
            string url = Keys.AvatrBaseUrl;
            string parameter = string.Empty;
            for (int i = 0; i < SelectedColors.Length; i++)
                if (SelectedColors[i] != Colors.None)
                {
                    if (parameter == string.Empty)
                        parameter += Strings.ColorLabel + Strings.EqualSign + LowercaseFirstChar(ColorCodes[((int)SelectedColors[i]) - 1]);
                    else
                        parameter += Strings.Comma + LowercaseFirstChar(ColorCodes[((int)SelectedColors[i]) - 1]);
                }
            if (parameter != string.Empty)
                url += parameter + Strings.Ampersand;
            parameter = string.Empty;
            for (int i = 0; i < SelectedEyes.Length; i++)
                if (SelectedEyes[i] != Eyes.None)
                {
                    if (parameter == string.Empty)
                        parameter += Strings.EyesLabel + Strings.EqualSign + LowercaseFirstChar(SelectedEyes[i].ToString());
                    else
                        parameter += Strings.Comma + LowercaseFirstChar(SelectedEyes[i].ToString());
                }
            if (parameter != string.Empty)
                url += parameter + Strings.Ampersand;
            parameter = string.Empty;
            for (int i = 0; i < SelectedMouths.Length; i++)
                if (SelectedMouths[i] != Mouth.None)
                {
                    if (parameter == string.Empty)
                        parameter += Strings.MouthLabel + Strings.EqualSign + LowercaseFirstChar(SelectedMouths[i].ToString());
                    else
                        parameter += Strings.Comma + LowercaseFirstChar(SelectedMouths[i].ToString());
                }
            if (parameter != string.Empty)
                url += parameter + Strings.Ampersand;
            parameter = string.Empty;
            for (int i = 0; i < SelectedSides.Length; i++)
                if (SelectedSides[i] != Sides.None)
                {
                    if (parameter == string.Empty)
                        parameter += Strings.SidesLabel + Strings.EqualSign + LowercaseFirstChar(SelectedSides[i].ToString());
                    else
                        parameter += Strings.Comma + LowercaseFirstChar(SelectedSides[i].ToString());
                }
            if (parameter != string.Empty)
                url += parameter + Strings.Ampersand;
            parameter = string.Empty;
            for (int i = 0; i < SelectedTextures.Length; i++)
                if (SelectedTextures[i] != Texture.None)
                {
                    if (parameter == string.Empty)
                        parameter += Strings.TextureLabel + Strings.EqualSign + LowercaseFirstChar(SelectedTextures[i].ToString());
                    else
                        parameter += Strings.Comma + LowercaseFirstChar(SelectedTextures[i].ToString());
                }
            if (parameter != string.Empty)
                url += parameter + Strings.Ampersand;
            parameter = string.Empty;
            for (int i = 0; i < SelectedFaces.Length; i++)
                if (SelectedFaces[i] != Face.None)
                {
                    if (parameter == string.Empty)
                        parameter += Strings.FaceLabel + Strings.EqualSign + LowercaseFirstChar(SelectedFaces[i].ToString());
                    else
                        parameter += Strings.Comma + LowercaseFirstChar(SelectedFaces[i].ToString());
                }
            if (parameter != string.Empty)
                url += parameter + Strings.Ampersand;
            parameter = string.Empty;
            for (int i = 0; i < SelectedTops.Length; i++)
                if (SelectedTops[i] != Top.None)
                {
                    if (parameter == string.Empty)
                        parameter += Strings.TopLabel + Strings.EqualSign + LowercaseFirstChar(SelectedTops[i].ToString());
                    else
                        parameter += Strings.Comma + LowercaseFirstChar(SelectedTops[i].ToString());
                }
            if (parameter != string.Empty)
                url += parameter + Strings.Ampersand;
            return url += Strings.SeedLabel + Strings.EqualSign + seed;
        }
        #endregion
    }
}
