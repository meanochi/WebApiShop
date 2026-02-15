using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public record ShowCreateDTO(
        string Title, 
        DateOnly Date, 
        TimeOnly BeginTime, 
        TimeOnly Duration, 
        string Audience, 
        string Sector, 
        string Description, 
        string ImgUrl, 
        int ProviderId, 
        int CategoryId);
}
