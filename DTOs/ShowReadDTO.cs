using Entities;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public record ShowReadDTO(
        int Id, 
        string Title, 
        DateOnly Date, 
        TimeOnly BeginTime, 
        TimeOnly Duration, 
        string Audience, 
        string Sector, 
        string Description, 
        string ImgUrl, 
        string CategoryName,
        ICollection<OrderedSeat> OrderedSeats,
        string ProviderName,
        string ProviderProfileImgUrl,
        ICollection<Section> Sections
        );
}
