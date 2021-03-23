using System;

namespace cs_blockchain
{
    class Blockchain
    {
        public int current_chain_length;
        public string previous_hash;



        public Blockchain()
        {
            Ledger ledger = new Ledger();
            current_chain_length = ledger.chain_size();
            previous_hash = ledger.last_block();
            new_block();
        }

        public void new_block()
        {
            string date = DateTime.Now.ToString("yyyy/MM/dd");
            var block = new Block( current_chain_length, date, "some data", previous_hash );
            var ledger = new Ledger();
            ledger.write_new_block( block );
            string decryption = ledger.Decrypt(previous_hash);
            Console.WriteLine( decryption );
        }

    }
}
