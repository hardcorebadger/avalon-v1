// Generated by SpatialOS codegen. DO NOT EDIT!
// source: Persistence in improbable/standard_library.schema.

namespace Improbable
{

public static class Persistence_Extensions
{
  public static Persistence.Data Get(this global::Improbable.Worker.IComponentData<Persistence> data)
  {
    return (Persistence.Data) data;
  }

  public static Persistence.Update Get(this global::Improbable.Worker.IComponentUpdate<Persistence> update)
  {
    return (Persistence.Update) update;
  }
}

public partial class Persistence : global::Improbable.Worker.IComponentMetaclass
{
  public const uint ComponentId = 55;

  uint global::Improbable.Worker.IComponentMetaclass.ComponentId
  {
    get { return ComponentId; }
  }

  /// <summary>
  /// Concrete data type for the Persistence component.
  /// </summary>
  public class Data : global::Improbable.Worker.IComponentData<Persistence>
  {
    public global::Improbable.PersistenceData Value;

    public Data(global::Improbable.PersistenceData value)
    {
      Value = value;
    }

    public Data()
    {
      Value = new global::Improbable.PersistenceData();
    }

    public Data DeepCopy()
    {
      return new Data(Value.DeepCopy());
    }

    public global::Improbable.Worker.IComponentUpdate<Persistence> ToUpdate()
    {
      var update = new Update();
      return update;
    }
  }

  /// <summary>
  /// Concrete update type for the Persistence component.
  /// </summary>
  public class Update : global::Improbable.Worker.IComponentUpdate<Persistence>
  {
    public Update DeepCopy()
    {
      var _result = new Update();
      return _result;
    }

    public global::Improbable.Worker.IComponentData<Persistence> ToInitialData()
    {
      return new Data(new global::Improbable.PersistenceData());
    }

    public void ApplyTo(global::Improbable.Worker.IComponentData<Persistence> _data)
    {
    }
  }

  public partial class Commands
  {
  }

  // Implementation details below here.
  //----------------------------------------------------------------

  public global::Improbable.Worker.Internal.ComponentProtocol.ClientComponentVtable Vtable {
    get {
      global::Improbable.Worker.Internal.ComponentProtocol.ClientComponentVtable vtable;
      vtable.ComponentId = ComponentId;
      vtable.BufferFree = global::System.Runtime.InteropServices.Marshal
          .GetFunctionPointerForDelegate(global::Improbable.Worker.Internal.ClientObjects.ClientBufferFree);
      vtable.Free = global::System.Runtime.InteropServices.Marshal
          .GetFunctionPointerForDelegate(global::Improbable.Worker.Internal.ClientObjects.ClientFree);
      vtable.Copy = global::System.Runtime.InteropServices.Marshal
          .GetFunctionPointerForDelegate(global::Improbable.Worker.Internal.ClientObjects.ClientCopy);
      vtable.Deserialize = global::System.Runtime.InteropServices.Marshal
          .GetFunctionPointerForDelegate(clientDeserialize);
      vtable.Serialize = global::System.Runtime.InteropServices.Marshal
          .GetFunctionPointerForDelegate(clientSerialize);
      return vtable;
    }
  }

  public void AddSnapshotToEntity(global::Improbable.Worker.Internal.ComponentProtocol.ClientObject update,
                                  global::Improbable.Worker.Entity entity)
  {
    var dereferenced = (global::Improbable.Worker.IComponentData<Persistence>)
        global::Improbable.Worker.Internal.ClientObjects.Instance
        .Dereference(update.Reference);
    entity.Add<Persistence>(dereferenced);
  }

  public object ExtractSnapshot(global::Improbable.Worker.Entity entity)
  {
    return entity.Get<Persistence>().Value;
  }

  public void TrackComponent(global::Improbable.Worker.View view)
  {
    view.OnAddComponent<Persistence>(op =>
    {
      if (view.Entities.ContainsKey(op.EntityId))
      {
        view.Entities[op.EntityId].Add<Persistence>(op.Data);
      }
    });
    view.OnRemoveComponent<Persistence>(op =>
    {
      if (view.Entities.ContainsKey(op.EntityId))
      {
        view.Entities[op.EntityId].Remove<Persistence>();
      }
    });
    view.OnAuthorityChange<Persistence>(op =>
    {
      if (view.Entities.ContainsKey(op.EntityId))
      {
        view.Entities[op.EntityId].SetAuthority<Persistence>(op.HasAuthority);
      }
    });
    view.OnComponentUpdate<Persistence>(op =>
    {
      if (view.Entities.ContainsKey(op.EntityId))
      {
        view.Entities[op.EntityId].Update<Persistence>(op.Update);
      }
    });
  }

