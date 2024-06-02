namespace SimCode.Web.Models.AppResponse
{
    public class ApiResponse
    {
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "Successful";
        public object Result { get; set; }
        public string StatusCode { get; set; } = "00";
    }
}
