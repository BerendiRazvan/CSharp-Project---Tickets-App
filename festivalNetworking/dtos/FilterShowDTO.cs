using System;

namespace festivalNetworking.dtos
{
    [Serializable]
    public class FilterShowDTO
    {
        public string artistName { get; set; }
        public DateTime showDay{ get; set; }


        public FilterShowDTO(string artistName, DateTime showDay)
        {
            this.artistName = artistName;
            this.showDay = showDay;
        }
        
        
        public override string ToString()
        {
            return artistName + ";" + showDay;
        }
        
    }
}