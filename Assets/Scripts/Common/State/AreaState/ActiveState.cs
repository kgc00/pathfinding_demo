using System;
using System.Collections.Generic;
using System.Linq;

public class ActiveState : AreaState {
    private Area area;
    public ActiveState (Area area) { this.area = area; }

    public override void Enter () { }
    public override AreaState HandleUpdate () {
        // var info = EventQueue.HandleEvent ();
        // if (info != null && info.sender.ToString () == "ENTRANCE (Entrance)") {
        //     info.e.Invoke ();
        //     // area
        // }
        return null;
    }
}