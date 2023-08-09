using System.Data;

namespace Awarean.Airline.Domain;

public interface IDomainTransaction : IDisposable
{
    IDbTransaction Context { get; }

    void Start();

    void Commit();

    void Rollback();
}