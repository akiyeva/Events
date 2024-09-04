using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Events
{
    internal class MainAccount : BankAccount
    {
        public BankAccount SavingAccount { get; set; }

        public MainAccount(decimal balance) : base(balance)
        {

        }

        public void Withdraw(decimal amount)
        {
            if (Balance + SavingAccount.Balance >= amount)
            {
                if (amount > Balance)
                {
                    decimal sub = amount - Balance;
                    Balance = 0;
                    SavingAccount.Balance -= sub;
                }
                else
                {
                    Balance -= amount;
                }
            }
            else
            {
                base.OnOverdrawnEventHandler(amount - Balance - SavingAccount.Balance);
            }
        }
    }

    internal class SavingAccount : BankAccount
    {
        public SavingAccount(decimal balance) : base(balance)
        {
        }

        public void Withdraw(decimal amount)
        {
            if (Balance >= amount)
            {
                Balance -= amount;
            }
            else
            {
                OnOverdrawnEventHandler(amount - Balance);
            }
        }
    }


    internal class BankAccount
    {
        public EventHandler OverdrawnEventHandler;

        public decimal Balance { get; set; }

        public BankAccount(decimal balance)
        {
            Balance = balance;
            OverdrawnEventHandler = new EventHandler(OverdrawnEventMethod);
        }

        public virtual void OnOverdrawnEventHandler(decimal subAmount)
        {
            if (OverdrawnEventHandler != null)
                OverdrawnEventHandler.Invoke(this, new OverdrawnEventArgs(subAmount));
        }

        public void Deposit(decimal amount)
        {
            Balance += amount;
        }

        private void OverdrawnEventMethod(object sender, EventArgs e)
        {
            OverdrawnEventArgs args = (OverdrawnEventArgs)e;

            if (sender is MainAccount bankAccount)
            {
                MessageBox.Show("Main Account: Balansinda pul yoxdur " + args.SubAmount +
                                ". Current balance: " + (bankAccount.Balance + bankAccount.SavingAccount.Balance));
            }
            else if (sender is SavingAccount savingAccount)
            {
                MessageBox.Show("Saving Account: Balansinda pul yoxdur " + args.SubAmount +
                                ". Current balance: " + savingAccount.Balance);
            }
        }


        public decimal GetBalance() => Balance;
    }

    internal class OverdrawnEventArgs : EventArgs
    {
        public OverdrawnEventArgs(decimal subAmount)
        {
            SubAmount = subAmount;
        }

        public decimal SubAmount { get; set; }
    }
}
