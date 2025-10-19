public abstract class AuditableEntity
{
    public int Id { get; protected set; }
    public DateTime CriadoEm { get; set; }
    public DateTime AtualizadoEm { get; set; }
}