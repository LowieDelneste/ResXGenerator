using System.Security.Cryptography;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Aigamo.ResXGenerator.Tools;

public readonly record struct AdditionalTextWithHash
{
	public AdditionalTextWithHash(AdditionalText file)
		: this(file, CreateHash(file))
	{
	}

	public AdditionalTextWithHash(AdditionalText file, Guid hash)
	{
		File = file;
		Hash = hash;
	}

	public AdditionalText File { get; }
	public Guid Hash { get; }

	public bool Equals(AdditionalTextWithHash other) => File.Path.Equals(other.File.Path) && Hash.Equals(other.Hash);

	public override int GetHashCode()
	{
		unchecked
		{
			return File.GetHashCode() * 397 ^ Hash.GetHashCode();
		}
	}

	public override string ToString() => $"{nameof(File)}: {File?.Path}, {nameof(Hash)}: {Hash}";

	private static Guid CreateHash(AdditionalText file)
	{
		var pathBytes = Encoding.UTF8.GetBytes(file.Path ?? string.Empty);
		var content = file.GetText()?.ToString() ?? string.Empty;
		var contentBytes = Encoding.UTF8.GetBytes(content);
		var payload = new byte[pathBytes.Length + contentBytes.Length + 1];
		Buffer.BlockCopy(pathBytes, 0, payload, 0, pathBytes.Length);
		payload[pathBytes.Length] = 0;
		Buffer.BlockCopy(contentBytes, 0, payload, pathBytes.Length + 1, contentBytes.Length);

		using var sha256 = SHA256.Create();
		var hashBytes = sha256.ComputeHash(payload);
		var guidBytes = new byte[16];
		Buffer.BlockCopy(hashBytes, 0, guidBytes, 0, 16);
		return new Guid(guidBytes);
	}
}
