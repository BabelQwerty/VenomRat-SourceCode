using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace Server.Helper;

public class WordTextBox : TextEdit
{
	public enum WordType
	{
		DWORD,
		QWORD
	}

	private bool isHexNumber;

	private WordType type;

	private IContainer components;

	public int MaxLength
	{
		get
		{
			return base.Properties.MaxLength;
		}
		set
		{
			base.Properties.MaxLength = value;
		}
	}

	public bool IsHexNumber
	{
		get
		{
			return isHexNumber;
		}
		set
		{
			if (isHexNumber == value)
			{
				return;
			}
			if (value)
			{
				if (Type == WordType.DWORD)
				{
					Text = UIntValue.ToString("x");
				}
				else
				{
					Text = ULongValue.ToString("x");
				}
			}
			else if (Type == WordType.DWORD)
			{
				Text = UIntValue.ToString();
			}
			else
			{
				Text = ULongValue.ToString();
			}
			isHexNumber = value;
			UpdateMaxLength();
		}
	}

	public WordType Type
	{
		get
		{
			return type;
		}
		set
		{
			if (type != value)
			{
				type = value;
				UpdateMaxLength();
			}
		}
	}

	public uint UIntValue
	{
		get
		{
			try
			{
				if (string.IsNullOrEmpty(Text))
				{
					return 0u;
				}
				if (IsHexNumber)
				{
					return uint.Parse(Text, NumberStyles.HexNumber);
				}
				return uint.Parse(Text);
			}
			catch (Exception)
			{
				return uint.MaxValue;
			}
		}
	}

	public ulong ULongValue
	{
		get
		{
			try
			{
				if (string.IsNullOrEmpty(Text))
				{
					return 0uL;
				}
				if (IsHexNumber)
				{
					return ulong.Parse(Text, NumberStyles.HexNumber);
				}
				return ulong.Parse(Text);
			}
			catch (Exception)
			{
				return ulong.MaxValue;
			}
		}
	}

	public bool IsConversionValid()
	{
		if (string.IsNullOrEmpty(Text))
		{
			return true;
		}
		if (!IsHexNumber)
		{
			return ConvertToHex();
		}
		return true;
	}

	public WordTextBox()
	{
		InitializeComponent();
		MaxLength = 8;
	}

	protected override void OnKeyPress(KeyPressEventArgs e)
	{
		base.OnKeyPress(e);
		e.Handled = !IsValidChar(e.KeyChar);
	}

	private bool IsValidChar(char ch)
	{
		if (!char.IsControl(ch) && !char.IsDigit(ch))
		{
			if (IsHexNumber && char.IsLetter(ch))
			{
				return char.ToLower(ch) <= 'f';
			}
			return false;
		}
		return true;
	}

	private void UpdateMaxLength()
	{
		if (Type == WordType.DWORD)
		{
			if (IsHexNumber)
			{
				MaxLength = 8;
			}
			else
			{
				MaxLength = 10;
			}
		}
		else if (IsHexNumber)
		{
			MaxLength = 16;
		}
		else
		{
			MaxLength = 20;
		}
	}

	private bool ConvertToHex()
	{
		try
		{
			if (Type == WordType.DWORD)
			{
				uint.Parse(Text);
			}
			else
			{
				ulong.Parse(Text);
			}
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
	}
}
