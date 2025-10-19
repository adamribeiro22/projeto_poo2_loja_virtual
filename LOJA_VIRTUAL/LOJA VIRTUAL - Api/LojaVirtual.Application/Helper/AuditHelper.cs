namespace LojaVirtual.Infrastructure.Helper
{
    namespace Atelie.Core.Utils
    {
        public static class AuditHelper
        {
            public static TEntity UpdateAuditFields<TEntity>(TEntity entity) where TEntity : AuditableEntity
            {
                if (entity.Id == 0)
                {
                    entity.CriadoEm = DateTime.UtcNow;
                }

                entity.AtualizadoEm = DateTime.UtcNow;

                return entity;
            }
        }
    }
}
