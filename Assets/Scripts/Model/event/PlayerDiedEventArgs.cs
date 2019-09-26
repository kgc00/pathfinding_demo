using System;
public class PlayerDiedEventArgs : InfoEventArgs {
    public PlayerDiedEventArgs (object sender, Action e) : base (sender, e) {
        type = new InfoEventData (EventTypes.PlayerDied, HandlerType.World);
    }
}