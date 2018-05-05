namespace Ultz.BeagleFramework
{
    public class Column
    {
        public Column(int index, string name)
        {
            Index = index;
            Name = name;
        }

        public int Index { get; }
        public string Name { get; }
    }
}