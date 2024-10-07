namespace ToDoApp.UI
{
    public class ApiEndpoints
    {
        public string BaseUrl { get; private set; }
        public ApiEndpoints(string BaseUrl) 
        {
            if (string.IsNullOrEmpty(BaseUrl))
            {
                BaseUrl = "http://localhost:8080/api";
            }
            this.BaseUrl = BaseUrl;
        }
        public string ToDo { get => Combine(BaseUrl, "ToDo"); }
        public string User { get => Combine(BaseUrl, "User"); }
        public static string Combine(string uri1, string uri2)
        {
            uri1 = uri1.TrimEnd('/');
            uri2 = uri2.TrimStart('/');
            return string.Format("{0}/{1}", uri1, uri2);
        }
    }
}
