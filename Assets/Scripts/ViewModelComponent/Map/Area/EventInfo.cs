using System;
public class EventInfo<T> : EventArgs {
    public object sender;
    public System.Action<T> e;

    public EventInfo (object sender, System.Action<T> e) {
        this.sender = sender;
        this.e = e;
    }
}