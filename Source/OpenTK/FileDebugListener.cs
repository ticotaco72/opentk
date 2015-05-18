using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace OpenTK
{
    class FileDebugListener : TraceListener
    {
        const string filename = "opentk_debug.txt";

        public FileDebugListener()
        {
            File.Delete(filename);
        }

        public override void Write(string message)
        {
            File.AppendAllText(filename, message);
        }

        public override void WriteLine(string message)
        {
            File.AppendAllText(filename, message + "\r\n");
        }
    }
}
