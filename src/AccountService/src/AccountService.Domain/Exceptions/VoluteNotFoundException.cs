namespace AccountService.Domain.Exceptions;

public class VoluteNotFoundException : Exception
{
    public VoluteNotFoundException(string message) : base(message) { }
}