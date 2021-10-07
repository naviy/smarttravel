namespace Luxena
{
	public static class ByteArrayExtensions
	{
		public static string ToHexString(this byte[] bytes)
		{
			var c = new char[bytes.Length * 2];

			for (var i = 0; i < bytes.Length; i++)
			{
				var b = (byte) (bytes[i] >> 4);
				c[i*2] = (char) (b > 9 ? b + 0x37 : b + 0x30);
				b = (byte) (bytes[i] & 0xF);
				c[i*2 + 1] = (char) (b > 9 ? b + 0x37 : b + 0x30);
			}

			return new string(c);
		}
	}
}