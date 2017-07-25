// ===========
// DO NOT EDIT - this file is automatically regenerated.
// =========== 

namespace Improbable
{

/// <summary>
/// MetadataWriter is deprecated and will be removed in a future SpatialOS version. Please migrate to the simplified
/// <c>Metadata.Writer</c> interface (but see its documentation for semantic differences).
/// </summary>
[global::Improbable.Entity.Component.WriterInterface]
[global::Improbable.Entity.Component.ComponentId(53)]
[global::System.Obsolete("Please use Metadata.Writer (but see its documentation for semantic differences).")]
public interface MetadataWriter : MetadataReader
{
  Metadata.Updater Update { get; }
}

/// <summary>
/// MetadataReader is deprecated and will be removed in a future SpatialOS version. Please migrate to the simplified
/// <c>Metadata.Reader</c> interface (but see its documentation for semantic differences).
/// </summary>
[global::Improbable.Entity.Component.ReaderInterface]
[global::Improbable.Entity.Component.ComponentId(53)]
[global::System.Obsolete("Please use Metadata.Reader (but see its documentation for semantic differences).")]
public interface MetadataReader
{
  bool IsAuthoritativeHere { get; }
  event global::System.Action PropertyUpdated;
  event global::System.Action<bool> AuthorityChanged;
  // Field entityType = 1.
  string EntityType { get; }
  event global::System.Action<string> EntityTypeUpdated;
}

public partial class Metadata : global::Improbable.Entity.Component.IComponentFactory
{
  [global::Improbable.Entity.Component.WriterInterface]
  [global::Improbable.Entity.Component.ComponentId(53)]
  public interface Writer : Reader, global::Improbable.Entity.Component.IComponentWriter
  {
    /// <summary>
    /// Sends a component update.
    /// </summary>
    /// <remarks>
    /// Unlike the deprecated <c>MetadataWriter</c> interface, changes made by the update are not
    /// applied to the local copy of the component (returned by the <c>Data</c> property) until the
    /// update is processed by the connection. Therefore, the <c>ComponentUpdated</c> event is not
    /// triggered immediately, but queued to be triggered at a later time. Additionally, the
    /// update will be sent even if it would have no effect on the current local copy of the
    /// component data; discarding of no-op updates should be done manually. The behaviour is
    /// undefined if the update is mutated after it is sent; use <c>Send(update.DeepCopy())</c> if
    /// you intend to hold on to the update and modify it later. <seealso cref="CopyAndSend"/>
    /// </remarks>
    void Send(Update update);
    /// <summary>
    /// Performs a deep copy and sends the copy as a component update.
    /// </summary>
    /// <remarks>
    /// Works exactly like <c>Send(update)</c>, but performs a deep copy of the update before sending
    /// it. Use this method if you intend to hold on to the update and modify it later. <see cref="Send"/>
    /// </remarks>
    void CopyAndSend(Update update);
    /// <summary>
    /// Returns the CommandReceiver for this component.
    /// </summary>
    ICommandReceiver CommandReceiver { get; }
  }

  [global::Improbable.Entity.Component.ReaderInterface]
  [global::Improbable.Entity.Component.ComponentId(53)]
  public interface Reader
  {
    /// <summary>
    /// Whether the local worker currently has authority over this component.
    /// </summary>
    bool HasAuthority { get; }
    /// <summary>
    /// Retrieves the local copy of the component data.
    /// </summary>
    global::Improbable.MetadataData Data { get; }

    /// <summary>
    /// Triggered whenever an update is received for this component.
    /// </summary>
    global::Improbable.Entity.Component.EventCallbackHandler<Update> ComponentUpdated { get; set; }
    /// <summary>
    /// Triggered whenever the local worker's authority over this component changes.
    /// </summary>
    global::Improbable.Entity.Component.EventCallbackHandler<bool> AuthorityChanged { get; set; }
    /// <summary>
    /// Register callbacks here to be invoked whenever entityType field value is changed.
    /// </summary>
    global::Improbable.Entity.Component.PropertyCallbackHandler<string> EntityTypeUpdated { get; set; }
  }

