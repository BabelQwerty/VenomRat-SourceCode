using System;
using System.Collections.Generic;
using MessagePackLib.MessagePack;

namespace Server.Helper;

public class AsyncTask
{
	public MsgPack msgPack;

	public string id;

	public List<string> doneClient;

	public string title { get; set; }

	public string cnt => $"{doneClient.Count}";

	public AsyncTask(MsgPack _msgPack, string _title)
	{
		msgPack = _msgPack;
		id = Guid.NewGuid().ToString();
		doneClient = new List<string>();
		title = _title;
	}
}
