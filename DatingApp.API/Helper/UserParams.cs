namespace DatingApp.API.Helper
{
    public class UserParams
    {
        private const int maxPageSize = 50;

        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get 
            { 
                return _pageSize;
            }
            set 
            { 
                _pageSize = (value > maxPageSize)? maxPageSize: value;
            }
        }
        
        public int UserId { get; set; }
        public string Gender { get; set; }
        public int MinAge { get; set; } = 18;

        public int MaxAge { get; set; } = 99;
        public string OrderBy { get; set; }
        public bool isLikee { get; set; } = false ;
        public bool isLiker { get; set; } = false;

    }
}