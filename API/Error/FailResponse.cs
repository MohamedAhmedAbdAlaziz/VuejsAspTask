using API.Errors;

namespace API.Error
{
    public class FailResponse : ApiResponse
    {
         
        public FailResponse(int statusCode, string message = null, string[] errors= null) : base(statusCode, message)
        {
            Errors = errors?? new string[] { GetDefaultMessageForStatusCode(statusCode) };
        }
        public string[] Errors { get; set; }
         
    }
}
