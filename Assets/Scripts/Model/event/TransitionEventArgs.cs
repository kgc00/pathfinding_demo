using System;
public class TransitionEventArgs : InfoEventArgs {
    public Point transitionDirection;
    public TransitionEventArgs (object sender, Action e, Point transitionDirection) : base (sender, e) {
        this.transitionDirection = transitionDirection;
        type = new InfoEventData (EventTypes.TransitionEvent, HandlerType.World);
    }
}