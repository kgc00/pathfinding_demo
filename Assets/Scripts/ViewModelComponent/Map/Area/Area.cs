using System.Linq;
using UnityEngine;

public class Area : MonoBehaviour, IEventHandler {
    public Board Board { get; private set; }
    private AreaState state;
    public AreaState State { get => state; }
    public Point Location { get; private set; }

    [SerializeField] public AreaStateData AreaData;
    public void Initialize (AreaStateData ad, Point location) {
        AreaData = ad;
        Location = location;
        GameObject boardGO = new GameObject ("Board");
        boardGO.transform.parent = transform;
        Board = boardGO.AddComponent<Board> ();
        Board.Initialize (AreaData.currentInstance, this);
        state = new SetupState (this);
        this.state.Enter ();
    }

    void Update () {
        AreaState state = this.state?.HandleUpdate ();
        if (state == null)
            return;

        this.state = state;
        this.state.Enter ();
    }

    public void HandleIncomingEvent (InfoEventArgs curEvent) {
        switch (curEvent.type.eventType) {
            case EventTypes.StateChangeEvent:
                if (state is SetupState) {
                    SetupState curState = (SetupState) state;
                    curState.AdvanceAreaState ();
                }
                break;
            default:
                break;
        }
    }

    public void UpdateBossDoor () {
        var tileData = Board.Tiles.FirstOrDefault (tile => tile.Value.TypeReference == TileTypes.BOSS_ENTRANCE);

        if (tileData.Value == null) return;

        var bossDoor = (BossRoomEntrance) tileData.Value;

        bossDoor.SetLockedStatus (
            WorldProgressionComponent.CheckDoorUnlockRequirements (
                bossDoor, this
            )
        );
    }

    public void OnDestroy () {
        this.state.HandleTransition ();
    }
}