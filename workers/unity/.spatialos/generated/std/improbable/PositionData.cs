// Generated by SpatialOS codegen. DO NOT EDIT!
// source: improbable.PositionData in improbable/standard_library.schema.

namespace Improbable
{

public partial struct PositionData : global::System.IEquatable<PositionData>
{
  /// <summary>
  /// Field coords = 1.
  /// </summary>
  public global::Improbable.Coordinates coords;

  public PositionData(global::Improbable.Coordinates coords)
  {
    this.coords = coords;
  }

  public PositionData DeepCopy()
  {
    var _result = new PositionData();
    _result.coords = coords.DeepCopy();
    return _result;

  }

  public override bool Equals(object _obj)
  {
    return _obj is PositionData && Equals((PositionData) _obj);
  }

  public static bool operator==(PositionData a, PositionData b)
  {
    return a.Equals(b);
  }

  public static bool operator!=(PositionData a, PositionData b)
  {
    return !a.Equals(b);
  }

  public bool Equals(PositionData _obj)
  {
    return
        coords == _obj.coords;
  }

  public override int GetHashCode()
  {
    int _result = 1327;
    _result = (_result * 977) + coords.GetHashCode();
    return _result;
  }
}

public static class PositionData_Internal
{
  public static unsafe void Write(global::Improbable.Worker.Internal.PbioHandlePool _pool,
                                  PositionData _data, global::Improbable.Worker.Internal.Pbio.Object* _obj)
  {
    {
      global::Improbable.Coordinates_Internal.Write(_pool, _data.coords, global::Improbable.Worker.Internal.Pbio.AddObject(_obj, 1));
    }
  }

  public static unsafe PositionData Read(global::Improbable.Worker.Internal.Pbio.Object* _obj)
  {
    PositionData _data;
    {
      _data.coords = global::Improbable.Coordinates_Internal.Read(global::Improbable.Worker.Internal.Pbio.GetObject(_obj, 1));
    }
    return _data;
  }
}

}