  /// <summary>
  /// Metadata.Updater is deprecated and will be removed in a future SpatialOS version.
  /// Please use Metadata.Writer.Send() instead (see the documentation for semantic differences).
  /// </summary>
  [global::System.Obsolete("Please use Metadata.Writer.Send() instead (see the documentation for semantic differences).")]
  public interface Updater
  {
    void FinishAndSend();
    // Field entityType = 1.
    #pragma warning disable 0618
    Updater EntityType(string newValue);
    #pragma warning restore 0618
  }

  public interface ICommandReceiver
  {
  }

  public partial class Commands
  {

  }
  // Implementation details below here.
  //-----------------------------------

  private readonly global::System.Collections.Generic.Dictionary<global::Improbable.EntityId, Impl> implMap =
      new global::System.Collections.Generic.Dictionary<global::Improbable.EntityId, Impl>();
  private bool loggedInvalidAddComponent = false;
  private bool loggedInvalidRemoveComponent = false;

  public void UnregisterWithConnection(global::Improbable.Worker.Connection connection, global::Improbable.Worker.Dispatcher dispatcher) {
    loggedInvalidAddComponent = false;
    loggedInvalidRemoveComponent = false;
    implMap.Clear();
  }

  public void RegisterWithConnection(global::Improbable.Worker.Connection connection, global::Improbable.Worker.Dispatcher dispatcher,
                                     global::Improbable.Entity.Component.ComponentFactoryCallbacks callbacks)
  {
    dispatcher.OnAddComponent<Metadata>((op) => {
      if (!implMap.ContainsKey(op.EntityId))
      {
        var impl = new Impl(connection, op.EntityId, op.Data.Get());
        implMap.Add(op.EntityId, impl);
        if (callbacks.OnComponentAdded != null)
        {
          callbacks.OnComponentAdded(op.EntityId, this, impl);
        }
      }
      else if (!loggedInvalidAddComponent)
      {
        global::Improbable.Worker.ClientError.LogClientException(new global::System.InvalidOperationException(
            "Component Metadata added to entity " + op.EntityId + ", but it already exists." +
            "This error will be reported once only for each component."));
        loggedInvalidAddComponent = true;
      }
    });

    dispatcher.OnRemoveComponent<Metadata>((op) => {
      Impl impl;
      if (implMap.TryGetValue(op.EntityId, out impl))
      {
        if (callbacks.OnComponentRemoved != null)
        {
          callbacks.OnComponentRemoved(op.EntityId, this, impl);
        }
        implMap.Remove(op.EntityId);
      }
      else if (!loggedInvalidRemoveComponent)
      {
        global::Improbable.Worker.ClientError.LogClientException(new global::System.InvalidOperationException(
            "Component Metadata removed from entity " + op.EntityId + ", but it does not exist." +
            "This error will be reported once only for each component."));
        loggedInvalidRemoveComponent = true;
      }
    });

    dispatcher.OnAuthorityChange<Metadata>((op) => {
      Impl impl;
      if (implMap.TryGetValue(op.EntityId, out impl))
      {
        impl.On_AuthorityChange(op.HasAuthority);
        if (callbacks.OnAuthorityChanged != null)
        {
          callbacks.OnAuthorityChanged(op.EntityId, this, op.HasAuthority, impl);
        }
      }
    });

    dispatcher.OnComponentUpdate<Metadata>((op) => {
      Impl impl;
      if (implMap.TryGetValue(op.EntityId, out impl))
      {
        impl.On_ComponentUpdate(op.Update.Get());
        if (callbacks.OnComponentUpdated != null)
        {
          callbacks.OnComponentUpdated(op.EntityId, this, op.Update.Get());
        }
      }
    });
  }

  public object GetComponentForEntity(global::Improbable.EntityId entityId)
  {
    Impl impl;
    implMap.TryGetValue(entityId, out impl);
    return impl;
  }

