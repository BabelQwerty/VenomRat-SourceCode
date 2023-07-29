namespace Server.ReverseProxy;

public class ReverseProxyConnect : ReverseProxyCommand
{
	public string Target { get; set; }

	public int Port { get; set; }
}
