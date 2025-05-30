namespace MyCoursesApp.Commons {
    public class ResultType {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public static ResultType Ok(string message = "Success") => new() { Success = true, Message = message };
        public static ResultType Fail(string message) => new() { Success = false, Message = message };
    }

    public class ResultType<T> : ResultType {
        public T? Data { get; set; }
        public static new ResultType<T> Fail(string message) => new() { Success = false, Message = message };
        public static ResultType<T> Ok(T data, string message = "Success") => new() { Success = true, Message = message, Data = data };
    }
}
