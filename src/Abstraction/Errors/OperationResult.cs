using System.Globalization;

namespace Tekoding.KoIdentity.Abstraction.Errors;

/// <summary>
/// Represents the result of an operation.
/// </summary>
public class OperationResult
{
    /// <summary>
    /// Gets the state of the current operation indicating if the operation succeeded or not.
    /// </summary>
    public bool State => Payload is not Error[];
    
    /// <summary>
    /// Gets the payload of the operation containing an optional object for further use.
    /// </summary>
    /// <example>
    /// Containing an <b>Entity</b> when the operation succeeded and it was loaded successfully from the database or
    /// containing (can be multiple; at least one) <b>Error</b> if not.
    /// </example>
    public object? Payload { get; }

    /// <summary>
    /// Gets the amount of <see cref="Error"/>s, if any, assigned to the current operation.
    /// </summary>
    public int ErrorCount
    {
        get
        {
            if (Payload is Error[] errors)
            {
                return errors.Length;
            }

            return 0;
        }
    }

    private OperationResult(object? payload = null)
    {
        Payload = payload;
    }

    /// <summary>
    /// Creates a new operation result indicating a succeeded operation containing a payload.
    /// </summary>
    /// <param name="payload">The payload is getting assigned for further use.</param>
    /// <returns>Returns a new operation result indicating, that the operation succeeded.</returns>
    public static OperationResult SuccessWithPayload(object? payload) => new(payload);

    /// <summary>
    /// Creates a new operation result indicating a succeeded operation.
    /// </summary>
    public static OperationResult Success => new();

    /// <summary>
    /// Creates a new operation result indicating a failed operation.
    /// </summary>
    /// <param name="errors">An array of errors which caused the operation to fail.</param>
    /// <returns>Returns a new operation result indicating, that the operation failed.</returns>
    /// <exception cref="InvalidOperationException">Thrown, when no errors are assigned.</exception>
    public static OperationResult Failed(params Error[] errors) => errors.Length == 0
        ? throw new InvalidOperationException("At least one Error has to be assigned.")
        : new OperationResult(errors);

    /// <summary>
    /// Converts the value of the current operation result object to its equivalent string representation.
    /// </summary>
    /// <returns>Returns a string representation of the current operation result object.</returns>
    /// <remarks>
    /// If the operation was successful it will return <b>Succeeded</b> otherwise it returns <b>Failed: ...</b>
    /// followed by a comma delimited list of error codes from its error collection.
    /// </remarks>
    public override string ToString()
    {
        if (State)
        {
            return "Succeeded";
        }

        return string.Format(CultureInfo.InvariantCulture, "{0}: {1}", "Failed",
            string.Join("m ", ((IEnumerable<Error>)Payload!).Select(e => e.Code).ToList()));
    }
}