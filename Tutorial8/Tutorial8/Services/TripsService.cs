﻿using System.Data;
using Microsoft.Data.SqlClient;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public class TripsService : ITripsService
{
    private readonly string _connectionString = "Server=localhost;User ID=sa;Password=Linux2004;Encrypt=False";
    
    public async Task<List<TripDTO>> GetTrips()
    {
        var trips = new List<TripDTO>();

        string command = "SELECT IdTrip, Name,Description FROM Trip";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            await conn.OpenAsync();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    int idOrdinal = reader.GetOrdinal("IdTrip");
                    trips.Add(new TripDTO()
                    {
                        Id = reader.GetInt32(idOrdinal),
                        Name = reader.GetString(1),
                        Descr = reader.GetString(2),
                        Countries =  await GetCountries(reader.GetInt32(idOrdinal))

                    });
                }
            }
        }
        

        return trips;
    }

    public async Task<List<CountryDTO>> GetCountries(int id){
        
        var countries = new List<CountryDTO>();

        string command = "select Name from Country c , Country_Trip ct  WHERE  c.IdCountry  = ct.IdCountry and IdTrip  = @id";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.Add("@id",SqlDbType.Int).Value = id;
            await conn.OpenAsync();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    countries.Add(new CountryDTO(){Name = reader.GetString(0)});
                }
            }
        }
        return countries;

    }



    public async Task<Boolean> DoesTripExist(int id)
    {
        string command = "SELECT count(*) FROM Trip as t where exists (select * from Client_trip as c where IdClient = @id and c.IdTrip = t.IdTrip  )";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn)){
            cmd.Parameters.Add("@id",SqlDbType.Int).Value = id;
            await conn.OpenAsync();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                  return  reader.GetInt32(0) == 0;
                }
            }
        }
        
    return true;
    }


    public async Task<Boolean> DoesClientExists(int id)
    {
        string command = "select count(*) from Client_trip as c where IdClient = @id";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn)){
            cmd.Parameters.Add("@id",SqlDbType.Int).Value = id;
            await conn.OpenAsync();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                  return  reader.GetInt32(0) == 0;
                }
            }
        }
        
    return true;
    }

    //todo move to clientService??

    public async Task<String> AddClient(ClientCreateDTO clientCreateDTO)
    {
        string command = "INSERT INTO Client (FirstName, LastName, Email, Telephone, Pesel) VALUES (@fn,@ln,@em,@ph,@ps)";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn)){
            cmd.Parameters.Add("@fn",SqlDbType.VarChar).Value = clientCreateDTO.FirstName;

            cmd.Parameters.Add("@ln",SqlDbType.VarChar).Value = clientCreateDTO.LastName;

            cmd.Parameters.Add("@em",SqlDbType.VarChar).Value = clientCreateDTO.Email;

            cmd.Parameters.Add("@ph",SqlDbType.VarChar).Value = clientCreateDTO.phone;

             cmd.Parameters.Add("@ps",SqlDbType.VarChar).Value = clientCreateDTO.pesel;
            await conn.OpenAsync();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    
                }
            }
        }
        
    return "";
    }

    public async Task<int> GetClientID(ClientCreateDTO clientCreateDTO)
    {
        string command = "select IdClient from Client c where FirstName  = @fn and LastName  = @ln and Email = @em and Telephone = @ph and Pesel = @ps";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn)){
            cmd.Parameters.AddWithValue("@fn",clientCreateDTO.FirstName);

            cmd.Parameters.AddWithValue("@ln",clientCreateDTO.LastName);

            cmd.Parameters.AddWithValue("@em",clientCreateDTO.Email);

            cmd.Parameters.AddWithValue("@ph",clientCreateDTO.phone);

             cmd.Parameters.AddWithValue("@ps",clientCreateDTO.pesel);
            await conn.OpenAsync();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    return  reader.GetInt32(0);
                }
            }
        }
        
    return -1;
    }








    public async Task<List<TripDTO>> GetTrip(int id)
    {
        var trips = new List<TripDTO>();

        string command = "SELECT IdTrip, Name ,Description FROM Trip as t where exists (select * from Client_trip as c where IdClient = @id and c.IdTrip = t.IdTrip  )";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.Add("@id",SqlDbType.Int).Value = id;
            await conn.OpenAsync();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    trips.Add(new TripDTO()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Descr = reader.GetString(2),
                        Countries =  await GetCountries(reader.GetInt32(0))

                    });
                }
            }
        }
        

        return trips;
    }


     public async Task<Boolean> DoesTripExistByTripId(int id)
    {
        string command = "SELECT count(*) FROM Trip as t where t.IdTrip = @id";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn)){
            cmd.Parameters.Add("@id",SqlDbType.Int).Value = id;
            await conn.OpenAsync();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                  return  reader.GetInt32(0) == 0;
                }
            }
        }
        
    return true;
    }


    public async Task<Boolean> DoesTripExistByTripIdAndClientId(int id,int tripid)
    {
        string command = "SELECT count(*) FROM Client_Trip as t where t.IdTrip = @id and t.IdClient = @cid";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn)){
            cmd.Parameters.Add("@id",SqlDbType.Int).Value = id;

            cmd.Parameters.Add("@cid",SqlDbType.Int).Value = id;
            await conn.OpenAsync();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                  return  reader.GetInt32(0) == 0;
                }
            }
        }
        
    return true;
    }

    public async Task<Boolean> DoesTripFullByTripId(int id)
    {
        string command = "SELECT count(*) FROM Trip as t where t.MaxPeople > (select count(*) from Client_Trip ct  where t.IdTrip = ct.IdTrip) and t.IdTrip = @id";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn)){
            cmd.Parameters.Add("@id",SqlDbType.Int).Value = id;
            await conn.OpenAsync();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                  return  reader.GetInt32(0) == 0;
                }
            }
        }
        
    return true;
    }

    public async Task AddTrip(int id,int tripid)
    {
        string command = "INSERT INTO Client_Trip (IdClient, IdTrip, RegisteredAt, PaymentDate) VALUES (@id,@tid,@dt,null)";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn)){
           
            cmd.Parameters.Add("@id",SqlDbType.Int).Value = id;
            cmd.Parameters.Add("@tid",SqlDbType.Int).Value = tripid;
            cmd.Parameters.Add("@dt",SqlDbType.Int).Value = DateTime.Now.Year *10000+DateTime.Now.Month*100+DateTime.Now.Day;

            await conn.OpenAsync();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                await reader.ReadAsync();

            }
        }
        
    }

    public async Task<Boolean> DeletetByTripIdAndClientId(int id,int tripid)
    {
        string command = "Delete FROM Client_Trip where IdTrip = @id and IdClient = @cid";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn)){
            cmd.Parameters.Add("@id",SqlDbType.Int).Value = id;

            cmd.Parameters.Add("@cid",SqlDbType.Int).Value = id;
            await conn.OpenAsync();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                  return  reader.GetInt32(0) == 0;
                }
            }
        }
        
    return true;
    }

    
}