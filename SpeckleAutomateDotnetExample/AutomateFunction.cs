using Objects;
using Objects.Geometry;
using Objects.Primitive;
using Speckle.Automate.Sdk;
using Speckle.Core.Logging;
using Speckle.Core.Models;
using Speckle.Core.Models.Extensions;

public static class AutomateFunction
{
  static double GetMeasurement(Base obj, string key)
  {
    object? item = obj[key];
    if (item is double d) return d;
    if (item is float f) return f;
    if (item is int i) return i;

    if (item is string s && double.TryParse(s, out double sD)) return sD;

    return 0;
  }

  public static async Task Run(
    AutomationContext automationContext,
    FunctionInputs functionInputs
  )
  {
    _ = typeof(ObjectsKit).Assembly; // INFO: Force objects kit to initialize
    Base commitObject = await automationContext.ReceiveVersion();
    Console.WriteLine(commitObject);

    var width = GetMeasurement(commitObject, "width");
    Console.WriteLine(width);

    var height = GetMeasurement(commitObject, "height");
    Console.WriteLine(height);

    var length = GetMeasurement(commitObject, "length");
    Console.WriteLine(length);

    Plane plane = new(new Point(0, 0, 0), new Vector(0, 0, 1), new Vector(1, 0, 0), new Vector(0, 1, 0));
    Box box = new(plane, new(0, width), new(0, length), new(0, height));
    await automationContext.CreateNewVersionInProject(box, functionInputs.TargetModelName);
    automationContext.MarkRunSuccess("Execution completed successfully");
  }
}
