using UnityEngine;

[System.Serializable]
public class Entrance : Tile {
    public override void SetOccupied (Unit occupier) {
        base.SetOccupied (occupier);
        Board.RelayEventToArea (new EventInfo<Unit> (this, OnOccupied));
    }
    protected override void OnOccupied (Unit occupier) {
        Debug.Log (string.Format ("occupado"));
    }
}