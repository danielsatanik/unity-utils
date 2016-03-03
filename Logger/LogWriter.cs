using UnityEngine;

namespace UnityUtils.Debugging
{
    public class LogWriter : System.IO.TextWriter
    {
        public override void Write(string value)
        {
            base.Write(value);
            Debug.Log(value);
        }

        public override System.Text.Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }
}