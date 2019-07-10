using UnityEngine;

public class Area : MonoBehaviour {
    public Board Board { get; private set; }
    private AreaState state;
    [SerializeField] private LevelData levelData;
    public void Initialize (LevelData ld) {
        levelData = ld;
        state = new SetupState (this);
        GameObject boardGO = new GameObject ("Board");
        boardGO.transform.parent = transform;
        Board = boardGO.AddComponent<Board> ();
        Board.Initialize (levelData);
    }

    void Update () {
        AreaState state = this.state?.HandleUpdate ();
        if (state == null)
            return;

        this.state = state;
        this.state.Enter ();
    }
}