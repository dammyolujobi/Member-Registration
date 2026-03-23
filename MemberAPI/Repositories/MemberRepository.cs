using Dapper;
using Npgsql;
using MemberAPI.Models;
using Microsoft.AspNetCore.Identity;
using System.Drawing;

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
            "SELECT id as Id, first_name as FirstName,last_name as LastName, email as Email, phoneNumber as PhoneNumber, datejoined as DateJoined,is_active as IsActive FROM members" 
        );
    }
    public async Task<Member?> FindByName(string firstname)
    {
        using var conn = GetConnection();
        return await conn.QueryFirstOrDefaultAsync<Member>(
            "SELECT id as Id, first_name as FirstName,last_name as LastName, email as Email, phoneNumber as PhoneNumber, datejoined as DateJoined,is_active as IsActive FROM members where first_name = @FirstName",
            new{firstname = firstname}
        );
    }
    public async Task<Member?> GetByIdAsync(int id)
    {
        using var conn = GetConnection();
        return await conn.QueryFirstOrDefaultAsync<Member>(
            "SELECT id as Id, first_name as FirstName,last_name as LastName, email as Email, phoneNumber as PhoneNumber, datejoined as DateJoined,is_active as IsActive FROM members where id = @Id",
            new {id = id}
        );
    }

    public async Task<Member> CreateAsync(Member member)
    {
        using var conn = GetConnection();

        var id = await conn.ExecuteScalarAsync<int>(
            @"INSERT INTO members(first_name,last_name,email,phoneNumber)
            VALUES(@FirstName,@LastName,@Email,@PhoneNumber)
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
            @"UPDATE members SET first_name = @FirstName,last_name = @LastName, email = @Email, phoneNumber = @PhoneNumber,is_active = @IsActive 
            WHERE id = @id",
            member
        );
        return rows>0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var conn = GetConnection();

        var rows = await conn.ExecuteAsync(
            "DELETE FROM members WHERE id = @Id",
            new{id = id}
        );

        return rows>0;
    }
}
