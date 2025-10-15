using System;
using System.Linq;
using FinanceManager.Enums;
using FinanceManager.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Data;

public static class DataSeeder
{
    public static void Seed(AppDbContext context)
    {
        // Переконуємось, що база даних створена
        context.Database.EnsureCreated();

        // Перевіряємо, чи є вже користувачі
        if (context.Users.Any())
        {
            return; // База вже заповнена
        }

        // 1. Створюємо валюти
        var uah = new Currency { CurrencyCode = "UAH", UsdExchangeRate = 40.5m };
        var usd = new Currency { CurrencyCode = "USD", UsdExchangeRate = 1.0m };
        var eur = new Currency { CurrencyCode = "EUR", UsdExchangeRate = 0.92m };
        context.Currencies.AddRange(uah, usd, eur);
        
        // 2. Створюємо користувача
        var user = new User
        {
            Email = "test@example.com",
            PasswordHash = "some_hash" // В реальному додатку тут має бути хеш
        };
        context.Users.Add(user);

        // 3. Створюємо фінансовий профіль
        var profile = new FinancialProfile
        {
            User = user,
            Name = "Особистий профіль",
            MainCurrencyCode = "UAH"
        };
        context.FinancialProfiles.Add(profile);

        // 4. Створюємо рахунки
        var monoAccount = new Account { FinancialProfile = profile, Name = "Карта Monobank", Balance = 15000.00m, CurrencyCode = "UAH" };
        var cashAccount = new Account { FinancialProfile = profile, Name = "Готівка", Balance = 2500.00m, CurrencyCode = "UAH" };
        context.Accounts.AddRange(monoAccount, cashAccount);

        // 5. Створюємо категорії
        var foodCategory = new Category { FinancialProfile = profile, Name = "Продукти", Type = CategoryType.Expense, Icon = "food", ColorHex = "#FFD700" };
        var transportCategory = new Category { FinancialProfile = profile, Name = "Транспорт", Type = CategoryType.Expense, Icon = "transport", ColorHex = "#4682B4" };
        var salaryCategory = new Category { FinancialProfile = profile, Name = "Зарплата", Type = CategoryType.Income, Icon = "salary", ColorHex = "#32CD32" };
        context.Categories.AddRange(foodCategory, transportCategory, salaryCategory);

        // 6. Створюємо теги
        var tagImportant = new Tag { FinancialProfile = profile, Name = "Важливо" };
        var tagTrip = new Tag { FinancialProfile = profile, Name = "Подорож" };
        context.Tags.AddRange(tagImportant, tagTrip);

        // Зберігаємо все в базу
        context.SaveChanges();
        
        // 7. Створюємо транзакції
        var transaction1 = new Transaction
        {
            Account = monoAccount,
            Category = salaryCategory,
            Amount = 25000,
            TransactionDateTime = DateTime.UtcNow.AddDays(-10),
            Description = "Аванс"
        };
        var transaction2 = new Transaction
        {
            Account = monoAccount,
            Category = foodCategory,
            Amount = -750,
            TransactionDateTime = DateTime.UtcNow.AddDays(-5),
            Description = "Сільпо",
            Tags = new[] { tagImportant }
        };
        var transaction3 = new Transaction
        {
            Account = cashAccount,
            Category = transportCategory,
            Amount = -50,
            TransactionDateTime = DateTime.UtcNow.AddDays(-2),
            Description = "Метро"
        };
        context.Transactions.AddRange(transaction1, transaction2, transaction3);
        
        context.SaveChanges();
    }
}