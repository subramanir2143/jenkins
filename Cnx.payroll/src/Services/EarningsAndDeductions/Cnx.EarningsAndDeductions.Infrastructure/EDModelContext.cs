using Cnx.EarningsAndDeductions.Domain.Model;
using Cnx.EarningsAndDeductions.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cnx.EarningsAndDeductions.Infrastructure
{
    public class EDModelContext : DbContext, IUnitOfWork
    {
        private IDbContextTransaction _currentTransaction;

        public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

        public bool HasActiveTransaction => _currentTransaction != null;

        public EDModelContext()
        {

        }
        public EDModelContext(DbContextOptions<EDModelContext> options)
            : base(options)
        { }

        public DbSet<EDModel> EDModel { get; set; }

        public DbSet<DuplicateEDModel> DuplicateEDModel { get; set; }

        public DbSet<PayCodeModel> PayCodeModel { get; set; }

        public DbSet<PayPeriodModel> PayPeriodModel { get; set; }

        public DbSet<EmployeeDataModel> EmployeeDataModel { get; set; }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {

            // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
            // performed through the DbContext will be committed
            var result = await base.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null) return null;

            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            return _currentTransaction;
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    }

    public class EDModelContextDesignFactory : IDesignTimeDbContextFactory<EDModelContext>
    {
        public EDModelContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EDModelContext>()
                .UseMySql("Server=mysql.data;database=EarningsAndDeductions;user=root;password=P@ssw0rd");

            return new EDModelContext(optionsBuilder.Options);
        }
    }
}
