using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DataProtection
{
	public class EmptyCrypter : ITextCrypter
	{
		public string Crypt(string text)
		{
			return text;
		}

		public string Decrypt(string text)
		{
			return text;
		}
	}
}
