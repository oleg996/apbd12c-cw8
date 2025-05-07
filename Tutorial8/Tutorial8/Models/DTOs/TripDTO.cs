﻿using System.ComponentModel;

namespace Tutorial8.Models.DTOs;

public class TripDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<CountryDTO> Countries { get; set; }
    
    public string Descr {get; set;}
}

public class CountryDTO
{
    public string Name { get; set; }
}