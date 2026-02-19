using Android.Hardware.Lights;
using static Android.InputMethodServices.Keyboard;

namespace CatanGame.Models
{
    public class Avatar
    {
        public string Url { get; set; } = Keys.AvatrBaseUrl;

        Dictionary<string, string> colors = new Dictionary<string, string>
        {
            { "Black", "#000000" },
            { "Cyan", "#00ACC1" },
            { "Blue", "#1E88E5" },
            { "Deep Purple", "#5E35B1" },
            { "Brown", "#6D4C41" },
            { "Light Green", "#7CB342" },
            { "Purple", "#8E24AA" },
            { "Light Blue", "#039BE5" },
            { "Green", "#43A047" },
            { "Blue Grey", "#546E7A" },
            { "Teal", "#00897B" },
            { "Indigo", "#3949AB" },
            { "Grey", "#757575" },
            { "Lime", "#C0CA33" },
            { "Pink", "#D81B60" },
            { "Red", "#E53935" },
            { "Orange Red", "#F4511E" },
            { "Orange", "#FB8C00" },
            { "Yellow", "#FDD835" },
            { "Amber", "#FFB300" }
        };
        public enum Eyes
        {
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
        public enum Moth
        {
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
            antenna01,
            antenna02,
            cables01,
            cables02,
            round,
            square,
            squareAssymetric
        }
        public enum Texture
        {
            Camo01,
            Camo02,
            Circuits,
            Dirty01,
            Dirty02,
            Dots,
            Grunge01,
            Grunge02
        }

    }
}

