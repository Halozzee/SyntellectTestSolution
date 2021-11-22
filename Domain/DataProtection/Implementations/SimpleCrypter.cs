using Domain.DataProtection.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Domain.DataProtection.Implementations
{
	/// <summary>
	/// Простой шифровальщик. Работает с помощью XOR сдвига.
	/// </summary>
	public class SimpleCrypter : ITextCrypter
	{
		private readonly int _shift = 256;
		public string Crypt(string text)
		{
			return EncryptDecrypt(text, _shift);
		}

		public string Decrypt(string text)
		{
			return EncryptDecrypt(text, _shift);
		}

		private string EncryptDecrypt(string szPlainText, int szEncryptionKey)
		{
			StringBuilder szInputStringBuild = new StringBuilder(szPlainText);
			StringBuilder szOutStringBuild = new StringBuilder(szPlainText.Length);
			char Textch;
			for (int iCount = 0; iCount < szPlainText.Length; iCount++)
			{
				Textch = szInputStringBuild[iCount];
				Textch = (char)(Textch ^ szEncryptionKey);
				szOutStringBuild.Append(Textch);
			}
			return szOutStringBuild.ToString();
		}
	}
}
