namespace CatanGame.Models
{
    public abstract class AvatarModel
    {
        public string[] ColorCodes { get; set; } = [Strings.CyanCode,Strings.BlueCode,Strings.DeepPurpleCode,Strings.BrownCode,Strings.LightGreenCode,Strings.PurpleCode,Strings.LightBlueCode,Strings.GreenCode,Strings.BlueGreyCode,
                Strings.TealCode,Strings.IndigoCode,Strings.GreyCode,Strings.YellowGreenCode,Strings.PinkCode,Strings.RedCode,Strings.OrangeRedCode,Strings.OrangeCode,Strings.YellowCode,Strings.AmberCode];
        public Colors[] SelectedColors { get; set; } = [];
        public Eyes[] SelectedEyes { get; set; } = [];
        public Mouth[] SelectedMouths { get; set; } = [];
        public Sides[] SelectedSides { get; set; } = [];
        public Texture[] SelectedTextures { get; set; } = [];
        public Face[] SelectedFaces { get; set; } = [];
        public Top[] SelectedTops { get; set; } = [];

        public enum Colors
        {
            none,
            cyan,
            blue,
            deepPurple,
            brown,
            lightGreen,
            purple,
            lightBlue,
            green,
            blueGrey,
            teal,
            indigo,
            grey,
            yellowGreen,
            pink,
            red,
            orangeRed,
            orange,
            yellow,
            amber
        }
        public enum Eyes
        {
            none,
            bulging,
            dizzy,
            eva,
            frame1,
            frame2,
            glow,
            happy,
            hearts,
            robocop,
            round,
            roundFrame01,
            roundFrame02,
            sensor,
            shade01
        }
        public enum Mouth
        {
            none,
            bite,
            diagram,
            grill01,
            grill02,
            grill03,
            smile01,
            smile02,
            square01,
            square02
        }
        public enum Sides
        {
            none,
            antenna01,
            antenna02,
            cables01,
            cables02,
            round,
            aquare,
            aquareAssymetric
        }
        public enum Texture
        {
            none,
            camo01,
            camo02,
            circuits,
            dirty01,
            dirty02,
            dots,
            grunge01,
            grunge02
        }
        public enum Face
        {
            none,
            round01,
            round02,
            square01,
            square02,
            square03,
            square04
        }
        public enum Top
        {
            none,
            antenna,
            antennaCrooked,
            bulb01,
            glowingBulb01,
            glowingBulb02,
            horns,
            lights,
            pyramid,
            radar
        }

        public abstract string GetUrlWithString(string seed);
    }
}