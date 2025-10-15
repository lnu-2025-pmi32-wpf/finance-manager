
using System;
using System.Linq;
using FinanceManager.Enums;
using FinanceManager.Models;

namespace FinanceManager.Data;

public static class DataSeeder
{
    public static void Seed(AppDbContext context)
    {
        // Перевіряємо, чи є вже дані в базі
        if (context.Transactions.Any())
        {
            return; // База вже заповнена
        }

        var transactions = new[]
        {
            new Transaction { Description = "Зарплата", Amount = 50000, Date = DateTime.Now.AddDays(-10), Type = TransactionType.Income },
            new Transaction { Description = "Оренда квартири", Amount = 12000, Date = DateTime.Now.AddDays(-9), Type = TransactionType.Expense },
            new Transaction { Description = "Продукти в супермаркеті", Amount = 1500, Date = DateTime.Now.AddDays(-8), Type = TransactionType.Expense },
            new Transaction { Description = "Фріланс проект", Amount = 7500, Date = DateTime.Now.AddDays(-5), Type = TransactionType.Income },
            new Transaction { Description = "Кафе з друзями", Amount = 800, Date = DateTime.Now.AddDays(-3), Type = TransactionType.Expense },
            new Transaction { Description = "Поповнення мобільного", Amount = 250, Date = DateTime.Now.AddDays(-1), Type = TransactionType.Expense },
        };

        context.Transactions.AddRange(transactions);
        context.SaveChanges();
    }
}