  private static unsafe readonly global::Improbable.Worker.Internal.ComponentProtocol.ClientDeserialize
      clientDeserialize = ClientDeserialize;
  private static unsafe readonly global::Improbable.Worker.Internal.ComponentProtocol.ClientSerialize
      clientSerialize = ClientSerialize;

  [global::Improbable.Worker.Internal.MonoPInvokeCallback(typeof(global::Improbable.Worker.Internal.ComponentProtocol.ClientDeserialize))]
  private static unsafe bool
  ClientDeserialize(global::System.UInt32 componentId,
                    global::System.Byte objType,
                    global::System.Byte* buffer,
                    global::System.UInt32 length,
                    global::Improbable.Worker.Internal.ComponentProtocol.ClientObject** obj)
  {
    *obj = null;
    using (var pbio = global::Improbable.Worker.Internal.Pbio.Create())
    {
      try
      {
        var root = global::Improbable.Worker.Internal.Pbio.GetRootObject(pbio);
        global::Improbable.Worker.Internal.Pbio.MergeFromBuffer(root, buffer, length);
        *obj = global::Improbable.Worker.Internal.ClientObjects.ObjectAlloc();
        if (objType == (byte) global::Improbable.Worker.Internal.ComponentProtocol.ClientObjectType.Update) {
          var data = new Update();
          (*obj)->Reference = global::Improbable.Worker.Internal.ClientObjects.Instance.CreateReference(data);
        }
        else if (objType == (byte) global::Improbable.Worker.Internal.ComponentProtocol.ClientObjectType.Snapshot)
        {
          var data = new Data(global::Improbable.PersistenceData_Internal.Read(
              global::Improbable.Worker.Internal.Pbio.GetObject(root, 55)));
          (*obj)->Reference = global::Improbable.Worker.Internal.ClientObjects.Instance.CreateReference(data);
        }
        else if (objType == (byte) global::Improbable.Worker.Internal.ComponentProtocol.ClientObjectType.Request)
        {
          var data = new global::Improbable.Worker.Internal.GenericCommandObject();
          (*obj)->Reference = global::Improbable.Worker.Internal.ClientObjects.Instance.CreateReference(data);
        }
        else if (objType == (byte) global::Improbable.Worker.Internal.ComponentProtocol.ClientObjectType.Response)
        {
          var data = new global::Improbable.Worker.Internal.GenericCommandObject();
          (*obj)->Reference = global::Improbable.Worker.Internal.ClientObjects.Instance.CreateReference(data);
        }
      }
      catch (global::System.Exception e)
      {
        global::Improbable.Worker.ClientError.LogClientException(e);
        return false;
      }
      return true;
    }
  }

  [global::Improbable.Worker.Internal.MonoPInvokeCallback(typeof(global::Improbable.Worker.Internal.ComponentProtocol.ClientSerialize))]
  private static unsafe void
  ClientSerialize(global::System.UInt32 componentId,
                  global::System.Byte objType,
                  global::Improbable.Worker.Internal.ComponentProtocol.ClientObject* obj,
                  global::System.Byte** buffer,
                  global::System.UInt32* length)
  {
    *buffer = null;
    *length = 0;
    using (var _pool = new global::Improbable.Worker.Internal.PbioHandlePool())
    {
      try
      {
        var root = global::Improbable.Worker.Internal.Pbio.GetRootObject(_pool.GetPbio());
        if (objType == (byte) global::Improbable.Worker.Internal.ComponentProtocol.ClientObjectType.Update) {
          global::Improbable.Worker.Internal.Pbio.AddObject(
              global::Improbable.Worker.Internal.Pbio.AddObject(root, /* entity_state */ 2), 55);
        }
        else if (objType == (byte) global::Improbable.Worker.Internal.ComponentProtocol.ClientObjectType.Snapshot) {
          var data = (Data) global::Improbable.Worker.Internal.ClientObjects.Instance.Dereference(obj->Reference);
          global::Improbable.PersistenceData_Internal.Write(_pool, data.Value, global::Improbable.Worker.Internal.Pbio.AddObject(root, 55));
        }
        else if (objType == (byte) global::Improbable.Worker.Internal.ComponentProtocol.ClientObjectType.Request)
        {
          global::Improbable.Worker.Internal.Pbio.AddObject(root, 55);
        }
        else if (objType == (byte) global::Improbable.Worker.Internal.ComponentProtocol.ClientObjectType.Response)
        {
          global::Improbable.Worker.Internal.Pbio.AddObject(root, 55);
        }
        *length = global::Improbable.Worker.Internal.Pbio.GetWriteBufferLength(root);
        *buffer = global::Improbable.Worker.Internal.ClientObjects.BufferAlloc((int) *length);
        global::Improbable.Worker.Internal.Pbio.WriteToBuffer(root, *buffer);
      }
      catch (global::System.Exception e)
      {
        global::Improbable.Worker.ClientError.LogClientException(e);
      }
    }
  }
}

}
