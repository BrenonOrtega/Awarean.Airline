namespace Awarean.Airline.Domain;

public interface IDomainTransaction
{
    void Start();

    void Commit();

    void Rollback();
}