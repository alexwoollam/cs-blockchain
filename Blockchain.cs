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
            
        }



    }
}
