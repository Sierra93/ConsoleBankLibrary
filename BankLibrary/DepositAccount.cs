using System;
using System.Collections.Generic;
using System.Text;

namespace BankLibrary {
    public class DepositAccount : Account {
        public DepositAccount(decimal sum, int percentage) : base(sum, percentage) { }
        protected internal override void Open() {
            base.OnOpened(new AccountEventArgs("Открыт новый депозитный счет! Id счета " + this._id, this._sum));
        }
        public override void Put(decimal sum) {
            //  Проверяем прошло ли 30 дней с момента открытия счета чтобы положить деньги на счет
            if (_days % 30 == 0) {
                base.Put(sum);
            }
            else {
                base.OnAdded(new AccountEventArgs("На счет можно положить только после 30-ти дневного периода", 0));
            }
        }
        public override decimal Withdraw(decimal sum) {
            // Проверяем, прошло ли 30 дней с момента открытия счета прежде чем можно снять деньги со счета
            if (_days % 30 == 0) {
                return base.Withdraw(sum);
            }
            else {
                base.OnWithdrawed(new AccountEventArgs("Вывести деньги можно только спустя 30-ти дней", 0));
            }
            return 0;
        }
        protected internal override void Calculate() {
            // Если 30 дней с момента открытия счета прошло, то начисляем проценты
            if (_days % 30 == 0) {
                base.Calculate();
            }
        }
    }
}
