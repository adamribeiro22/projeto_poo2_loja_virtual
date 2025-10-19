namespace LojaVirtual.Application.Exception
{
    public class ValidationException : System.Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException() : base("Ocorreram um ou mais erros de validação.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IDictionary<string, string[]> errors) : this()
        {
            Errors = errors;
        }
    }
}
