using System.Collections.Generic;
using UnityEngine;
using static Platinio.EventManager;

namespace Platinio
{
    public enum GameEventType
    {
        Event_1,
        Event_2,
        Event_3
    }

    public static class EventManager 
    {
        public delegate void OnEvent(GameEventType eventType, params object[] args);
        private static Dictionary<GameEventType, List<EventInfo>> events = new Dictionary<GameEventType, List<EventInfo>>();


        public static void AddListener(GameEventType eventType , Component listener, OnEvent action )
        {
            List<EventInfo> eventInfoList = null;

            EventInfo info;
            info.action = action;
            info.listener = listener;

            //event already exist
            if (events.TryGetValue(eventType, out eventInfoList))
            {               
                //add new listener
                eventInfoList.Add(info);
                return;
            }

            //create new event
            eventInfoList = new List<EventInfo>();
            eventInfoList.Add(info);
            events.Add(eventType, eventInfoList);

        }

        public static void RemoveEventFromListenerAndAction(GameEventType eventType, Component listener , OnEvent action)
        {
            List<EventInfo> eventInfoList;

            if (events.TryGetValue(eventType, out eventInfoList))
            {
                for (int n = 0; n < eventInfoList.Count; n++)
                {
                    if (eventInfoList[n].listener == listener && eventInfoList[n].action == action)
                    {
                        eventInfoList.RemoveAt(n);
                        break;
                    }
                }
            }
        }

        public static void RemoveEventsFromListener(GameEventType eventType, Component listener)
        {
            List<EventInfo> eventInfoList;

            if (events.TryGetValue(eventType, out eventInfoList))
            {
                for (int n = 0; n < eventInfoList.Count; n++)
                {
                    if (eventInfoList[n].listener == listener)
                    {
                        eventInfoList.RemoveAt(n);                        
                    }
                }
            }
        }

        public static void RemoveAllFromListener(Component listener)
        {            
            foreach (KeyValuePair<GameEventType, List<EventInfo>> entry in events)
            {
                RemoveEventsFromListener(entry.Key  , listener);
            }
        }

        public static void TriggerEvent(GameEventType eventType, object args = null)
        {
            List<EventInfo> eventInfoList;

            if (events.TryGetValue(eventType, out eventInfoList))
            {
                for (int n = 0; n < eventInfoList.Count; n++)
                {
                    if (eventInfoList[n].listener != null)
                    {
                        eventInfoList[n].action(eventType, args);
                    }
                    else
                    {
                        RemoveAllFromListener( eventInfoList[n].listener );
                    }
                    
                }
               
            }

        }

    }

    public struct EventInfo
    {
        public Component listener;
        public OnEvent action;
    }
}

