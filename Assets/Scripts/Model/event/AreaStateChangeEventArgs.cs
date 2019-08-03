using System;
public class AreaStateChangeEventArgs : InfoEventArgs {
    public AreaStateTypes newState;
    public AreaStateChangeEventArgs (object sender, Action e, AreaStateTypes newState) : base (sender, e) {
        this.newState = newState;
        type = new InfoEventData (EventTypes.StateChangeEvent, HandlerType.Area);
    }
}