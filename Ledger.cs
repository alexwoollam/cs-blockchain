using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Linq;

namespace cs_blockchain
{
    class Ledger
    {

        public string ledger;

        public Ledger()
        {            
            ledger = @"ledger.txt";
            if( ! File.Exists(ledger) ){
                this.block_zero();
            }
        }

        public void block_zero()
        {
            DateTime date = new DateTime(1986, 1, 1);
            Block zero = new Block( 0, date.ToString(), "Genisis", "0000000" );
            write_new_block( zero );
        }

        public void write_new_block( Block block )
        {
            string data = JsonSerializer.Serialize( block );

            try
            {
                using (FileStream fs = File.Create(ledger))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(data);
                    fs.Write(info, 0, info.Length);
                }

                using (StreamReader sr = File.OpenText(ledger))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }
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
            Block hash = JsonSerializer.Deserialize<Block>( last_line );
            return hash.last_hash.ToString();
        }
    }
}
