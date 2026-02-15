using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public record OrderedSeatReadDTO(int Id, int Row, int Col, int Status, int SectionId, 
        string ShowTitle, string ShowImgUrl, string ShowDate, string ShowBeginsAt);
}
