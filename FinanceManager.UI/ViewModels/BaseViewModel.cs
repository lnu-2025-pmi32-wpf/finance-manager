// <copyright file="BaseViewModel.cs" company="LNU">
// Copyright (c) LNU. All rights reserved.
// </copyright>

namespace FinanceManager.UI.ViewModels
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void Raise([CallerMemberName] string name = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
