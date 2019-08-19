using System;
using System.Collections.Generic;
using System.Text;

namespace BankLibrary {
    // Библиотека классов, которую будет использовать главный проект Program
    public interface IAccount {
        // Положить деньги на счет
        void Put(decimal sum);
        // Взять деньги со счета
        decimal Withdraw(decimal sum);
    }
}
