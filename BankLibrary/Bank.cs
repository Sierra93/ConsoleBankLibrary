using System;
using System.Collections.Generic;
using System.Text;

namespace BankLibrary {
    // Этот класс служит обобщением дял всех счетов, так как счета существуют не сами по себе а в банке
    public class Bank<T> where T : Account {
        T[] accounts;
        public string Name { get; private set; }
        public Bank(string name) {
            this.Name = name;
        }       
    }
}
