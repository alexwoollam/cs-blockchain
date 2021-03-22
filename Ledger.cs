using System;
using System.IO;
using System.Text.Json;

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
            Console.WriteLine( data );//returns {}, empty object?
            Console.WriteLine( block );///returns namespace/classname?
        }

        public int chain_size()
        {
            return 0;
        }

        public string last_block()
        {
            return "abcdefg";
        }
    }
}
