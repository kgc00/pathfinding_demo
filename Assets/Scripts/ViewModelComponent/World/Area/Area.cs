using UnityEngine;

public class Area : MonoBehaviour {
    private Board board;
    private AreaState state;
    [SerializeField] private LevelData levelData;
    public void Initialize (LevelData ld) {
        Debug.Log ("initializing");
        levelData = ld;
        state = new SetupState (this);
        GameObject boardGO = new GameObject ("Board");
        boardGO.transform.parent = transform;
        board = boardGO.AddComponent<Board> ();
        board.Initialize (levelData);
    }

    void Update () {
        AreaState state = this.state?.HandleUpdate ();
        if (state == null)
            return;

        this.state = state;
        this.state.Enter ();
    }
}