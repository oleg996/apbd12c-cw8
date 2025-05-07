using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tutorial8.Models.DTOs;

public class ClientCreateDTO
{
    [Length(1,120)]
  public required String FirstName{get;set;}
  [Length(1,120)]
  public required String LastName{get;set;}
  [EmailAddress]
  public required String Email{get;set;}
  [Phone]
  public required String phone{get;set;}
  [Length(11,11)]
  public required String pesel{get;set;}
    
}
