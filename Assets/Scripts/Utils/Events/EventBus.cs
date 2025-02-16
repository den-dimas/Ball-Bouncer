using System;
using System.Collections.Generic;

public static class EventBus
{
  private static readonly Dictionary<Type, Delegate> events = new();

  public static void Publish<T>(T data)
  {
    Type type = data.GetType();

    if (events.TryGetValue(type, out Delegate exist))
    {
      exist?.DynamicInvoke(data);
    }
  }

  public static void Subscribe<T>(Action<T> action) where T : EventData
  {
    Type type = typeof(T);

    if (events.ContainsKey(type))
    {
      events[type] = Delegate.Combine(events[type], action);
    }
    else
    {
      events[type] = action;
    }
  }

  public static void Unsubscribe<T>(Action<T> action) where T : EventData
  {
    Type type = typeof(T);

    if (events.ContainsKey(type))
    {
      events[type] = Delegate.Remove(events[type], action);
    }
  }
}