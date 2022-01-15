namespace CluedIn.ExternalSearch.Providers.CVR.Model
{
    public class Result<T>
    {
        public Result()
        {
        }

        public Result(string rawContent, T data)
        {
            this.RawContent = rawContent;
            this.Data       = data;
        }

        public string RawContent { get; set; }
        public T Data { get; set; }
    }
}