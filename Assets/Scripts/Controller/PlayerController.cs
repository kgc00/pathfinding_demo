using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller {
    Coroutine acting;
    Coroutine cooldown;
    List<PathfindingData> tilesInRange;
    PathfindingData tileToMoveTo;
    public override void Initialize (Board board, Unit owner, WalkingMovement movement) {
        base.Initialize (board, owner, movement);
    }

    void Update () {

        switch (owner.State) {
            case UnitStates.IDLE:

                IdleState ();
                break;
            case UnitStates.PREPARING:
                PrepState ();
                break;
            case UnitStates.ACTING:
                ActingState ();
                break;
            case UnitStates.COOLDOWN:
                CooldownState ();
                break;
            default:
                break;
        }
    }

    public override void IdleState () {
        if (Input.GetMouseButtonDown (0)) {
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast (ray, out hit, 50, 1 << 11)) {
                Unit selectedUnit = hit.transform.GetComponent<Unit> ();
                if (selectedUnit.State == UnitStates.IDLE) {
                    Debug.Log ("selected unit");
                    owner.SetState (UnitStates.PREPARING);
                }
            }
        }
    }

    public override void PrepState () {
        List<PathfindingData> data = movement.GetTilesInRange (board);
        List<Tile> tiles = new List<Tile> ();
        data.ForEach (element => {
            tiles.Add (element.tile);
        });
        tilesInRange = data;
        BoardVisuals.AddTileToHighlights (owner, tiles);
        if (Input.GetMouseButtonDown (1)) {
            Debug.Log ("Clicked");
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast (ray, out hit, 50, 1 << 10)) {
                Tile selectedTile = hit.transform.GetComponent<Tile> ();
                if (selectedTile.isWalkable) {
                    PathfindingData temp = tilesInRange.Find (element => element.tile == selectedTile);
                    if (temp.tile != null) {
                        Debug.Log ("selected tile");
                        tileToMoveTo = temp;
                        owner.SetState (UnitStates.ACTING);
                    }

                }
            }
        }
    }

    public override void ActingState () {
        if (acting == null) {
            acting = StartCoroutine (movement.Traverse (tilesInRange, tileToMoveTo, () => {
                owner.SetState (UnitStates.COOLDOWN);
                acting = null;
            }));
            tileToMoveTo.tile = null;
            tileToMoveTo.shadow = null;
            tilesInRange = null;
        }
    }

    public override void CooldownState () {
        if (cooldown == null) {
            BoardVisuals.RemoveTileFromHighlights (owner);
            cooldown = StartCoroutine (countdown (Random.Range (0, 4),
                () => {
                    owner.SetState (UnitStates.IDLE);
                    cooldown = null;
                }));
        }
    }

    private IEnumerator countdown (float timeToWait, System.Action onComplete) {
        int safety = 0;
        while (timeToWait > 0) {
            timeToWait -= Time.deltaTime;
            yield return null;
            if (safety > 999999) {
                Debug.Log ("Break");
                break;
            }
        }
        onComplete ();
    }
}