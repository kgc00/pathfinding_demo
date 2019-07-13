using System;
using UnityEngine;

public class Area : MonoBehaviour, IEventHandler {
    public Board Board { get; private set; }
    private AreaState state;
    public AreaState State { get => state; }

    [SerializeField] public AreaStateData areaData;
    public void Initialize (AreaStateData ad) {
        areaData = ad;
        GameObject boardGO = new GameObject ("Board");
        boardGO.transform.parent = transform;
        Board = boardGO.AddComponent<Board> ();
        Board.Initialize (areaData.currentInstance, this);
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
}