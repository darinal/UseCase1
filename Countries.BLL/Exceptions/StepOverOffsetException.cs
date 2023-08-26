namespace Countries.BLL.Exceptions;

public class StepOverOffsetException : Exception
{
    public StepOverOffsetException() { }

    public StepOverOffsetException(string message) : base(message) { }

    public StepOverOffsetException(string message, Exception innerException) : base(message, innerException) { }
}