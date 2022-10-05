using System;
using Chess;
using Chess.Chessboard;
using Chess.Error;
using Chess.Error.Levels;

namespace Chess.Rules
{
    public class Move
    {
        public ErrorHandler ErrorHandler;
        public Tile fromTile;
        public Tile toTile;
        public int fromRank;
        public int fromFile;
        public int toRank;
        public int toFile;

        public Move(ErrorHandler ErrorHandler, Tile fromTile, Tile toTile) 
        {
            this.ErrorHandler = ErrorHandler;
            this.fromTile = fromTile;
            this.toTile = toTile;
            /* this.fromRank = fromRank; */
            /* this.fromFile = fromFile; */
            /* this.toRank = toRank; */
            /* this.toFile = toFile; */
            this.fromRank = fromTile.rank;
            this.fromFile = fromTile.file;
            this.toRank = toTile.rank;
            this.toFile = toTile.file;
        }

        public List<Tuple<int, int>> GetTileIndexesBetweenInputs()
        {
            var list = new List<Tuple<int, int>>();
            string moveInfo = "";
            // Vertical movement
            if (this.fromFile == this.toFile)
            {
                moveInfo += "Vertical move, tiles visited: ";
                int i = this.fromRank;
                while(i != this.toRank)
                {
                    i = (this.fromRank > this.toRank) ? i - 1 : i + 1;
                    moveInfo += "[" + this.fromRank + " " + i + "] ";
                    list.Add(new Tuple<int, int>(i, this.fromFile));
                }
            }
            // Horizontal movement
            else if (this.fromRank == this.toRank)
            {
                moveInfo += "Horizontal move, tiles visited: ";
                int i = this.fromFile;
                while(i != this.toFile)
                {
                    i = (this.fromFile > this.toFile) ? i - 1 : i + 1;
                    moveInfo += "[" + this.fromFile + " " + i + "] ";
                    list.Add(new Tuple<int, int>(this.fromRank, i));
                }
            }
            // Diagonal movement
            else
            {
                moveInfo += "Diagonal move, tiles visited: ";
                int i = this.fromRank;
                int ii = this.fromFile;
                while(i != this.toRank && ii != this.toFile)
                {
                    i = (this.fromRank > this.toRank) ? i - 1 : i + 1;
                    ii = (this.fromFile > this.toFile) ? ii - 1 : ii + 1;
                    moveInfo += "[" + ii + " " + i + "] ";
                    list.Add(new Tuple<int, int>(i, ii));
                }
            }

            this.ErrorHandler.New(moveInfo, Level.Debug);

            return list;
        }

        public bool IsPerpendicular()
        {
            return (fromFile == toFile || fromRank == toRank);
        }

        public bool IsDiagonal()
        {
            return (fromFile - fromRank == toFile - toRank || fromFile + fromRank == toFile + toRank);
        }

        public bool IsBlocked(List<Tile> list)
        {
            bool response = false;
            Tile targetTile = list[list.Count - 1];

            // Check if both tiles have pieces on them
            if (targetTile.Occupied())
            {
                // Compared if existing pieces are of different color
                if (this.fromTile.piece.color != this.targetTile.piece.color) return false;
            }

            // Loop over all tiles in between a move checking if they are occupied
            foreach (Tile tile in list)
            {
                // If piece is not null but type of Piece, we return true
                if (tile.Occupied()) response = true;
            }
            return response;
        }
    }
}
