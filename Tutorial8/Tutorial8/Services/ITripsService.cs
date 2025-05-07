using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public interface ITripsService
{
    Task<List<TripDTO>> GetTrips();
    
    Task<Boolean> DoesTripExist(int id);
    Task<List<TripDTO>> GetTrip(int id);

    Task<Boolean> DoesClientExists(int id);

    Task<String> AddClient(ClientCreateDTO clientCreateDTO);

    Task<int> GetClientID(ClientCreateDTO clientCreateDTO);
    Task<Boolean> DoesTripExistByTripId(int id);

    Task<Boolean> DoesTripFullByTripId(int id);


    Task AddTrip(int id,int tripid);

    Task<Boolean> DoesTripExistByTripIdAndClientId(int id,int tripid);

    Task<Boolean> DeletetByTripIdAndClientId(int id,int tripid);
}