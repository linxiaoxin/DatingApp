namespace DatingApp.API.Helper
{
    public class MsgParams
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
        public string MessageContainer { get; set; } = "unread";
        
    }
}