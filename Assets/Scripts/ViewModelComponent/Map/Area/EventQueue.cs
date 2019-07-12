using System.Collections.Generic;

public class EventQueue<Unit> {
    Queue<EventInfo<global::Unit>> events = new Queue<EventInfo<global::Unit>> ();
    public Queue<EventInfo<global::Unit>> Events { get => events; }
    public EventQueue () { }

    public void AddEvent (EventInfo<global::Unit> e) {
        events.Enqueue (e);
        UnityEngine.Debug.Log (string.Format ("Added {0} to queue from {1} with length of {2}", e.e, e.sender, events.Count));
    }

    public EventInfo<global::Unit> HandleEvent () {
        if (events.Count > 0)
            return events.Dequeue ();
        else return default;
    }
}