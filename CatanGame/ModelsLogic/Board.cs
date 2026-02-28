using CatanGame.Models;

namespace CatanGame.ModelsLogic
{
    public class Board : BoardModel
    {
        private static readonly Dictionary<string, BoardModel.TerrainType> TerrainMap = new()
        {
            { Board.TerrainType.Forest.ToString().ToLower(),   BoardModel.TerrainType.Forest },
            { Board.TerrainType.Fields.ToString().ToLower(),   BoardModel.TerrainType.Fields },
            { Board.TerrainType.Hills.ToString().ToLower(),    BoardModel.TerrainType.Hills },
            { Board.TerrainType.Mountien.ToString().ToLower(), BoardModel.TerrainType.Mountien },
            { Board.TerrainType.Pasture.ToString().ToLower(),  BoardModel.TerrainType.Pasture },
            { Board.TerrainType.Desert.ToString().ToLower(),   BoardModel.TerrainType.Desert }
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
                if (tileType != null && tileType.Contains(entry.Key))
                {
                    return entry.Value;
                }
            }
            //Should not happen
            return BoardModel.TerrainType.None;
        }
        private static int GetNumberTokenFromTile(string tileNumber)
        {
            // Check what value the tileNumber contains from the NumberTokenMap, and return the corresponding String value
            if (NumberTokenMap.TryGetValue(tileNumber, out int value))
            {
                return value;
            }
            //No number token(desert)
            return 0;
        }

