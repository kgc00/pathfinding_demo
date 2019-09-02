using System;
public class AreaClearedEvent : InfoEventArgs {
    public AreaClearedEvent (object sender, Action e) : base (sender, e) {
        type = new InfoEventData (EventTypes.AreaCleared, HandlerType.World);
    }
}