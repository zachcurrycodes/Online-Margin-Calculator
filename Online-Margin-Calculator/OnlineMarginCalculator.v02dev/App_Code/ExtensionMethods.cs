﻿using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class ExtensionMethods {
    public static T DeepCopy<T>(this T a) {
        using (MemoryStream stream = new MemoryStream()) {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, a);
            stream.Position = 0;
            return (T)formatter.Deserialize(stream);
        }
    }
}