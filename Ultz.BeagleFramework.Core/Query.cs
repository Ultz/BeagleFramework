namespace Ultz.BeagleFramework.Core.Structure
{
    public class Query
    {
        public Query(params Clause[] clauses)
        {
            Clauses = clauses;
        }

        public Clause[] Clauses { get; }
    }
}
