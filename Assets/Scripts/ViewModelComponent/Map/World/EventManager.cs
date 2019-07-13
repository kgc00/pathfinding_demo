using System;

public class EventManager {
    World world;
    Area area;
    public EventManager (World world, Area area) {
        this.world = world;
        this.area = area;
    }

    public void UpdateArea (Area area) {
        this.area = area;
    }

    public void HandleUpdate () {
        var curEvent = EventQueue.HandleEvent ();
        if (curEvent != null)
            DelegateEvent (curEvent);
    }

    private void DelegateEvent (InfoEventArgs curEvent) {
        // UnityEngine.Debug.Log (string.Format ("event from {2} of type {0} for handler type {1} ", curEvent.type.eventType, curEvent.type.handlerType, curEvent.sender));
        switch (curEvent.type.handlerType) {
            case HandlerType.Area:
                area.HandleIncomingEvent (curEvent);
                break;
            case HandlerType.World:
                world.HandleIncomingEvent (curEvent);
                break;
            case HandlerType.NoHandler:
                break;
        }
    }
}