using ProtoBuf;
using System;
using System.IO;

public static class Serialization 
{
	public static byte[] Serialize<T>(T instance)
	{
		byte[] bytes;
		using (var ms = new MemoryStream())
		{
			Serializer.Serialize(ms, instance);
			bytes = new byte[ms.Position];
			var fullBytes = ms.GetBuffer();
			Array.Copy(fullBytes, bytes, bytes.Length);
		}
		return bytes;
	}

	public static T Deserialize<T>(object obj)
	{
		byte[] bytes = (byte[])obj;
		using (var ms = new MemoryStream(bytes))
		{
			return Serializer.Deserialize<T>(ms);
		}
	}
}
