using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureCommSvc.Application.Responses
{
    public class ValidationError
    {
        public string inpField { get; set; }
        public object errMessage { get; set; }
    }
    public enum ResultType
    {
        /// <summary>
        /// Success 
        /// </summary>
        Success = 1,
        Error = 2,
        ValidationError = 3,
        Warning = 4,
        NotFound = 5,
        Empty,
        UnAuthorized,
        Duplicate,
        Acknowledged
    }

    /// <summary>
    /// Base service response object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResponse<T> : Response
         where T : class
    {
        /// <summary>
        /// Indicates if the response contains a result
        /// </summary>
        public override bool HasResult
        {
            get
            {
                return this.ResponseData != null;
            }
        }

        /// <summary>
        /// The result of the response
        /// </summary>
        public new T ResponseData
        {
            get
            {
                return base.ResponseData as T;
            }

            set
            {
                base.ResponseData = value;
            }
        }

        /// <summary>
        /// Creates a successful response with a given result object
        /// </summary>
        /// <param name="result">The result object to return with the response</param>
        /// <returns>The response object</returns>
        public static ApiResponse<T> Success(T result)
        {
            var response = new ApiResponse<T> { ResultType = ResultType.Success, ResponseData = result };

            return response;
        }
        /// <summary>
        /// Creates a Acknowledged response with a given result object
        /// </summary>
        /// <param name="message">The result object to return with the response</param>
        /// <returns>The response object</returns>
        public static ApiResponse<T> Acknowledged(string message)
        {
            var response = new ApiResponse<T> { ResultType = ResultType.Acknowledged, Message = message };
            return response;
        }

        /// <summary>
        /// Creates a failed result. It takes no result object
        /// </summary>
        /// <param name="errorMessage">The error message returned with the response</param>
        /// <returns>The created response object</returns>
        public new static ApiResponse<T> Failed(string errorMessage)
        {
            var response = new ApiResponse<T> { ResultType = ResultType.Error, Message = errorMessage };

            return response;
        }

        /// <summary>
        /// Unauthorized
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static ApiResponse<T> UnAuthorized(string errorMessage)
        {
            var response = new ApiResponse<T> { ResultType = ResultType.UnAuthorized, Message = errorMessage };

            return response;
        }


        /// <summary>
        /// Creates a failed result for not found error. It takes no result object
        /// </summary>
        /// <param name="errorMessage">The error message returned with the response</param>
        /// <returns>The created response object</returns>
        public static ApiResponse<T> NotFound(string errorMessage)
        {
            var response = new ApiResponse<T> { ResultType = ResultType.NotFound, Message = errorMessage };

            return response;
        }
        /// <summary>
        /// Creates a success result with no data but a message. It takes no result object
        /// </summary>
        /// <param name="message">The message returned with the response</param>
        /// <returns>The created response object</returns>
        public static ApiResponse<T> EmptyWithMessage(string message)
        {
            var response = new ApiResponse<T> { ResultType = ResultType.Success, Message = message };

            return response;
        }


        /// <summary>
        /// Creates a failed result for not found error. It takes no result object
        /// </summary>
        /// <param name="errorMessage">The error message returned with the response</param>
        /// <returns>The created response object</returns>
        public static ApiResponse<T> DuplicateEntry(string errorMessage)
        {
            var response = new ApiResponse<T> { ResultType = ResultType.Duplicate, Message = errorMessage };

            return response;
        }

        /// <summary>
        /// Creates a validation error response, indicating the input was invalid
        /// </summary>
        /// <param name="validationMessages">The validation message</param>
        /// <returns>The Response object</returns>
        public new static ApiResponse<T> ValidationError(List<ValidationError> validationMessages, List<string> validationMessageslst)
        {
            var response = new ApiResponse<T> { ResultType = ResultType.ValidationError, Message = "Response has validation errors", ValidationErrors = validationMessages, ValidationMessages = validationMessageslst };

            return response;
        }

        public new static ApiResponse<T> ValidationError(List<string> validationMessages)
        {
            var response = new ApiResponse<T> { ResultType = ResultType.ValidationError, Message = "Response has validation errors", ValidationMessages = validationMessages };

            return response;
        }

        /// <summary>
        /// Creates a warning result. The warning result is successful, but might have issues that should be addressed or logged
        /// </summary>
        /// <param name="warningMessage">The warning returned with the response</param>
        /// <param name="result">The result object</param>
        /// <returns>The created response object</returns>
        public static ApiResponse<T> Warning(string warningMessage, T result)
        {
            var response = new ApiResponse<T>
            {
                ResultType = ResultType.Warning,
                Message = warningMessage,
                ResponseData = result
            };

            return response;
        }

        /// <summary>
        /// Creates an empty result. The empty result is successful, but might have issues that should be addressed or logged
        /// </summary>
        /// <returns>The created response object</returns>
        public static new ApiResponse<T> Empty()
        {
            var response = new ApiResponse<T> { ResultType = ResultType.Empty };

            return response;
        }


    }

    /// <summary>
    /// A simple response object with no returned object. Just indicates successful or failed requests
    /// </summary>
    public class Response
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Response"/> class. 
        /// </summary>
        public Response()
        {
            this.ResultType = ResultType.Success;
        }

        /// <summary>
        /// Indicates if the response contains a result
        /// </summary>
        public virtual bool HasResult
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// The result of the response
        /// </summary>
        public object ResponseData { get; protected set; }

        /// <summary>
        /// Indicates if the response is successful or not. Warning or success result type indicate success
        /// </summary>
        public bool Successful
        {
            get
            {
                return this.ResultType == ResultType.Success
                    || this.ResultType == ResultType.Warning;
            }
        }

        /// <summary>
        /// The result type
        /// </summary>
        public ResultType ResultType { get; set; }

        /// <summary>
        /// The message returned with the response
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The validation error messages returned with the response
        /// </summary>
        public List<string> ValidationMessages { get; set; }

        public List<ValidationError> ValidationErrors { get; set; }

        /// <summary>
        /// Creates a failed result. It takes no result object
        /// </summary>
        /// <param name="errorMessage">The error message returned with the response</param>
        /// <returns>The created response object</returns>
        public static Response Failed(string errorMessage)
        {
            var response = new Response { ResultType = ResultType.Error, Message = errorMessage };

            return response;
        }

        /// <summary>
        /// Creates a validation error response, indicating the input was invalid
        /// </summary>
        /// <param name="validationMessages">The validation message</param>
        /// <returns>The Response object</returns>
        public static Response ValidationError(List<string> validationMessages)
        {
            var response = new Response { ResultType = ResultType.ValidationError, ValidationMessages = validationMessages };

            return response;
        }


        public static Response ValidationError(List<ValidationError> validationMessages, List<string> validationMessagelst)
        {
            var response = new Response { ResultType = ResultType.ValidationError, ValidationErrors = validationMessages, ValidationMessages = validationMessagelst };
            return response;
        }


        /// <summary>
        /// Creates a warning result. The warning result is successful, but might have issues that should be addressed or logged
        /// </summary>
        /// <param name="warningMessage">The warning returned with the response</param>
        /// <returns>The created response object</returns>
        public static Response Warning(string warningMessage)
        {
            var response = new Response { ResultType = ResultType.Warning, Message = warningMessage };

            return response;
        }


        /// <summary>
        /// Creates an empty result. The empty result is successful, but might have issues that should be addressed or logged
        /// </summary>
        /// <returns>The created response object</returns>
        public static Response Empty()
        {
            var response = new Response { ResultType = ResultType.Empty };

            return response;
        }
    }
}