  public void DisconnectEventHandlersWithTarget(global::Improbable.EntityId entityId, object actionTarget)
  {
    Impl impl;
    if (implMap.TryGetValue(entityId, out impl))
    {
      impl.DisconnectEventHandlers(actionTarget);
    }
  }

  #pragma warning disable 0618
  public class Impl : MetadataWriter, Writer, Updater
  #pragma warning restore 0618
  {
    private readonly global::Improbable.Worker.Connection connection;
    private readonly global::Improbable.EntityId entityId;
    private readonly CommandReceiverImpl commandReceiver;

    private Data data;
    private bool hasAuthority = false;
    
    public uint ComponentId { get { return 53; } }

    public Impl(global::Improbable.Worker.Connection connection, global::Improbable.EntityId entityId, Data initialData)
    {
      this.connection = connection;
      this.entityId = entityId;
      this.commandReceiver = new CommandReceiverImpl();
      data = initialData.DeepCopy();

      entityTypeUpdateEventHandler = 
        new global::Improbable.Entity.Component.PropertyCallbackHandler<string>(() => data.Value.entityType);
    }

    // Constructor for tests.
    public Impl()
    {
    }

    public ICommandReceiver CommandReceiver
    {
      get { return commandReceiver; }
    }

    internal CommandReceiverImpl CommandReceiverInternal
    {
      get { return commandReceiver; }
    }

    private global::Improbable.Entity.Component.EventCallbackHandler<Update> componentUpdatedHandler
      = new global::Improbable.Entity.Component.EventCallbackHandler<Update>();

    public global::Improbable.Entity.Component.EventCallbackHandler<Update> ComponentUpdated
    {
      get { return componentUpdatedHandler; }
	  set { componentUpdatedHandler = value; }
    }
    
    private global::Improbable.Entity.Component.EventCallbackHandler<bool> authorityChangedHandler
      = new global::Improbable.Entity.Component.EventCallbackHandler<bool>();

    public global::Improbable.Entity.Component.EventCallbackHandler<bool> AuthorityChanged
    {
      get { return authorityChangedHandler; }
	  set { authorityChangedHandler = value; }  
    }

    public bool HasAuthority
    {
      get { return hasAuthority; }
    }

    public global::Improbable.MetadataData Data
    {
      get { return data.Value; }
    }

    public void Send(Update update)
    {
      connection.SendComponentUpdate(entityId, update);
    }

    public void CopyAndSend(Update update)
    {
      Send(update.DeepCopy());
    }

    internal void On_AuthorityChange(bool newAuthority)
    {
      hasAuthority = newAuthority;
      authorityChangedHandler.InvokeCallbacks(newAuthority);
    }

    internal void On_ComponentUpdate(Update update)
    {
      update.ApplyTo(data);
      componentUpdatedHandler.InvokeCallbacks(update);
      TriggerCallbacks(update);
    }

    public global::Improbable.EntityId EntityId
    {
      get { return entityId; }
    }

    private global::Improbable.Entity.Component.PropertyCallbackHandler<string> entityTypeUpdateEventHandler;

    public global::Improbable.Entity.Component.PropertyCallbackHandler<string> EntityTypeUpdated
    {
      get { return entityTypeUpdateEventHandler; }  
	  set { entityTypeUpdateEventHandler = value; }  
    }

    private void TriggerCallbacks(Update update)
    {
      if (update.entityType.HasValue)
      {
        entityTypeUpdateEventHandler.InvokeCallbacks(update.entityType.Value);
      }
    }

    // Implementation of deprecated interface.
    //----------------------------------------

    private Metadata.Update currentUpdate = null;

    #pragma warning disable 0618
    public Updater Update
    #pragma warning restore 0618
    {
      get
      {
        if (currentUpdate == null)
        {
          currentUpdate = new Metadata.Update();
        }
        else
        {
          global::Improbable.Worker.ClientError.LogClientException(new global::System.InvalidOperationException(
              "Update for component Metadata called with an update already in-flight! " +
              "Each Update must be followed by a FinishAndSend() before another update is made."));
        }
        return this;
      }
    }

    public void FinishAndSend()
    {
      if (currentUpdate != null)
      {
        if (FinishAndSend_ResolveDiff(currentUpdate)) {
          On_ComponentUpdate(currentUpdate);
          connection.SendComponentUpdate(entityId, currentUpdate, /* legacy semantics */ true);
        }
        currentUpdate = null;
      }
      else
      {
        global::Improbable.Worker.ClientError.LogClientException(new global::System.InvalidOperationException(
            "FinishAndSend() for component Everything called with no update in-flight!"));
      }
    }

    public bool IsAuthoritativeHere
    {
      get { return HasAuthority; }
    }

    private readonly global::Improbable.Entity.Component.DeprecatedEvent<global::System.Action<bool>, global::System.Action<bool>> authorityChangedWrapper =
        new global::Improbable.Entity.Component.DeprecatedEvent<global::System.Action<bool>, global::System.Action<bool>>((x) => x);

    #pragma warning disable 0618
    event global::System.Action<bool> MetadataReader.AuthorityChanged
    #pragma warning restore 0618
    {
      add { ((Reader) this).AuthorityChanged.Add(authorityChangedWrapper.Add(value, value.Target)); value(HasAuthority); }
      remove { ((Reader) this).AuthorityChanged.Remove(authorityChangedWrapper.Remove(value, value.Target)); }
    }

    private readonly global::Improbable.Entity.Component.DeprecatedEvent<global::System.Action<Update>, global::System.Action> propertyUpdatedWrapper =
        new global::Improbable.Entity.Component.DeprecatedEvent<global::System.Action<Update>, global::System.Action>(PropertyUpdated_Wrap);

    private static global::System.Action<Update> PropertyUpdated_Wrap(global::System.Action action)
    {
      return (update) => { action(); };
    }

    public event global::System.Action PropertyUpdated
    {
      add { ComponentUpdated.Add(propertyUpdatedWrapper.Add(value, value.Target)); value(); }
      remove { ComponentUpdated.Remove(propertyUpdatedWrapper.Remove(value, value.Target)); }
    }

    public string EntityType
    {
      get { return Data.entityType; }
    }

    private readonly global::Improbable.Entity.Component.DeprecatedEvent<global::System.Action<Update>, global::System.Action<string>> entityTypeWrapper =
        new global::Improbable.Entity.Component.DeprecatedEvent<global::System.Action<Update>, global::System.Action<string>>(EntityTypeUpdated_Wrap);

    private static global::System.Action<Update> EntityTypeUpdated_Wrap(global::System.Action<string> action)
    {
      return (update) => { if (update.entityType.HasValue) { action(update.entityType.Value); } };
    }

    #pragma warning disable 0618
    event global::System.Action<string> MetadataReader.EntityTypeUpdated
    #pragma warning restore 0618
    {
      add { ComponentUpdated.Add(entityTypeWrapper.Add(value, value.Target)); value(Data.entityType); }
      remove { ComponentUpdated.Remove(entityTypeWrapper.Remove(value, value.Target)); }
    }

    #pragma warning disable 0618
    Updater Updater.EntityType(string newValue)
    #pragma warning restore 0618
    {
      currentUpdate.SetEntityType(newValue);
      return this;
    }
    private bool FinishAndSend_ResolveDiff(Update update)
    {
      bool isNoOp = true;
      if (update.entityType.HasValue)
      {
        if (update.entityType.Value == Data.entityType)
        {
          update.entityType.Clear();
        }
        else
        {
          isNoOp = false;
        }
      }
      return !isNoOp;
    }

    internal void DisconnectEventHandlers(object actionTarget)
    {
      entityTypeWrapper.DisconnectTarget(actionTarget);
    }

    internal class CommandReceiverImpl : ICommandReceiver
    {
    }
  }
}
}
