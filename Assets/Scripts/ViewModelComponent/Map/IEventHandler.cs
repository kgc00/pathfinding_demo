using System.Collections;
using UnityEngine;
public interface IEventHandler {
    void HandleIncomingEvent (InfoEventArgs curEvent);
}