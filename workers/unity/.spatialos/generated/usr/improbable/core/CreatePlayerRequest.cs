// Generated by SpatialOS codegen. DO NOT EDIT!
// source: improbable.core.CreatePlayerRequest in improbable/core/PlayerCreator.schema.

namespace Improbable.Core
{

public partial struct CreatePlayerRequest : global::System.IEquatable<CreatePlayerRequest>
{
  /// <summary>
  /// Field player_id = 1.
  /// </summary>
  public int playerId;

  public CreatePlayerRequest(int playerId)
  {
    this.playerId = playerId;
  }

  public CreatePlayerRequest DeepCopy()
  {
    var _result = new CreatePlayerRequest();
    _result.playerId = playerId;
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
    return
        playerId == _obj.playerId;
  }

  public override int GetHashCode()
  {
    int _result = 1327;
    _result = (_result * 977) + playerId.GetHashCode();
    return _result;
  }
}

public static class CreatePlayerRequest_Internal
{
  public static unsafe void Write(global::Improbable.Worker.Internal.PbioHandlePool _pool,
                                  CreatePlayerRequest _data, global::Improbable.Worker.Internal.Pbio.Object* _obj)
  {
    {
      global::Improbable.Worker.Internal.Pbio.AddInt32(_obj, 1, _data.playerId);
    }
  }

  public static unsafe CreatePlayerRequest Read(global::Improbable.Worker.Internal.Pbio.Object* _obj)
  {
    CreatePlayerRequest _data;
    {
      _data.playerId = global::Improbable.Worker.Internal.Pbio.GetInt32(_obj, 1);
    }
    return _data;
  }
}

}
