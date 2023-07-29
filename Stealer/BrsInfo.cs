using System.Collections.Generic;

namespace Stealer;

public class BrsInfo
{
	public List<Password> listps { get; set; } = new List<Password>();


	public List<Cookie> listcookie { get; set; } = new List<Cookie>();


	public List<Site> listhist { get; set; } = new List<Site>();


	public List<Bookmark> listbmark { get; set; } = new List<Bookmark>();


	public List<AutoFill> listautofill { get; set; } = new List<AutoFill>();


	public List<CreditCard> listcredit { get; set; } = new List<CreditCard>();

}
