using System.IO;
using System.Text;

namespace KoukosUtilities
{
    public static class ConfigFileWriter
    {
        private static readonly UTF8Encoding Utf8Encode;
        private static StringBuilder Builder;
        static ConfigFileWriter()
        {
            Utf8Encode = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
            Builder = new StringBuilder();
        }

        public static void WriteLine(object key, object line)
        {
            Builder.Append(key);
            Builder.Append(": ");
            Builder.AppendLine(line.ToString());
        }

        public static void WriteAllToFile(string dir)
        {
            File.WriteAllText(dir, Builder.ToString(), Utf8Encode);
            Builder.Clear();
        }
    }
}
