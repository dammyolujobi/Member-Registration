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
            "SELECT id, FullName, email as Email, phoneNumber as PhoneNumber, datejoined as DateJoined,is_active as isActive FROM members" 
        );
    }
    public async Task<Member?> FindByName(string fullname)
    {
        using var conn = GetConnection();
        return await conn.QueryFirstOrDefaultAsync<Member>(
            "SELECT id, FullName, email as Email, phoneNumber as PhoneNumber, datejoined as DateJoined,is_active as isActive FROM members where FullName = @FullName",
            new{fullname = fullname}
        );
    }
    public async Task<Member?> GetByIdAsync(int id)
    {
        using var conn = GetConnection();
        return await conn.QueryFirstOrDefaultAsync<Member>(
            "SELECT id, FullName, email as Email, phoneNumber as PhoneNumber, datejoined as DateJoined,is_active as isActive FROM members where id = @id",
            new {id = id}
        );
    }

    public async Task<Member> CreateAsync(Member member)
    {
        using var conn = GetConnection();

        var id = await conn.ExecuteScalarAsync<int>(
            @"INSERT INTO members(FullName,email,phoneNumber)
            VALUES(@FullName,@Email,@PhoneNumber)
            RETURNING id",
            member
        );
        member.id = id;
        return member;
    }

    public async Task<bool> UpdateAsync(Member member)
    {
        using var conn = GetConnection();

        var rows = await conn.ExecuteAsync(
            @"UPDATE members SET FullName = @FullName, email = @Email, phoneNumber = @PhoneNumber,is_active = @isActive 
            WHERE id = @id",
            member
        );
        return rows>0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var conn = GetConnection();

        var rows = await conn.ExecuteAsync(
            "DELETE FROM members WHERE id = @id",
            new{id = id}
        );

        return rows>0;
    }
}
