namespace SocialMedia.Api.Response
{
    public class ApiSingleResponse <T>
    {
        public T Data { get; set; }
        public ApiSingleResponse(T data)
        {
            Data = data;
        }
    }
}
