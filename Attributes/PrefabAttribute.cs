namespace UnityUtils.Attributes
{
  [System.AttributeUsage(System.AttributeTargets.Class, Inherited = true)]
  public class PrefabAttribute : System.Attribute
  {
    public readonly string Name;
    public readonly bool Persistent;

    public PrefabAttribute(string name, bool persistent = false)
    {
      Name = name;
      Persistent = persistent;
    }
  }
}