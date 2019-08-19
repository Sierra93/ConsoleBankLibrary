using System;
using System.Collections.Generic;
using System.Text;

namespace BankLibrary {
    // Этот класс служит обобщением дял всех счетов, так как счета существуют не сами по себе а в банке
    public enum AccountType {
        Ordinary,
        Deposit
    }
    public class Bank<T> where T : Account {
        T[] accounts; // Массив для хранения счетов
        public string Name { get; private set; }
        // Создаем перечисление с типами счетов        
        public Bank(string name) {
            this.Name = name;
        }
        // Создаем счет
        public void Open(AccountType accountType, decimal sum, AccountStateHandler addSumHandler, AccountStateHandler withdrawSumHandler, AccountStateHandler calculationHandler, AccountStateHandler closeAccountHandler, AccountStateHandler openAccountHandler) {
            T newAccount = null;    // Обобщение для хранения счетов
            // Проверяем тип счета, и в зависимости от типа счета будем создавать объекты классов этих счетов
            switch(accountType) {
                case AccountType.Ordinary:
                    // Создаем объект класса demand и сразу приводим его к типу обобщения
                    newAccount = new DemandAccount(sum, 1) as T;
                    break;
                case AccountType.Deposit:
                    // Создаем объект класса deposit и сразу приводим его к типу обобщения
                    newAccount = new DepositAccount(sum, 40) as T;
                    break;
            }
            if (newAccount == null) {
                throw new Exception("Ошибка создания счета");
            }
            // Добавляем новый счет в массив счетов
            if (accounts == null) {
                accounts = new T[] { newAccount };
            }
            else {
                T[] tempAccounts = new T[accounts.Length + 1];
                for (int i = 0; i < accounts.Length; i++) {
                    tempAccounts[i] = accounts[i];
                }
                tempAccounts[tempAccounts.Length - 1] = newAccount;
                accounts = tempAccounts;
            }
            // Установка обработчиков событий счета
            newAccount.Added += addSumHandler;
            newAccount.Withdrawed += withdrawSumHandler;            
            newAccount.Closed += closeAccountHandler;
            newAccount.Opened += openAccountHandler;
            newAccount.Calculated += calculationHandler;
            newAccount.Open();
        }
        // Поиск счета по id
        public T FindAccount(int id) {
            for (int i = 0; i < accounts.Length; i++) {
                if (accounts[i].Id == id) {
                    return accounts[i];
                }
            }
            return null;
        }
        // Перегруженная версия поиска счета
        public T FindAccount(int id, out int index) {
            for (int i = 0; i < accounts.Length; i++) {
                if (accounts[i].Id == id) {
                    index = i;
                    return accounts[i];
                }
            }
            index = -1;
            return null;
        }
        // Добавляем средства на счет
        public void Put(decimal sum, int id) {
            T account = FindAccount(id);
            if (account == null) {
                throw new Exception("Счет не найден");
            }
            account.Put(sum);
        }
        // Выводим средства
        public void Withdraw(decimal sum, int id) {
            T account = FindAccount(id);
            if (account == null) {
                throw new Exception("Счет не найден");
            }
            account.Withdraw(sum);
        }
        // Закрытие счета
        public void Close(int id) {
            int index;
            T account = FindAccount(id, out index);
            if (account == null) {
                throw new Exception("Счет не найден");
            }
            account.Close();
            if (accounts.Length <= 1) {
                accounts = null;
            }
            else {
                // Уменьшаем массив счетов, удаляя из него закрытый счет
                T[] tempAccounts = new T[accounts.Length - 1];
                for (int i = 0, j = 0; i < accounts.Length; i++) {
                    if (i != index) {
                        tempAccounts[j++] = accounts[i];
                    }
                    accounts = tempAccounts;
                }
            }
        }
        // Начисление процентов по счетам
        public void CalculatePercentage() {
            // Если массив не содан, то выходим из метода
            if (accounts == null) { return; }            
            for (int i = 0; i < accounts.Length; i++) {
                T account = accounts[i];
                account.IncrementDays();
                account.Calculate();
            }
        }
    }
}
