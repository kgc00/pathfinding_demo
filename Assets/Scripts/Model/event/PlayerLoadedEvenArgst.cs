using System;
public class PlayerLoadedEventArgs : InfoEventArgs {
    public PlayerLoadedEventArgs (object sender, Action e) : base (sender, e) {
        type = new InfoEventData (EventTypes.PlayerLoaded, HandlerType.World);
    }
}