        protected override void InitHex(string[] tileTypes, string[] tileNumbers)
        {
            Hexes = new HexTile[19];
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
        protected override void InitVertices()
        {
            Vertices = new VertexNode[54];
            // Initialize the vertex nodes on the board
            for (int i = 1; i < 24; i += 2)
            {
                for (int k = 0; k < GameGrid.GetAmountOfColumns(i) - 1; k++)
                {
                    Vertices[GameGrid.GetPieceLocationInArray(i,k)] = new VertexNode(i, k);
                }
            }
        }
        protected override void InitEdges()
        {
            Edges = new EdgeLink[72];
            // Initialize the edegs on the board
            for (int i = 2; i < 24; i += 2)
            {
                for (int k = 0; k < GameGrid.GetAmountOfColumns(i) - 1; k++)
                {
                    Edges[GameGrid.GetPieceLocationInArray(i, k)] = new EdgeLink(i, k);
                }
            }
        }
        protected override void SetEdgesVertices()
        {
            // Set the Vertices for each edge
            for (int i = 2; i < 24; i += 2)
            {
                for (int k = 0; k < GameGrid.GetAmountOfColumns(i) - 1; k++)
                {
                    if (i == 2 || i == 6 || i == 10)
                    {
                        if (k % 2 == 0)
                        {
                            Edges[GameGrid.GetPieceLocationInArray(i, k)].VertexNodeOne = Vertices[GameGrid.GetPieceLocationInArray(i - 1, k / 2)];
                            Edges[GameGrid.GetPieceLocationInArray(i, k)].VertexNodeTwo = Vertices[GameGrid.GetPieceLocationInArray(i + 1, k / 2)];

                        }
                        else
                        {
                            Edges[GameGrid.GetPieceLocationInArray(i, k)].VertexNodeOne = Vertices[GameGrid.GetPieceLocationInArray(i - 1, k / 2)];
                            Edges[GameGrid.GetPieceLocationInArray(i, k)].VertexNodeTwo = Vertices[GameGrid.GetPieceLocationInArray(i + 1, k / 2 + 1)];
                        }
                    }
                    else if (i == 14 || i == 18 || i == 22)
                    {
                        if (k % 2 == 0)
                        {
                            Edges[GameGrid.GetPieceLocationInArray(i, k)].VertexNodeOne = Vertices[GameGrid.GetPieceLocationInArray(i - 1, GameGrid.GetAmountOfColumns(i - 1) - (k / 2) - 2)];
                            Edges[GameGrid.GetPieceLocationInArray(i, k)].VertexNodeTwo = Vertices[GameGrid.GetPieceLocationInArray(i + 1, GameGrid.GetAmountOfColumns(i + 1) - (k / 2) - 2)];
                        }
                        else
                        {
                            Edges[GameGrid.GetPieceLocationInArray(i, k)].VertexNodeTwo = Vertices[GameGrid.GetPieceLocationInArray(i - 1, GameGrid.GetAmountOfColumns(i - 1) - (k / 2) - 3)];
                            Edges[GameGrid.GetPieceLocationInArray(i, k)].VertexNodeOne = Vertices[GameGrid.GetPieceLocationInArray(i + 1, GameGrid.GetAmountOfColumns(i + 1) - (k / 2) - 2)];
                        }
                    }
                    else if (i == 4 || i == 8 || i == 12 || i == 16 || i == 20)
                    {
                        if (i > 12)
                        {
                            Edges[GameGrid.GetPieceLocationInArray(i, k)].VertexNodeTwo = Vertices[GameGrid.GetPieceLocationInArray(i - 1, GameGrid.GetAmountOfColumns(i - 1) - k - 2)];
                            Edges[GameGrid.GetPieceLocationInArray(i, k)].VertexNodeOne = Vertices[GameGrid.GetPieceLocationInArray(i + 1, GameGrid.GetAmountOfColumns(i + 1) - k - 2)];
                        }
                        else
                        {
                            Edges[GameGrid.GetPieceLocationInArray(i, k)].VertexNodeTwo = Vertices[GameGrid.GetPieceLocationInArray(i - 1, k)];
                            Edges[GameGrid.GetPieceLocationInArray(i, k)].VertexNodeOne = Vertices[GameGrid.GetPieceLocationInArray(i + 1, k)];
                        }
                    }
                }
            }
        }
        protected override void SetVerticesEdegs()
        {
            // Set the edges for each vertex node
            for (int i = 1; i < 24; i += 2)
            {
                for (int k = 0; k < GameGrid.GetAmountOfColumns(i) - 1; k++)
                {
                    if (i == 1)
                    {
                        Vertices[GameGrid.GetPieceLocationInArray(i, k)].Edges = new EdgeLink[2];
                        Vertices[GameGrid.GetPieceLocationInArray(i, k)].Edges[0] = Edges[GameGrid.GetPieceLocationInArray(i + 1, k * 2)];
                        Vertices[GameGrid.GetPieceLocationInArray(i, k)].Edges[1] = Edges[GameGrid.GetPieceLocationInArray(i + 1, k * 2 + 1)];
                    }
                    else if (i == 3 || i == 7 || i == 11 || i == 15 || i == 19)
                    {
                        if (i > 12)
                        {
                            Vertices[GameGrid.GetPieceLocationInArray(i, k)].Edges = new EdgeLink[3];
                            Vertices[GameGrid.GetPieceLocationInArray(i, k)].Edges[0] = Edges[GameGrid.GetPieceLocationInArray(i + 1, GameGrid.GetAmountOfColumns(i + 1) - 2 - k)];
                            Vertices[GameGrid.GetPieceLocationInArray(i, k)].Edges[1] = Edges[GameGrid.GetPieceLocationInArray(i - 1, GameGrid.GetAmountOfColumns(i - 1) - (k * 2) - 3)];
                            Vertices[GameGrid.GetPieceLocationInArray(i, k)].Edges[2] = Edges[GameGrid.GetPieceLocationInArray(i - 1, GameGrid.GetAmountOfColumns(i - 1) - (k * 2) - 2)];
                        }
                        else
                        {
                            if (k != GameGrid.GetAmountOfColumns(i) - 2 && k != 0)
                                Vertices[GameGrid.GetPieceLocationInArray(i, k)].Edges = new EdgeLink[3];
                            else
                                Vertices[GameGrid.GetPieceLocationInArray(i, k)].Edges = new EdgeLink[2];
                            Vertices[GameGrid.GetPieceLocationInArray(i, k)].Edges[0] = Edges[GameGrid.GetPieceLocationInArray(i + 1, k)];
                            if (k != GameGrid.GetAmountOfColumns(i) - 2)
                                Vertices[GameGrid.GetPieceLocationInArray(i, k)].Edges[1] = Edges[GameGrid.GetPieceLocationInArray(i - 1, k * 2)];
                            if (k != 0)
                            {
                                if (k != GameGrid.GetAmountOfColumns(i) - 2)
                                    Vertices[GameGrid.GetPieceLocationInArray(i, k)].Edges[2] = Edges[GameGrid.GetPieceLocationInArray(i - 1, (k * 2) - 1)];
                                else
                                    Vertices[GameGrid.GetPieceLocationInArray(i, k)].Edges[1] = Edges[GameGrid.GetPieceLocationInArray(i - 1, (k * 2) - 1)];
                            }
                        }
                    }
                    else if (i == 5 || i == 9 || i == 13 || i == 17 || i == 21)
                    {
                        if (i < 12)
                        {
                            Vertices[GameGrid.GetPieceLocationInArray(i, k)].Edges = new EdgeLink[3];
                            Vertices[GameGrid.GetPieceLocationInArray(i, k)].Edges[0] = Edges[GameGrid.GetPieceLocationInArray(i - 1, k)];
                            Vertices[GameGrid.GetPieceLocationInArray(i, k)].Edges[1] = Edges[GameGrid.GetPieceLocationInArray(i + 1, (k * 2) + 1)];
                            Vertices[GameGrid.GetPieceLocationInArray(i, k)].Edges[2] = Edges[GameGrid.GetPieceLocationInArray(i + 1, k * 2)];
                        }
                        else
                        {
                            if (k < GameGrid.GetAmountOfColumns(i) - 2 && k > 0)
                                Vertices[GameGrid.GetPieceLocationInArray(i, k)].Edges = new EdgeLink[3];
                            else
                                Vertices[GameGrid.GetPieceLocationInArray(i, k)].Edges = new EdgeLink[2];
                            if (i == 13)
                                Vertices[GameGrid.GetPieceLocationInArray(i, k)].Edges[0] = Edges[GameGrid.GetPieceLocationInArray(i - 1, k)];
                            else
                                Vertices[GameGrid.GetPieceLocationInArray(i, k)].Edges[0] = Edges[GameGrid.GetPieceLocationInArray(i - 1, GameGrid.GetAmountOfColumns(i - 1) - 2 - k)];
                            if (k < GameGrid.GetAmountOfColumns(i) - 2)
                                Vertices[GameGrid.GetPieceLocationInArray(i, k)].Edges[1] = Edges[GameGrid.GetPieceLocationInArray(i + 1, GameGrid.GetAmountOfColumns(i + 1) - (k * 2) - 2)];
                            if (k > 0)
                            {
                                if (k < GameGrid.GetAmountOfColumns(i) - 2)
                                    Vertices[GameGrid.GetPieceLocationInArray(i, k)].Edges[2] = Edges[GameGrid.GetPieceLocationInArray(i + 1, GameGrid.GetAmountOfColumns(i + 1) - (k * 2) - 1)];
                                else
                                    Vertices[GameGrid.GetPieceLocationInArray(i, k)].Edges[1] = Edges[GameGrid.GetPieceLocationInArray(i + 1, GameGrid.GetAmountOfColumns(i + 1) - (k * 2) - 1)];
                            }
                        }
                    }
                    else if (i == 23)
                    {
                        Vertices[GameGrid.GetPieceLocationInArray(i, k)].Edges = new EdgeLink[2];
                        Vertices[GameGrid.GetPieceLocationInArray(i, k)].Edges[0] = Edges[GameGrid.GetPieceLocationInArray(i - 1, GameGrid.GetAmountOfColumns(i - 1) - (k * 2) - 2)];
                        Vertices[GameGrid.GetPieceLocationInArray(i, k)].Edges[1] = Edges[GameGrid.GetPieceLocationInArray(i - 1, GameGrid.GetAmountOfColumns(i - 1) - (k * 2) - 3)];
                    }
                }
            }
        }
        protected override void SetHexesVertices()
        {
            //Set the vertices for each hex tile
            for(int i = 1; i < 6; i++)
            {
                for (int k = 0; k < GameGrid.GetAmountOfColumnsTiles(i); k++)
                {
                    Hexes[GameGrid.GetTileLocationInArray(i, k + 1)].Corners[1] = Vertices[GameGrid.GetPieceLocationInArray((i * 4) - 1, k)];
                    Hexes[GameGrid.GetTileLocationInArray(i, k + 1)].Corners[2] = Vertices[GameGrid.GetPieceLocationInArray((i * 4) - 1, k + 1)];
                    Hexes[GameGrid.GetTileLocationInArray(i, k + 1)].Corners[3] = Vertices[GameGrid.GetPieceLocationInArray((i * 4) + 1, k)];
                    Hexes[GameGrid.GetTileLocationInArray(i, k + 1)].Corners[4] = Vertices[GameGrid.GetPieceLocationInArray((i * 4) + 1, k + 1)];
                    if (i < 3)
                    {
                        Hexes[GameGrid.GetTileLocationInArray(i, k + 1)].Corners[0] = Vertices[GameGrid.GetPieceLocationInArray((i * 4) - 3, k)];
                        Hexes[GameGrid.GetTileLocationInArray(i, k + 1)].Corners[5] = Vertices[GameGrid.GetPieceLocationInArray((i * 4) + 3, k + 1)];
                    }
                    else if(i == 3)
                    {
                        Hexes[GameGrid.GetTileLocationInArray(i, k + 1)].Corners[0] = Vertices[GameGrid.GetPieceLocationInArray((i * 4) - 3, k)];
                        Hexes[GameGrid.GetTileLocationInArray(i, k + 1)].Corners[5] = Vertices[GameGrid.GetPieceLocationInArray((i * 4) + 3, k)];
                    }
                    else
                    {
                        Hexes[GameGrid.GetTileLocationInArray(i, k + 1)].Corners[0] = Vertices[GameGrid.GetPieceLocationInArray((i * 4) - 3, k + 1)];
                        Hexes[GameGrid.GetTileLocationInArray(i, k + 1)].Corners[5] = Vertices[GameGrid.GetPieceLocationInArray((i * 4) + 3, k )];
                    }
                }
            }
        }

        public override void InitBoard(IndexedButton[][] Pieces, string[] tileTypes, string[] tileNumbers)
        {
            InitHex(tileTypes, tileNumbers);
            InitVertices();
            InitEdges();
            SetEdgesVertices();
            SetVerticesEdegs();
            SetHexesVertices();
        }
    }
}