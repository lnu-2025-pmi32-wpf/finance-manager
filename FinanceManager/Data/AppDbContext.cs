using FinanceManager.Enums;
using FinanceManager.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<FinancialProfile> FinancialProfiles { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Goal> Goals { get; set; }
    public DbSet<ScheduledPayment> ScheduledPayments { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Account
        modelBuilder.Entity<Account>()
            .Property(a => a.Balance)
            .HasColumnType("decimal(18, 2)")
            .HasDefaultValue(0.00m);

        // Category
        modelBuilder.Entity<Category>()
            .Property(c => c.Type)
            .HasConversion<string>();

        // Goal
        modelBuilder.Entity<Goal>()
            .Property(g => g.TargetAmount)
            .HasColumnType("decimal(18, 2)");
        
        modelBuilder.Entity<Goal>()
            .Property(g => g.CurrentAmount)
            .HasColumnType("decimal(18, 2)")
            .HasDefaultValue(0.00m);

        // ScheduledPayment
        modelBuilder.Entity<ScheduledPayment>()
            .Property(p => p.Amount)
            .HasColumnType("decimal(18, 2)");
            
        modelBuilder.Entity<ScheduledPayment>()
            .Property(p => p.Frequency)
            .HasConversion<string>();

        // Transaction
        modelBuilder.Entity<Transaction>()
            .Property(t => t.Amount)
            .HasColumnType("decimal(18, 2)");

        modelBuilder.Entity<Transaction>()
            .Property(t => t.TransactionDateTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
            
        // Relationships
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Category)
            .WithMany()
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<ScheduledPayment>()
            .HasOne(p => p.Category)
            .WithMany()
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);
            
        // Many-to-Many: Transaction and Tag
        modelBuilder.Entity<Transaction>()
            .HasMany(t => t.Tags)
            .WithMany(t => t.Transactions)
            .UsingEntity(j => j.ToTable("TransactionTags"));
    }
}