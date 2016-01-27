using System.IO;
using Unity.IO.Compression;

namespace UnityUtils.Utilities.Extensions
{
    public static class FileInfoExtension
    {
        /// <summary>
        /// Compress the specified file and keep it.
        /// </summary>
        /// <param name="fi">file</param>
        public static void Compress(this FileInfo fi)
        {
            // Get the stream of the source file.
            using (FileStream inFile = fi.OpenRead())
            {
                // Prevent compressing hidden and 
                // already compressed files.
                if ((File.GetAttributes(fi.FullName)
                    & FileAttributes.Hidden)
                    != FileAttributes.Hidden & fi.Extension != ".gz")
                {
                    string destFileName = fi.FullName.Substring(0, fi.FullName.LastIndexOf('.')) +
                                          System.DateTime.Now.ToString("yyyyMMddHHmmfff") +
                                          fi.FullName.Substring(fi.FullName.LastIndexOf('.')) +
                                          ".gz";
                    // Create the compressed file.
                    using (FileStream outFile =
                               File.Create(destFileName))
                    {
                        using (var compress = new GZipStream(outFile, CompressionMode.Compress))
                        {
                            // Copy the source file into 
                            // the compression stream.
                            inFile.CopyTo(compress);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Archive the specified file
        /// </summary>
        /// <param name="fi">file</param>
        public static void Archive(this FileInfo fi)
        {
            fi.Compress();
            fi.Delete();
        }
    }
}