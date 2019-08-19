using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace BankLibrary {
    // Событийная модель, которая используется для реакции изменения методов через делегаты
    public delegate void AccountStateHandler(object sender, AccountEventArgs e);    // Делегат, который используется для создания событий класса AccountEnentArgs
    public class AccountEventArgs { 
        // Сообщение 
        public string message { get; private set; }
        // Сумма, на которую изменился счет
        public decimal sum { get; private set; } 
        // Конструктор для инициализации полей класса
        public AccountEventArgs(string _message, decimal _sum) {
            this.message = _message; 
            this.sum = _sum; 
        }
    }
}
