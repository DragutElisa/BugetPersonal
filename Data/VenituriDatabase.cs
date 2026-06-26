using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BugetPersonal.Models;

namespace BugetPersonal.Data
{
    public class VenituriDatabase
    {
        readonly SQLiteAsyncConnection database;
        public VenituriDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Venit>().Wait();
        }
        public Task<List<Venit>> GetVenituriAsync()
        {
            return database.Table<Venit>().ToListAsync();
        }
        public Task<Venit> GetVenitAsync(int id)
        {
            return database.Table<Venit>()
                            .Where(i => i.Id == id)
                            .FirstOrDefaultAsync();
        }
        public Task<int> SaveVenitAsync(Venit venit)
        {
            if (venit.Id != 0)
            {
                return database.UpdateAsync(venit);
            }
            else
            {
                return database.InsertAsync(venit);
            }
        }
        public Task<int> DeleteVenitAsync(Venit venit)
        {
            return database.DeleteAsync(venit);
        }
    }
}

