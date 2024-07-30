namespace RestItla.Application.Interfaces.Repositories
{
    public interface IRelationShipRepository<TRelation>
    where TRelation : class
    {
        public Task<ICollection<TRelation>> Create(TRelation[] relations);
        public Task Remove(TRelation[] relations);
    }
}