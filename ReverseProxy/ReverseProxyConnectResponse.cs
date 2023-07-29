namespace Server.ReverseProxy;

public class ReverseProxyConnectResponse : ReverseProxyCommand
{
	public bool IsConnected { get; set; }

	public byte[] LocalAddress { get; set; }

	public int LocalPort { get; set; }

	public int RemotePort { get; set; }

	public string HostName { get; set; }
}
