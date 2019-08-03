using System.Collections.Generic;

public class EventQueue {
    static Queue<InfoEventArgs> events = new Queue<InfoEventArgs> ();
    public static Queue<InfoEventArgs> Events { get => events; }
    private static readonly EventQueue INSTANCE = new EventQueue ();
    public static EventQueue Instance {
        get { return INSTANCE; }
    }

    public static void AddEvent (InfoEventArgs e) {
        events.Enqueue (e);
    }

    public static InfoEventArgs HandleEvent () {
        if (events.Count > 0)
            return events.Dequeue ();
        else return default;
    }
}