using System.Data;
using Awarean.Airline.Domain;

namespace Awarean.Airline.Infrastructure.Dapper;

public class DomainTransaction : IDomainTransaction, IDisposable
{
    private IDbConnection connection;
    private IDbTransaction? transaction;
    private bool disposedValue;

    public DomainTransaction(IDbConnection connection)
        => this.connection = connection ?? throw new ArgumentNullException(nameof(connection));

    public IDbTransaction Context
    {
        get
        {
            Start();
            return transaction;
        }
    }

    public void Start()
    {
        if (transaction is null)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();

            transaction = connection.BeginTransaction();
        }
    }

    public void Commit()
    {
        if (transaction is null)
            throw new InvalidOperationException("Cannot commit a transaction before starting.");

        transaction.Commit();
        DisposeTransaction();
    }

    public void Rollback()
    {
        if (transaction is null)
            throw new InvalidOperationException("Cannot Rollback a transaction before starting.");

        transaction.Rollback();
        DisposeTransaction();
    }

    private void DisposeTransaction()
    {
        transaction?.Dispose();
        transaction = null;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                DisposeTransaction();
                connection.Dispose();
            }
            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            connection = null;
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}