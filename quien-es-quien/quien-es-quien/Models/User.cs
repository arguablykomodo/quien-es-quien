using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace quien_es_quien.Models {
    public class User {
        private long _bitcoins;
        private string _username;
        private int _score;
        private int _bestscore;
        private bool _admin;

        public User(long bitcoins, string username, int score, int bestscore, bool admin) {
            _bitcoins = bitcoins;
            _username = username;
            _score = score;
            _bestscore = bestscore;
            _admin = admin;
        }

        public long Bitcoins { get => _bitcoins; set => _bitcoins = value; }
        public string Username { get => _username; }
        public int Score { get => _score; set => _score = value; }
        public int Bestscore { get => _bestscore; set => _bestscore = value; }
        public bool Admin { get => _admin;}

        public void UpdateBitcoins(long bitcoins) {
            if (this.Bitcoins - bitcoins < 0 && this.Bitcoins < 0) {
                this.Bitcoins = 0;
            }
            DaB db = new DaB();
            db.UpdateBitcoins(this, bitcoins);
            db.Disconnect();
        }
    }
}