using System;
using System.Collections.Generic;
using System.Text;

namespace BankLibrary {
    public abstract class Account : IAccount {
        // Событие при выводе денег
        protected internal event AccountStateHandler Withdrawed;
        // Событие при добавлении денег на счет
        protected internal event AccountStateHandler Added;
        // Событие при открытии счета
        protected internal event AccountStateHandler Opened;
        // Событие при закрытии счета
        protected internal event AccountStateHandler Closed;
        // Событие при начислении процентов
        protected internal event AccountStateHandler Calculated;
        protected int _id; 
        static int counter = 0;
        protected decimal _sum;    // Хранение суммы
        protected int _percentage;     // Хранение процента
        protected int _days = 0;   // Время с открытия счета
        public Account(decimal sum, int percentage) {
            this._sum = sum;
            this._percentage = percentage;
            this._id = ++counter;
        }
        // Текущая сумма на счету
        public decimal CurrentSum {
            get { return _sum; }
        }
        public int Percentage {
            get { return _percentage; }
        }
        public int Id { 
            get { return _id; }
        }
        // Вызов событий 
        private void CallEvent(AccountEventArgs e, AccountStateHandler handler) {
            if (handler != null && e != null) {
                handler(this, e);
            }
        }
        // Вызов отдельных событий. Для каждого события определим свой виртуальный метод
        // Открытие счета
        protected virtual void OnOpened(AccountEventArgs e) {
            CallEvent(e, Opened);
        }
        // Вывод денег
        protected virtual void OnWithdrawed(AccountEventArgs e) {
            CallEvent(e, Withdrawed);
        }
        // Добавление денег на счет
        protected virtual void OnAdded(AccountEventArgs e) {
            CallEvent(e, Added);
        }
        // Закрытие счета
        protected virtual void OnClosed(AccountEventArgs e) {
            CallEvent(e, Closed);
        }
        // Начисление процентов
        protected virtual void OnCalculated(AccountEventArgs e) {
            CallEvent(e, Calculated);
        }
        // Поступление денег на счет
        public virtual void Put(decimal sum) {
            _sum += sum;
            OnAdded(new AccountEventArgs("На счет поступило" + sum, sum));
        }
        // Взять деньги со счета
        public virtual decimal Withdraw(decimal sum) {
            decimal result = 0;
            if (sum <= _sum) {
                _sum = sum;
                result = sum;
                OnWithdrawed(new AccountEventArgs("Сумма " + sum + "снята со счета " + _id, sum));
            }
            else {
                OnWithdrawed(new AccountEventArgs("Недостаточно денег на счете " + _id, 0));
            }
            return result;
        }
        // Открытие счета
        protected internal virtual void Open() {
            OnOpened(new AccountEventArgs("Открыт новый счет! Id счета " + this._id, this._sum));
        }
        // Закрытие счета
        protected internal virtual void Close() {
            OnClosed(new AccountEventArgs("Счет " + _id + " закрыт! Итоговая сумма: " + CurrentSum, CurrentSum));
        }
        // Начисление процентов
        protected internal void IncrementDays() {
            _days++;
        }
        protected internal virtual void Calculate() {
            // Начисляем
            decimal increment = _sum * _percentage / 100;
            _sum = _sum + increment;
            OnCalculated(new AccountEventArgs("Начислены проценты в размере: " + increment, increment));
        }
    }
}
