using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KoukosUtilities
{
    public static class PlayerPreferences
    {
        public enum DataType
        {
            Bool = 00,
            Byte = 01,
            Sbyte = 02,
            Char = 03,
            Decimal = 04,
            Double = 05,
            Float = 06,
            Int = 07,
            Uint = 08,
            Long = 09,
            Ulong = 10,
            Short = 11,
            Ushort = 12,
            String = 13
        }

        private static Dictionary<object, object> Preferences;
        private static string AppdataLocation;
        private static UTF8Encoding Encoding;

        static PlayerPreferences()
        {
            Preferences = new Dictionary<object, object>();
            AppdataLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SCP Secret Laboratory", "registry.txt");
            Encoding = new UTF8Encoding();
            if (!File.Exists(AppdataLocation))
                File.Create(AppdataLocation);
            LoadPrefs(AppdataLocation);
        }

        public static void LoadPrefs(string FilePath)
        {

            if (File.ReadAllLines(FilePath).Length != 0)
            {
                foreach (string S in File.ReadAllLines(FilePath))
                {
                    try
                    {
                        object Key = S.Substring(0, S.LastIndexOf("::-%(|::"));
                        object Value = S.Substring(S.LastIndexOf("::-%(|::") + 8);
                        Preferences.Add(Key, Value);
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
        }

        public static void RefreshPrefs(string FilePath)
        {
            Preferences.Clear();
            LoadPrefs(FilePath);
        }

        public static void SetPrefs()
        {
            DeleteOldPrefs();
            StringBuilder FileBuilder = new StringBuilder();
            for (int i = 0; i < Preferences.Count; i++)
            {
                FileBuilder.Append(Preferences.ElementAt(i).Key);
                FileBuilder.Append("::-%(|::");
                FileBuilder.AppendLine(Preferences.ElementAt(i).Value.ToString());
            }
            File.WriteAllText(AppdataLocation, FileBuilder.ToString(), Encoding);
            FileBuilder.Clear();
            RefreshPrefs(AppdataLocation);
        }

        public static void DeleteOldPrefs()
        {
            File.WriteAllText(AppdataLocation, "", Encoding);
        }

        public static bool GetKey(object Key)
        {
            if (Preferences.TryGetValue(Key, out object Value))
                return true;
            return false;
        }

        public static object GetKey(object Key, object DefValue)
        {
            if (Preferences.TryGetValue(Key, out object Value))
                return Value;
            return DefValue;
        }

        public static void SetKey(object Key, object Value, bool LowerCase = true)
        {
            if (GetKey(Key))
            {
                if (LowerCase)
                    Preferences[Key] = Value.ToString().ToLowerInvariant();
                else
                    Preferences[Key] = Value;
                SetPrefs();
            }
            else
            {
                AddKey(Key, Value, LowerCase);
            }
        }

        public static void AddKey(object Key, object Value, bool LowerCase = true)
        {
            if (!GetKey(Key))
            {
                if (LowerCase)
                    Preferences.Add(Key, Value.ToString().ToLowerInvariant());
                else
                    Preferences.Add(Key, Value);
                SetPrefs();
            }
            else
            {
                SetKey(Key, Value);
            }
        }
    }
}
