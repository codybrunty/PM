﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PropogateDragVertical : MonoBehaviour {

    public UnityEngine.UI.ScrollRect scrollViewVertical;

    void Start() {
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entryBegin = new EventTrigger.Entry(), entryDrag = new EventTrigger.Entry(), entryEnd = new EventTrigger.Entry(), entrypotential = new EventTrigger.Entry()
            , entryScroll = new EventTrigger.Entry();

        entryBegin.eventID = EventTriggerType.BeginDrag;
        entryBegin.callback.AddListener((data) => { scrollViewVertical.OnBeginDrag((PointerEventData)data); });
        trigger.triggers.Add(entryBegin);

        entryDrag.eventID = EventTriggerType.Drag;
        entryDrag.callback.AddListener((data) => { scrollViewVertical.OnDrag((PointerEventData)data); });
        trigger.triggers.Add(entryDrag);

        entryEnd.eventID = EventTriggerType.EndDrag;
        entryEnd.callback.AddListener((data) => { scrollViewVertical.OnEndDrag((PointerEventData)data); });
        trigger.triggers.Add(entryEnd);

        entrypotential.eventID = EventTriggerType.InitializePotentialDrag;
        entrypotential.callback.AddListener((data) => { scrollViewVertical.OnInitializePotentialDrag((PointerEventData)data); });
        trigger.triggers.Add(entrypotential);

        entryScroll.eventID = EventTriggerType.Scroll;
        entryScroll.callback.AddListener((data) => { scrollViewVertical.OnScroll((PointerEventData)data); });
        trigger.triggers.Add(entryScroll);
    }

    public void ProprogateAgain() {
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entryBegin = new EventTrigger.Entry(), entryDrag = new EventTrigger.Entry(), entryEnd = new EventTrigger.Entry(), entrypotential = new EventTrigger.Entry()
            , entryScroll = new EventTrigger.Entry();

        entryBegin.eventID = EventTriggerType.BeginDrag;
        entryBegin.callback.AddListener((data) => { scrollViewVertical.OnBeginDrag((PointerEventData)data); });
        trigger.triggers.Add(entryBegin);

        entryDrag.eventID = EventTriggerType.Drag;
        entryDrag.callback.AddListener((data) => { scrollViewVertical.OnDrag((PointerEventData)data); });
        trigger.triggers.Add(entryDrag);

        entryEnd.eventID = EventTriggerType.EndDrag;
        entryEnd.callback.AddListener((data) => { scrollViewVertical.OnEndDrag((PointerEventData)data); });
        trigger.triggers.Add(entryEnd);

        entrypotential.eventID = EventTriggerType.InitializePotentialDrag;
        entrypotential.callback.AddListener((data) => { scrollViewVertical.OnInitializePotentialDrag((PointerEventData)data); });
        trigger.triggers.Add(entrypotential);

        entryScroll.eventID = EventTriggerType.Scroll;
        entryScroll.callback.AddListener((data) => { scrollViewVertical.OnScroll((PointerEventData)data); });
        trigger.triggers.Add(entryScroll);
    }

}
