namespace HomeProject.Models.Request
{
    public class FilterBase
    {
        public bool IsActive { get; set; } = true;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 3;
        public int ItemsPerPage
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value < _pageSize) ? _pageSize : value;
            }
        }
    }
}
