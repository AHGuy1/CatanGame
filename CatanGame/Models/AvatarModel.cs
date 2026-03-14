namespace CatanGame.Models
{
    public abstract class AvatarModel
    {
        #region Enums
        public enum Colors
        {
            None,
            Cyan,
            Blue,
            DeepPurple,
            Brown,
            LightGreen,
            Purple,
            LightBlue,
            Green,
            BlueGrey,
            Teal,
            Indigo,
            Grey,
            YellowGreen,
            Pink,
            Red,
            OrangeRed,
            Orange,
            Yellow,
            Amber
        }

        public enum Eyes
        {
            None,
            Bulging,
            Dizzy,
            Eva,
            Frame1,
            Frame2,
            Glow,
            Happy,
            Hearts,
            Robocop,
            Round,
            RoundFrame01,
            RoundFrame02,
            Sensor,
            Shade01
        }

        public enum Mouth
        {
            None,
            Bite,
            Diagram,
            Grill01,
            Grill02,
            Grill03,
            Smile01,
            Smile02,
            Square01,
            Square02
        }

        public enum Sides
        {
            None,
            Antenna01,
            Antenna02,
            Cables01,
            Cables02,
            Round,
            Aquare,
            AquareAssymetric
        }

        public enum Texture
        {
            None,
            Camo01,
            Camo02,
            Circuits,
            Dirty01,
            Dirty02,
            Dots,
            Grunge01,
            Grunge02
        }

        public enum Face
        {
            None,
            Round01,
            Round02,
            Square01,
            Square02,
            Square03,
            Square04
        }

        public enum Top
        {
            None,
            Antenna,
            AntennaCrooked,
            Bulb01,
            GlowingBulb01,
            GlowingBulb02,
            Horns,
            Lights,
            Pyramid,
            Radar
        }
        #endregion

        #region Properties
        public string[] ColorCodes { get; set; } = [Strings.CyanCode,Strings.BlueCode,Strings.DeepPurpleCode,Strings.BrownCode,Strings.LightGreenCode,Strings.PurpleCode,Strings.LightBlueCode,Strings.GreenCode,
        Strings.BlueGreyCode,Strings.TealCode,Strings.IndigoCode,Strings.GreyCode,Strings.YellowGreenCode,Strings.PinkCode,Strings.RedCode,Strings.OrangeRedCode,Strings.OrangeCode,Strings.YellowCode,Strings.AmberCode];
        public Colors[] SelectedColors { get; set; } = [];
        public Eyes[] SelectedEyes { get; set; } = [];
        public Mouth[] SelectedMouths { get; set; } = [];
        public Sides[] SelectedSides { get; set; } = [];
        public Texture[] SelectedTextures { get; set; } = [];
        public Face[] SelectedFaces { get; set; } = [];
        public Top[] SelectedTops { get; set; } = [];
        #endregion

        #region PublicMethods
        public abstract string GetUrlWithString(string seed);
        #endregion
    }
}
