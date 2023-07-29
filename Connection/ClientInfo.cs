using System.Collections.Generic;
using Params;

namespace Server.Connection;

public class ClientInfo
{
	public string tooltip = string.Empty;

	public List<string> apps = new List<string>();

	public List<string> runningapps = new List<string>();

	public KeylogParams keyparam = new KeylogParams();

	public string summary => $"{ip}:{port}";

	public string ip { get; set; } = string.Empty;


	public int port { get; set; }

	public string note { get; set; } = string.Empty;


	public string country { get; set; } = string.Empty;


	public string group { get; set; } = string.Empty;


	public string hwid { get; set; } = string.Empty;


	public string desktopname { get; set; } = string.Empty;


	public string user { get; set; } = string.Empty;


	public string cpu { get; set; } = string.Empty;


	public string gpu { get; set; } = string.Empty;


	public string ram { get; set; } = string.Empty;


	public bool camera { get; set; }

	public string os { get; set; } = string.Empty;


	public string version { get; set; } = string.Empty;


	public bool admin { get; set; }

	public string permission
	{
		get
		{
			if (!admin)
			{
				return "User";
			}
			return "Admin";
		}
	}

	public string defender { get; set; } = string.Empty;


	public string installed { get; set; } = string.Empty;


	public string ping { get; set; } = "0 MS";


	public string activewin { get; set; } = "";

}
