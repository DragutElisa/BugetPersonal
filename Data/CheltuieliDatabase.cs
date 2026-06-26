using BugetPersonal.Models;
using SQLite;

public class CheltuieliDatabase
{
   
    public SQLiteAsyncConnection database;

    public CheltuieliDatabase(string dbPath)
    {
        database = new SQLiteAsyncConnection(dbPath);
        database.CreateTableAsync<Cheltuieli>().Wait();
    }


    public Task<List<Cheltuieli>> GetCheltuieliAsync()
    {
        return database.Table<Cheltuieli>().ToListAsync();
    }

    // Adaugă metodele cu numele corect
    public Task<int> SaveCheltuieliAsync(Cheltuieli cheltuiala)
    {
        if (cheltuiala.Id != 0)
        {
            return database.UpdateAsync(cheltuiala);
        }
        else
        {
            return database.InsertAsync(cheltuiala);
        }
    }


    // Păstrează și metodele originale pentru compatibilitate
    public Task<int> SaveCheltuialaAsync(Cheltuieli cheltuiala)
    {

        return SaveCheltuieliAsync(cheltuiala);
    }

    public Task<int> DeleteCheltuieliByIdAsync(int id)
    {
        return database.DeleteAsync<Cheltuieli>(id);
    }
}