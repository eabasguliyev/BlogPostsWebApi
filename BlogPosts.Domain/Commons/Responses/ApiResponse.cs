using System.Collections.Generic;

namespace BlogPosts.Domain.Commons.Responses
{
    public record ValidationError(string FieldName, string ErrorMessage);

    public class ApiResponse<T> where T : IDto
    {
        public string ErrorMessage { get; set; }
        public bool IsSuccess { get; set; }
        public List<ValidationError> ValidationErrors { get; set; }
        public T Data { get; set; }


        public ApiResponse(T data)
        {
            Data = data;
            ErrorMessage = "";
            IsSuccess = true;
        }

        public ApiResponse(string errorMessage)
        {
            ErrorMessage = errorMessage;
            IsSuccess = false;
        }
    }
}