// Generated by SpatialOS codegen. DO NOT EDIT!
// source: improbable.core.CreatePlayerRequest in improbable/core/PlayerCreator.schema.

namespace Improbable.Core
{

public partial struct CreatePlayerRequest : global::System.IEquatable<CreatePlayerRequest>
{
  public CreatePlayerRequest DeepCopy()
  {
    var _result = new CreatePlayerRequest();
    return _result;

  }

  public override bool Equals(object _obj)
  {
    return _obj is CreatePlayerRequest && Equals((CreatePlayerRequest) _obj);
  }

  public static bool operator==(CreatePlayerRequest a, CreatePlayerRequest b)
  {
    return a.Equals(b);
  }

  public static bool operator!=(CreatePlayerRequest a, CreatePlayerRequest b)
  {
    return !a.Equals(b);
  }

  public bool Equals(CreatePlayerRequest _obj)
  {
    return true;
  }

  public override int GetHashCode()
  {
    int _result = 1327;
    return _result;
  }
}

public static class CreatePlayerRequest_Internal
{
  public static unsafe void Write(global::Improbable.Worker.Internal.PbioHandlePool _pool,
                                  CreatePlayerRequest _data, global::Improbable.Worker.Internal.Pbio.Object* _obj)
  {
  }

  public static unsafe CreatePlayerRequest Read(global::Improbable.Worker.Internal.Pbio.Object* _obj)
  {
    CreatePlayerRequest _data;
    return _data;
  }
}

}
