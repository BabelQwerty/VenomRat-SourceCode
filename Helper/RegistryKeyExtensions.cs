using System;
using Microsoft.Win32;

namespace Server.Helper;

public static class RegistryKeyExtensions
{
	public static string RegistryTypeToString(this RegistryValueKind valueKind, object valueData)
	{
		if (valueData == null)
		{
			return "(value not set)";
		}
		switch (valueKind)
		{
		case RegistryValueKind.Binary:
			if (((byte[])valueData).Length == 0)
			{
				return "(zero-length binary value)";
			}
			return BitConverter.ToString((byte[])valueData).Replace("-", " ").ToLower();
		case RegistryValueKind.MultiString:
			return string.Join(" ", (string[])valueData);
		case RegistryValueKind.DWord:
			return string.Format("0x{0} ({1})", ((uint)(int)valueData).ToString("x8"), ((uint)(int)valueData).ToString());
		case RegistryValueKind.QWord:
			return string.Format("0x{0} ({1})", ((ulong)(long)valueData).ToString("x8"), ((ulong)(long)valueData).ToString());
		case RegistryValueKind.String:
		case RegistryValueKind.ExpandString:
			return valueData.ToString();
		default:
			return string.Empty;
		}
	}

	public static RegistryKey OpenReadonlySubKeySafe(this RegistryKey key, string name)
	{
		try
		{
			return key.OpenSubKey(name, writable: false);
		}
		catch
		{
			return null;
		}
	}

	public static RegistryKey OpenWritableSubKeySafe(this RegistryKey key, string name)
	{
		try
		{
			return key.OpenSubKey(name, writable: true);
		}
		catch
		{
			return null;
		}
	}

	public static string RegistryTypeToString(this RegistryValueKind valueKind)
	{
		return valueKind switch
		{
			RegistryValueKind.Binary => "REG_BINARY", 
			RegistryValueKind.MultiString => "REG_MULTI_SZ", 
			RegistryValueKind.DWord => "REG_DWORD", 
			RegistryValueKind.QWord => "REG_QWORD", 
			RegistryValueKind.String => "REG_SZ", 
			RegistryValueKind.ExpandString => "REG_EXPAND_SZ", 
			RegistryValueKind.Unknown => "(Unknown)", 
			_ => "REG_NONE", 
		};
	}
}
