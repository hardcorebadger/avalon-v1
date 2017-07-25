// Generated by SpatialOS codegen. DO NOT EDIT!
// source: PlayerCreator in improbable/core/PlayerCreator.schema.

namespace Improbable.Core
{

public static class PlayerCreator_Extensions
{
  public static PlayerCreator.Data Get(this global::Improbable.Worker.IComponentData<PlayerCreator> data)
  {
    return (PlayerCreator.Data) data;
  }

  public static PlayerCreator.Update Get(this global::Improbable.Worker.IComponentUpdate<PlayerCreator> update)
  {
    return (PlayerCreator.Update) update;
  }

  public static PlayerCreator.Commands.CreatePlayer.Request Get(this global::Improbable.Worker.ICommandRequest<PlayerCreator.Commands.CreatePlayer> request)
  {
    return (PlayerCreator.Commands.CreatePlayer.Request) request;
  }

  public static PlayerCreator.Commands.CreatePlayer.Response Get(this global::Improbable.Worker.ICommandResponse<PlayerCreator.Commands.CreatePlayer> response)
  {
    return (PlayerCreator.Commands.CreatePlayer.Response) response;
  }
}

public partial class PlayerCreator : global::Improbable.Worker.IComponentMetaclass
{
  public const uint ComponentId = 1001;

  uint global::Improbable.Worker.IComponentMetaclass.ComponentId
  {
    get { return ComponentId; }
  }

  /// <summary>
  /// Concrete data type for the PlayerCreator component.
  /// </summary>
  public class Data : global::Improbable.Worker.IComponentData<PlayerCreator>
  {
    public global::Improbable.Core.PlayerCreatorData Value;

    public Data(global::Improbable.Core.PlayerCreatorData value)
    {
      Value = value;
    }

    public Data()
    {
      Value = new global::Improbable.Core.PlayerCreatorData();
    }

    public Data DeepCopy()
    {
      return new Data(Value.DeepCopy());
    }

    public global::Improbable.Worker.IComponentUpdate<PlayerCreator> ToUpdate()
    {
      var update = new Update();
      return update;
    }
  }

  /// <summary>
  /// Concrete update type for the PlayerCreator component.
  /// </summary>
  public class Update : global::Improbable.Worker.IComponentUpdate<PlayerCreator>
  {
    public Update DeepCopy()
    {
      var _result = new Update();
      return _result;
    }

    public global::Improbable.Worker.IComponentData<PlayerCreator> ToInitialData()
    {
      return new Data(new global::Improbable.Core.PlayerCreatorData());
    }

    public void ApplyTo(global::Improbable.Worker.IComponentData<PlayerCreator> _data)
    {
    }
  }

  public partial class Commands
  {
    /// <summary>
    /// Command create_player.
    /// </summary>
    public partial class CreatePlayer : global::Improbable.Worker.ICommandMetaclass
    {
      public uint ComponentId { get { return 1001; } }
      public uint CommandId { get { return 1; } }

      public class Request : global::Improbable.Worker.ICommandRequest<CreatePlayer>
      {
        public global::Improbable.Core.CreatePlayerRequest Value;

        public Request(global::Improbable.Core.CreatePlayerRequest value)
        {
          Value = value;
        }

        public Request()
        {
          Value = new global::Improbable.Core.CreatePlayerRequest();
        }

        public Request DeepCopy()
        {
          return new Request(Value.DeepCopy());
        }

        public global::Improbable.Worker.Internal.GenericCommandObject ToGenericObject()
        {
          return new global::Improbable.Worker.Internal.GenericCommandObject(1, this);
        }
      }

      public class Response : global::Improbable.Worker.ICommandResponse<CreatePlayer>
      {
        public global::Improbable.Core.CreatePlayerResponse Value;

        public Response(global::Improbable.Core.CreatePlayerResponse value)
        {
          Value = value;
        }

        public Response()
        {
          Value = new global::Improbable.Core.CreatePlayerResponse();
        }

        public Response DeepCopy()
        {
          return new Response(Value.DeepCopy());
        }

        public global::Improbable.Worker.Internal.GenericCommandObject ToGenericObject()
        {
          return new global::Improbable.Worker.Internal.GenericCommandObject(1, this);
        }
      }
    }
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
    var dereferenced = (global::Improbable.Worker.IComponentData<PlayerCreator>)
        global::Improbable.Worker.Internal.ClientObjects.Instance
        .Dereference(update.Reference);
    entity.Add<PlayerCreator>(dereferenced);
  }

  public object ExtractSnapshot(global::Improbable.Worker.Entity entity)
  {
    return entity.Get<PlayerCreator>().Value;
  }

