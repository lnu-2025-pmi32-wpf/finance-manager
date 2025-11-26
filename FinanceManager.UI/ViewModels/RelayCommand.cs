// <copyright file="RelayCommand.cs" company="LNU">
// Copyright (c) LNU. All rights reserved.
// </copyright>

namespace FinanceManager.UI.ViewModels
{
    using System;
    using System.Windows.Input;

    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => this._canExecute?.Invoke(parameter) ?? true;

        public void Execute(object? parameter) => this._execute(parameter);

        public event EventHandler? CanExecuteChanged;

        public void RaiseCanExecuteChanged() => this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
