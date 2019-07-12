using UnityEngine;

public class Area : MonoBehaviour {
    public Board Board { get; private set; }
    private AreaState state;
    [SerializeField] public AreaStateData areaData;
    public EventQueue<EventInfo<Unit>> eventQueue;
    public void Initialize (AreaStateData ad) {
        areaData = ad;
        eventQueue = new EventQueue<EventInfo<Unit>> ();
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
}