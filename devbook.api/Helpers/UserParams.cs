namespace devbook.api.Helpers
{
    public class UserParams
    {
        
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize;}
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value;}
        }


        public int UserId { get; set; }
        public string Skill { get; set; }
        public string OrderBy { get; set; }

        public bool Favouritees { get; set; } = false;
        public bool Favouriters { get; set; } = false;


        



    }
}