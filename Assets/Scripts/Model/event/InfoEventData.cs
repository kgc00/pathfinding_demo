public struct InfoEventData {
    public EventTypes eventType;
    public HandlerType handlerType;
    public InfoEventData (EventTypes eventType, HandlerType handlerType) {
        this.eventType = eventType;
        this.handlerType = handlerType;
    }
}