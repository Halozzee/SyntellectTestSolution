using Domain.DataProtection.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DataProtection.Implementations
{
	/// <summary>
	/// Пустой шифровальщик, который нужен для корректной работы DI. Ничего не шифрует.
	/// </summary>
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
