using System.Net;

namespace MailBank.Filters.StatusToBody
{
    public class BodyStatusAnswer
    {
        public int StatusCode { get; set; }

        public string Message { get; set; }
        public object Data { get; set; }

        public BodyStatusAnswer(int? statusCode, object data, string message = null)
        {
            StatusCode = statusCode ?? 200;
            Message = message;
            Data = data;
        }

        public BodyStatusAnswer(HttpStatusCode statusCode, object data, string message = null)
            : this((int)statusCode, data, message) { }


        public static BodyStatusAnswer Ok<T>(T data)
            => new BodyStatusAnswer<T>(HttpStatusCode.OK, data);

        public static BodyStatusAnswer NoContent<T>()
            => new BodyStatusAnswer(HttpStatusCode.NoContent, null);
        public static BodyStatusAnswer Empty(int? statusCode, string message = null)
            => new BodyStatusAnswer(statusCode, null, message);


    }


    public class BodyStatusAnswer<T> : BodyStatusAnswer
    {
        public BodyStatusAnswer(HttpStatusCode statusCode, T data) : base(statusCode, data)
        {
        }

        public BodyStatusAnswer(HttpStatusCode statusCode, T data, string message) : base(statusCode, data, message)
        {
        }
    }
}
