﻿using System;
using System.Diagnostics;

namespace UAlbion.Api.Eventing;

public abstract class Handler
{
    public abstract bool ShouldSubscribe { get; }
    public bool IsActive { get; set; }
    public bool IsPostHandler { get; }
    public Type Type { get; }
    public IComponent Component { get; }
    protected Handler(Type type, IComponent component, bool isPostHandler)
    {
        Type = type ?? throw new ArgumentNullException(nameof(type));
        Component = component ?? throw new ArgumentNullException(nameof(component));
        IsPostHandler = isPostHandler;
    }

    public override string ToString() => $"H<{Component.GetType().Name}, {Type.Name}>";
}

public interface IAsyncHandler { AlbionTask InvokeAsAsync(IEvent e); }
public interface IAsyncQueryHandler<T> : IAsyncHandler { AlbionTask<T> InvokeAsAsync(IQueryEvent<T> e); }

public interface ISyncHandler : IAsyncHandler { void Invoke(IEvent e); }
public interface ISyncQueryHandler<T> : IAsyncQueryHandler<T>, ISyncHandler { T Invoke(IQueryEvent<T> e); }

public class SyncHandler<TEvent> : Handler, ISyncHandler where TEvent : IEvent
{
    public override bool ShouldSubscribe => true;
    Action<TEvent> Callback { get; }
    public SyncHandler(Action<TEvent> callback, IComponent component, bool isPostHandler) 
        : base(typeof(TEvent), component, isPostHandler) => Callback = callback;
    [DebuggerHidden, StackTraceHidden] public void Invoke(IEvent e) => Callback((TEvent)e);
    [DebuggerHidden, StackTraceHidden] public AlbionTask InvokeAsAsync(IEvent e)
    {
        Callback((TEvent)e);
        return AlbionTask.CompletedTask;
    }

    public override string ToString() => $"H<{Component.GetType().Name}, {Type.Name}>";
}

public class SyncQueryHandler<TEvent, TResult> : Handler, ISyncQueryHandler<TResult> where TEvent : IQueryEvent<TResult>
{
    public override bool ShouldSubscribe => true;
    Func<TEvent, TResult> Callback { get; }
    public SyncQueryHandler(Func<TEvent, TResult> callback, IComponent component, bool isPostHandler) 
        : base(typeof(TEvent), component, isPostHandler) => Callback = callback;
    [DebuggerHidden, StackTraceHidden] void ISyncHandler.Invoke(IEvent e) => Callback((TEvent)e); // Ignores result
    [DebuggerHidden, StackTraceHidden] AlbionTask IAsyncHandler.InvokeAsAsync(IEvent e) // Ignores result
    {
        Callback((TEvent)e);
        return AlbionTask.CompletedTask;
    }

    [DebuggerHidden, StackTraceHidden] public TResult Invoke(IQueryEvent<TResult> e) => Callback((TEvent)e);
    [DebuggerHidden, StackTraceHidden] public AlbionTask<TResult> InvokeAsAsync(IQueryEvent<TResult> e)
    {
        var result = Callback((TEvent)e);
        return AlbionTask.FromResult(result);
    }

    public override string ToString() => $"HQ<{Component.GetType().Name}, {Type.Name}>";
}

public class ReceiveOnlyHandler<TEvent> : Handler, ISyncHandler where TEvent : IEvent
{
    public override bool ShouldSubscribe => false;
    Action<TEvent> Callback { get; }
    public ReceiveOnlyHandler(Action<TEvent> callback, IComponent component) 
        : base(typeof(TEvent), component, false) => Callback = callback;
    [DebuggerHidden, StackTraceHidden] public void Invoke(IEvent e) => Callback((TEvent)e);
    [DebuggerHidden, StackTraceHidden] public AlbionTask InvokeAsAsync(IEvent e)
    {
        Callback((TEvent)e);
        return AlbionTask.CompletedTask;
    }

    public override string ToString() => $"HR<{Component.GetType().Name}, {Type.Name}>";
}

public class AsyncHandler<TEvent> : Handler, IAsyncHandler where TEvent : IEvent
{
    public override bool ShouldSubscribe => true;
    Func<TEvent, AlbionTask> Callback { get; }
    public AsyncHandler(Func<TEvent, AlbionTask> callback, IComponent component, bool isPostHandler)
        : base(typeof(TEvent), component, isPostHandler) => Callback = callback;
    [DebuggerHidden, StackTraceHidden] public AlbionTask InvokeAsAsync(IEvent e) => Callback((TEvent)e);
    public override string ToString() => $"HA<{Component.GetType().Name}, {Type.Name}>";
}

public class AsyncQueryHandler<TEvent, TResult> : Handler, IAsyncQueryHandler<TResult> where TEvent : IQueryEvent<TResult>
{
    public override bool ShouldSubscribe => true;
    Func<TEvent, AlbionTask<TResult>> Callback { get; }
    public AsyncQueryHandler(Func<TEvent, AlbionTask<TResult>> callback, IComponent component, bool isPostHandler)
        : base(typeof(TEvent), component, isPostHandler) => Callback = callback;
    [DebuggerHidden, StackTraceHidden] public AlbionTask<TResult> InvokeAsAsync(IQueryEvent<TResult> e) => Callback((TEvent)e);
    [DebuggerHidden, StackTraceHidden] public AlbionTask InvokeAsAsync(IEvent e) => Callback((TEvent)e).AsUntyped;

    public override string ToString() => $"HQA<{Component.GetType().Name}, {Type.Name}>";
}

