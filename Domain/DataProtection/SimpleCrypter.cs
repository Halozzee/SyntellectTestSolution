using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Domain.DataProtection
{
    public class SimpleCrypter : ITextCrypter
	{
        public string Crypt(string text)
        {
            return EncryptDecrypt(text, 256);
        }

        public string Decrypt(string text)
        {
            return EncryptDecrypt(text, 256);
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
