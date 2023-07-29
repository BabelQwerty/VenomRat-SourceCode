using System.Threading;
using MessagePackLib.MessagePack;
using Server.Connection;

namespace Server.Handle_Packet;

public class HandlePing
{
	public void Ping(Clients client, MsgPack unpack_msgpack)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").SetAsString("Po_ng");
			ThreadPool.QueueUserWorkItem(client.Send, msgPack.Encode2Bytes());
			client.info.activewin = unpack_msgpack.ForcePathObject("Message").AsString;
			Program.mainform.UpdateActWin(client);
		}
		catch
		{
		}
	}

	public void Po_ng(Clients client, MsgPack unpack_msgpack)
	{
		try
		{
			int num = (int)unpack_msgpack.ForcePathObject("Message").AsInteger;
			client.info.ping = $"{num} MS";
			Program.mainform.UpdatePing(client);
		}
		catch
		{
		}
	}
}
