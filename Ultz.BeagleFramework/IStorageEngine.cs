using System;
using System.Collections.Generic;

namespace Ultz.BeagleFramework
{
    public interface IStorageEngine : IDisposable
    {
        string Id { get; }
        void Initialize(string initializationParameters);
        ITable GetTable(string name);
        Column GetColumn(string table, int col);
        Column GetColumn(string table, string col);
        ITable CreateTable(string table, string[] cols);
        void DeleteTable(string table);
    }
}