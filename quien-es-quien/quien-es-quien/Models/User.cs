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

        public User(long bitcoins, string username, int score, int bestscore) {
            _bitcoins = bitcoins;
            _username = username;
            _score = score;
            _bestscore = bestscore;
        }

        public long Bitcoins { get => _bitcoins; set => _bitcoins = value; }
        public string Username { get => _username; }
        public int Score { get => _score; set => _score = value; }
        public int Bestscore { get => _bestscore; set => _bestscore = value; }
    }
}