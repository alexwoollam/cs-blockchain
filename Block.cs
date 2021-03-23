using System;

namespace cs_blockchain
{
    class Block
    {

        public int id { get; set; }
        public string time_stamp { get; set; }
        public string data{ get; set; }
        public string last_hash{ get; set; }

        public Block( int id, string time_stamp, string data, string last_hash )
        {
            this.id = id;
            this.time_stamp = time_stamp;
            this.data = data;
            this.last_hash = last_hash; 
        }

    }
}
