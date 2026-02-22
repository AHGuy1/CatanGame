using CatanGame.Models;

namespace CatanGame.ModelsLogic
{
    public class Avatar : AvatarModel
    {
        public Avatar()
        {
        }

        public override string GetUrlWithString(string seed)
        {
            string url = Keys.AvatrBaseUrl;
            string parameter = string.Empty;
            for (int i = 0; i < SelectedColors.Length; i++)
                if (SelectedColors[i] != Colors.none)
                {
                    if (parameter == string.Empty)
                        parameter += Strings.ColorLabel + Strings.EqualSign + ColorCodes[((int)SelectedColors[i]) - 1];
                    else
                        parameter += Strings.Comma + ColorCodes[((int)SelectedColors[i]) - 1];
                }
            if (parameter != string.Empty)
                url += parameter + Strings.Ampersand;
            parameter = string.Empty;
            for (int i = 0; i < SelectedEyes.Length; i++)
                if (SelectedEyes[i] != Eyes.none)
                {
                    if (parameter == string.Empty)
                        parameter += Strings.EyesLabel + Strings.EqualSign + SelectedEyes[i].ToString();
                    else
                        parameter += Strings.Comma + SelectedEyes[i].ToString();
                }
            if (parameter != string.Empty)
                url += parameter + Strings.Ampersand;
            parameter = string.Empty;
            for (int i = 0; i < SelectedMouths.Length; i++)
                if (SelectedMouths[i] != Mouth.none)
                {
                    if (parameter == string.Empty)
                        parameter += Strings.MouthLabel + Strings.EqualSign + SelectedMouths[i].ToString();
                    else
                        parameter += Strings.Comma + SelectedMouths[i].ToString();
                }
            if (parameter != string.Empty)
                url += parameter + Strings.Ampersand;
            parameter = string.Empty;
            for (int i = 0; i < SelectedSides.Length; i++)
                if (SelectedSides[i] != Sides.none)
                {
                    if (parameter == string.Empty)
                        parameter += Strings.SidesLabel + Strings.EqualSign + SelectedSides[i].ToString();
                    else
                        parameter += Strings.Comma + SelectedSides[i].ToString();
                }
            if (parameter != string.Empty)
                url += parameter + Strings.Ampersand;
            parameter = string.Empty;
            for (int i = 0; i < SelectedTextures.Length; i++)
                if (SelectedTextures[i] != Texture.none)
                {
                    if (parameter == string.Empty)
                        parameter += Strings.TextureLabel + Strings.EqualSign + SelectedTextures[i].ToString();
                    else
                        parameter += Strings.Comma + SelectedTextures[i].ToString();
                }
            if (parameter != string.Empty)
                url += parameter + Strings.Ampersand;
            parameter = string.Empty;
            for (int i = 0; i < SelectedFaces.Length; i++)
                if (SelectedFaces[i] != Face.none)
                {
                    if (parameter == string.Empty)
                        parameter += Strings.FaceLabel + Strings.EqualSign + SelectedFaces[i].ToString();
                    else
                        parameter += Strings.Comma + SelectedFaces[i].ToString();
                }
            if (parameter != string.Empty)
                url += parameter + Strings.Ampersand;
            parameter = string.Empty;
            for (int i = 0; i < SelectedTops.Length; i++)
                if (SelectedTops[i] != Top.none)
                {
                    if (parameter == string.Empty)
                        parameter += Strings.TopLabel + Strings.EqualSign + SelectedTops[i].ToString();
                    else
                        parameter += Strings.Comma + SelectedTops[i].ToString();
                }
            if (parameter != string.Empty)
                url += parameter + Strings.Ampersand;
            return url += Strings.SeedLabel + Strings.EqualSign + seed;
        }
    }
}