  public void TrackComponent(global::Improbable.Worker.View view)
  {
    view.OnAddComponent<PlayerCreator>(op =>
    {
      if (view.Entities.ContainsKey(op.EntityId))
      {
        view.Entities[op.EntityId].Add<PlayerCreator>(op.Data);
      }
    });
    view.OnRemoveComponent<PlayerCreator>(op =>
    {
      if (view.Entities.ContainsKey(op.EntityId))
      {
        view.Entities[op.EntityId].Remove<PlayerCreator>();
      }
    });
    view.OnAuthorityChange<PlayerCreator>(op =>
    {
      if (view.Entities.ContainsKey(op.EntityId))
      {
        view.Entities[op.EntityId].SetAuthority<PlayerCreator>(op.HasAuthority);
      }
    });
    view.OnComponentUpdate<PlayerCreator>(op =>
    {
      if (view.Entities.ContainsKey(op.EntityId))
      {
        view.Entities[op.EntityId].Update<PlayerCreator>(op.Update);
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
          var data = new Data(global::Improbable.Core.PlayerCreatorData_Internal.Read(
              global::Improbable.Worker.Internal.Pbio.GetObject(root, 1001)));
          (*obj)->Reference = global::Improbable.Worker.Internal.ClientObjects.Instance.CreateReference(data);
        }
        else if (objType == (byte) global::Improbable.Worker.Internal.ComponentProtocol.ClientObjectType.Request)
        {
          var data = new global::Improbable.Worker.Internal.GenericCommandObject();
          var commandObject = global::Improbable.Worker.Internal.Pbio.GetObject(root, 1001);
          if (global::Improbable.Worker.Internal.Pbio.GetObjectCount(commandObject, 1) != 0) {
            data.CommandId = 1;
            data.CommandObject = new Commands.CreatePlayer.Request(global::Improbable.Core.CreatePlayerRequest_Internal.Read(global::Improbable.Worker.Internal.Pbio.GetObject(commandObject, 1)));
          }
          (*obj)->Reference = global::Improbable.Worker.Internal.ClientObjects.Instance.CreateReference(data);
        }
        else if (objType == (byte) global::Improbable.Worker.Internal.ComponentProtocol.ClientObjectType.Response)
        {
          var data = new global::Improbable.Worker.Internal.GenericCommandObject();
          var commandObject = global::Improbable.Worker.Internal.Pbio.GetObject(root, 1001);
          if (global::Improbable.Worker.Internal.Pbio.GetObjectCount(commandObject, 2) != 0) {
            data.CommandId = 1;
            data.CommandObject = new Commands.CreatePlayer.Response(global::Improbable.Core.CreatePlayerResponse_Internal.Read(global::Improbable.Worker.Internal.Pbio.GetObject(commandObject, 2)));
          }
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
              global::Improbable.Worker.Internal.Pbio.AddObject(root, /* entity_state */ 2), 1001);
        }
        else if (objType == (byte) global::Improbable.Worker.Internal.ComponentProtocol.ClientObjectType.Snapshot) {
          var data = (Data) global::Improbable.Worker.Internal.ClientObjects.Instance.Dereference(obj->Reference);
          global::Improbable.Core.PlayerCreatorData_Internal.Write(_pool, data.Value, global::Improbable.Worker.Internal.Pbio.AddObject(root, 1001));
        }
        else if (objType == (byte) global::Improbable.Worker.Internal.ComponentProtocol.ClientObjectType.Request)
        {
          var data = (global::Improbable.Worker.Internal.GenericCommandObject)
              global::Improbable.Worker.Internal.ClientObjects.Instance.Dereference(obj->Reference);
          var commandObject = global::Improbable.Worker.Internal.Pbio.AddObject(root, 1001);
          if (data.CommandId == 1)
          {
            var requestObject = (Commands.CreatePlayer.Request) data.CommandObject;
            {
              global::Improbable.Core.CreatePlayerRequest_Internal.Write(_pool, requestObject.Value, global::Improbable.Worker.Internal.Pbio.AddObject(commandObject, 1));
            }
          }
        }
        else if (objType == (byte) global::Improbable.Worker.Internal.ComponentProtocol.ClientObjectType.Response)
        {
          var data = (global::Improbable.Worker.Internal.GenericCommandObject)
              global::Improbable.Worker.Internal.ClientObjects.Instance.Dereference(obj->Reference);
          var commandObject = global::Improbable.Worker.Internal.Pbio.AddObject(root, 1001);
          if (data.CommandId == 1)
          {
            var responseObject = (Commands.CreatePlayer.Response) data.CommandObject;
            {
              global::Improbable.Core.CreatePlayerResponse_Internal.Write(_pool, responseObject.Value, global::Improbable.Worker.Internal.Pbio.AddObject(commandObject, 2));
            }
          }
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
