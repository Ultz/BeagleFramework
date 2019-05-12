namespace Ultz.BeagleFramework.Core
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
