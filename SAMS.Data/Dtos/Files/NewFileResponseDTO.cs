using System.Collections.Generic;

namespace SAMS.Data.Dtos
{
    public class NewFileResponseDTO
    {
        public IList<string> Urls { get; set; }
    }    
    public class NewFileResponseDetailedDTO
    {
        public string FileName { get; set; }
        public int Uploaded { get; set; }
        public string Url { get; set; }
    }
}
