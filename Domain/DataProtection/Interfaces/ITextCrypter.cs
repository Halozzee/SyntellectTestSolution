using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DataProtection.Interfaces
{
	/// <summary>
	/// Интерфейс, представляющий "сервис" шифрования.
	/// </summary>
	public interface ITextCrypter
	{
		/// <summary>
		/// Зашифровать текст.
		/// </summary>
		/// <param name="text">Любой текст.</param>
		/// <returns>Зашифрованное значение.</returns>
		public string Crypt(string text);

		/// <summary>
		/// Расшифровать текст.
		/// </summary>
		/// <param name="text">Зашифрованное значение.</param>
		/// <returns>Расшифрованный текст.</returns>
		public string Decrypt(string text);
	}
}
