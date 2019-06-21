using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Board board;
    Unit owner;
    int range;
    public void Initialize(Board board, Unit owner)
    {
        this.board = board;
        this.owner = owner;
    }
    public virtual List<Tile> GetTilesInRange(Board board)
    {
        List<Tile> retValue = board.Search(board.TileAt(owner.Position), ExpandSearch);
        Filter(retValue);
        return retValue;
    }
    protected virtual bool ExpandSearch(ShadowTile from, Tile to)
    {
        return (from.distance + 1) <= range;
    }
    protected virtual void Filter(List<Tile> tiles)
    {
        // for (int i = tiles.Count - 1; i >= 0; --i)
        //     if (tiles[i].content != null)
        //         tiles.RemoveAt(i);
    }
}
