using CatanGame.Models;

namespace CatanGame.ModelsLogic
{
    public class Board : BoardModel
    {
        private static readonly Dictionary<string, BoardModel.TerrainType> TerrainMap = new()
        {
            { Board.TerrainType.Forest.ToString(),   BoardModel.TerrainType.Forest },
            { Board.TerrainType.Fields.ToString(),   BoardModel.TerrainType.Fields },
            { Board.TerrainType.Hills.ToString(),    BoardModel.TerrainType.Hills },
            { Board.TerrainType.Mountien.ToString(), BoardModel.TerrainType.Mountien },
            { Board.TerrainType.Pasture.ToString(),  BoardModel.TerrainType.Pasture },
            { Board.TerrainType.Desert.ToString(),   BoardModel.TerrainType.Desert }
        };
        private static readonly Dictionary<string, int> NumberTokenMap = new()
        {
            { Strings.TwoImage,    2 },
            { Strings.ThreeImage,  3 },
            { Strings.FourImage,   4 },
            { Strings.FiveImage,   5 },
            { Strings.SixImage,    6 },
            { Strings.EightImage,  8 },
            { Strings.NineImage,   9 },
            { Strings.TenImage,    10 },
            { Strings.ElevenImage, 11 },
            { Strings.TwelveImage, 12 }
        };

        private static BoardModel.TerrainType GetTerrainFromTileType(string tileType)
        {
            // Check what value the tileType contains from the terrainMap, and return the corresponding TerrainType
            foreach (KeyValuePair<string, BoardModel.TerrainType> entry in TerrainMap)
            {
                if (tileType.Contains(entry.Key))
                {
                    return entry.Value;
                }
            }
            //Should not happen
            return BoardModel.TerrainType.None;
        }
        private static int GetNumberTokenFromTile(string tileNumber)
        {
            if (NumberTokenMap.TryGetValue(tileNumber, out int value))
            {
                return value;
            }
            //No number token(desert)
            return 0;
        }

        protected override void InitHex(string[] tileTypes, string[] tileNumbers)
        {
            // Initialize the hex tiles on the game board
            for (int i = 1; i < 6; i++)
            {
                for (int k = 1; k < 1 + GameGrid.GetAmountOfColumnsTiles(i); k++)
                {
                    int location = GameGrid.GetTileLocationInArray(i, k);
                    Hexes[location] = new(i, k, GetTerrainFromTileType(tileTypes[location]), GetNumberTokenFromTile(tileNumbers[location]));
                }
            }
        }

        public override void InitBoard(IndexedButton[][] pices, string[] tileTypes, string[] tileNumbers)
        {
            InitHex(tileTypes, tileNumbers);
        }
    }
}