using System;
public class GameClearedEvent : InfoEventArgs {
    public GameClearedEvent (object sender, Action e) : base (sender, e) {
        type = new InfoEventData (EventTypes.GameCleared, HandlerType.World);
    }
}