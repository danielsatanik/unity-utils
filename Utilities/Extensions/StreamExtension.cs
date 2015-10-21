using System;
using System.IO;

namespace UnityUtils.Utilities.Extensions
{
  public static class StreamExtension
  {
    public static void CopyTo(this Stream original, Stream destination)
    {
      if (destination == null)
      {
        throw new ArgumentNullException("destination");
      }
      if (!original.CanRead && !original.CanWrite)
      {
        throw new ObjectDisposedException("ObjectDisposedException");
      }
      if (!destination.CanRead && !destination.CanWrite)
      {
        throw new ObjectDisposedException("ObjectDisposedException");
      }
      if (!original.CanRead)
      {
        throw new NotSupportedException("NotSupportedException source");
      }
      if (!destination.CanWrite)
      {
        throw new NotSupportedException("NotSupportedException destination");
      }

      byte[] array = new byte[4096];
      int count;
      while ((count = original.Read(array, 0, array.Length)) != 0)
      {
        destination.Write(array, 0, count);
      }
    }
  }
}