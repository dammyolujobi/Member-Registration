using Dapper;
using Npgsql;
using MemberAPI.Models;


namespace MemberAPI.Repositories;

public class MemberRepository
{
    private readonly string _connectionString;

    public MemberRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    private NpgsqlConnection GetConnection() => 
        new NpgsqlConnection(_connectionString);
    
     public async Task<IEnumerable<Member>> GetAllAsync()
    {
        using var conn = GetConnection();
        return await conn.QueryAsync<Member>(
            "SELECT id as Id, first_name as FirstName,last_name as LastName, email as Email, phoneNumber as PhoneNumber, datejoined as DateJoined,is_active as IsActive FROM member" 
        );
    }
    public async Task<Member?> FindByName(string firstname)
    {
        using var conn = GetConnection();
        return await conn.QueryFirstOrDefaultAsync<Member>(
            "SELECT id as Id, first_name as FirstName,last_name as LastName, email as Email, phoneNumber as PhoneNumber, datejoined as DateJoined,is_active as IsActive FROM member WHERE first_name = @FirstName",
            new{firstname = firstname}
        );
    }
    public async Task<Member?> GetByIdAsync(int id)
    {
        using var conn = GetConnection();
        return await conn.QueryFirstOrDefaultAsync<Member>(
            "SELECT id as Id, first_name as FirstName,last_name as LastName, email as Email, phoneNumber as PhoneNumber, datejoined as DateJoined,is_active as IsActive FROM member where id = @Id",
            new {id = id}
        );
    }

    public async Task<Member> CreateAsync(Member member)
    {
        using var conn = GetConnection();

        var id = await conn.ExecuteScalarAsync<int>(
            @"INSERT INTO member(first_name,last_name,email,phoneNumber,is_active)
            VALUES(@FirstName,@LastName,@Email,@PhoneNumber,@IsActive)
            RETURNING id",
            member
        );
        member.Id = id;
        return member;
    }

    public async Task<bool> UpdateAsync(Member member)
    {
        using var conn = GetConnection();

        var rows = await conn.ExecuteAsync(
            @"UPDATE member SET first_name = @FirstName,last_name = @LastName, email = @Email, phoneNumber = @PhoneNumber,is_active = @IsActive 
            WHERE id = @id",
            member
        );
        return rows>0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var conn = GetConnection();

        var rows = await conn.ExecuteAsync(
            "DELETE FROM member WHERE id = @Id",
            new{id = id}
        );

        return rows>0;
    }
}
