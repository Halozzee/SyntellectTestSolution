using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DataProtection.Interfaces
{
	public interface ITextCrypter
	{
		public string Crypt(string text);
		public string Decrypt(string text);
	}
}
