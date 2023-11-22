namespace EShop.ViewModel
{
    public class Result<T>
    {
        public T? Data { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}