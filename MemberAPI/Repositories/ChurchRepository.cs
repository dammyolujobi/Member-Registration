using Dapper;
using Npgsql;
using MemberAPI.Models;

 namespace MemberAPI.Repositories;

 public class ChurchRepository
{
    private readonly string _connectionString;

    public ChurchRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    private NpgsqlConnection GetConnection() => 
        new NpgsqlConnection(_connectionString);

    public async Task<IEnumerable<Church>> GetAllAsync()
    {
        using var conn = GetConnection();
        return await conn.QueryAsync<Church>(
            "SELECT id as Id,name as Name,location as Location from church"
        );
    }

    public async Task<Church?> GetByIdAsync(int id)
    {
        using var conn = GetConnection();
        return await conn.QueryFirstOrDefaultAsync<Church>(
            "SELECT id as Id, name as Name,location as Location WHERE id = @Id",
            new{id = id}
        );
    }

    public async Task<Church> CreateAsync(Church church)
    {
        using var conn = GetConnection();

        var id = await conn.ExecuteScalarAsync<int>(
            @"INSERT INTO church(name,location,password,phone_no)
            VALUES(@Names,@Location,@Password,@PhoneNumber)
            RETURNING id",
            church
        );

        church.Id = id;
        return church;
    } 

    public async Task<bool> UpdateAsync(Church church)
    {
        using var conn = GetConnection();

        var rows = await conn.ExecuteAsync(
            "@UPDATE church SET name = @Name, location = @Location WHERE id = @id",
            church
        );
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var conn = GetConnection();
        var rows = await conn.ExecuteAsync(
            "DELETE FROM church WHERE id= @Id",
            new{id = id}
        );
        return rows >0;
    }

    public async Task<bool> GetPhoneNumber(string phoneNumber)
    {
        using var conn = GetConnection();

        var result = await conn.QueryFirstOrDefaultAsync<string>(
            "SELECT phone_no as PhoneNumber WHERE phone_no = @PhoneNumber",
            new {phoneNo = phoneNumber}
        );

        return result!=null;
    }
    public async Task<bool> GetPassword(string password)
    {
        using var conn = GetConnection();

        var result = await conn.QueryFirstOrDefaultAsync<string>(
            "SELECT password as Password WHERE password = @Password",
            new {password = password}
        );

        return result!=null;
    }
}