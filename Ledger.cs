using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Linq;
using System.Security.Cryptography;  

namespace cs_blockchain
{
    class Ledger
    {

        public string ledger;
        static readonly string PasswordHash = ConfigurationManager.AppSetting["PasswordHash"];
        static readonly string SaltKey = ConfigurationManager.AppSetting["SaltKey"];
        static readonly string VIKey = ConfigurationManager.AppSetting["VIKey"];

        public Ledger()
        {            
            ledger = @"ledger.store";
            if( ! File.Exists(ledger) ){
                this.block_zero();
            }
        }

        public void block_zero()
        {
            DateTime date = new DateTime(1986, 1, 1);
            Block zero = new Block( 0, date.ToString(), "Genisis", "0000000" );
            
            string data = JsonSerializer.Serialize( zero );
            using (FileStream fs = File.Create(ledger))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(data);
                fs.Write(info, 0, info.Length);
            }
        }

        public void write_new_block( Block block )
        {
            string data = JsonSerializer.Serialize( block );

            try
            {
                File.AppendAllText(ledger, Environment.NewLine + data);
            } 
            
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }

        public int chain_size()
        {
            int size = File.ReadLines(ledger).Count();
            return size;
        }

        public string last_block()
        {
            var last_line = File.ReadLines(ledger).Last();
            string hash = Encrypt(last_line);
            return hash;
        }

        public string Encrypt(string plainText)
		{
			byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

			byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
			var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
			var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
			
			byte[] cipherTextBytes;

			using (var memoryStream = new MemoryStream())
			{
				using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
				{
					cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
					cryptoStream.FlushFinalBlock();
					cipherTextBytes = memoryStream.ToArray();
					cryptoStream.Close();
				}
				memoryStream.Close();
			}
			return Convert.ToBase64String(cipherTextBytes);
		}

        public string Decrypt(string encryptedText)
		{
			byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
			byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
			var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

			var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
			var memoryStream = new MemoryStream(cipherTextBytes);
			var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
			byte[] plainTextBytes = new byte[cipherTextBytes.Length];

			int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
			memoryStream.Close();
			cryptoStream.Close();
			return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
		}
    }
}
