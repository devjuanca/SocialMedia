namespace SocialMedia.Api.Response
{
    public class ApiResponseWithMeta<T> : IHasMetadata
    {
        public T Data { get; set; }
        public Metadata Meta { get; set; }

        public ApiResponseWithMeta(T data)
        {
            Data = data;
        }
    }
}
