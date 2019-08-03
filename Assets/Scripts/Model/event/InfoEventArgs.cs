using System;
public abstract class InfoEventArgs : EventArgs {
    public object sender;
    public System.Action e;
    public InfoEventData type;
    public InfoEventArgs (object sender, System.Action e) {
        this.sender = sender;
        this.e = e;
        type = new InfoEventData (EventTypes.BaseEvent, HandlerType.NoHandler);
    }
}