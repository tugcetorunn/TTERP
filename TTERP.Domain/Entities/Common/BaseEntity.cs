namespace TTERP.Domain.Entities.Common
{
    public abstract class BaseEntity<TId> : IEntity<TId>, IAuditableEntity
    {
        public TId Id { get; protected set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime? UpdatedDate { get; private set; }
        public int? CreatedBy { get; private set; }
        public int? UpdatedBy { get; private set; }
        public bool IsActive { get; private set; } = true;
        public bool IsDeleted { get; private set; } = false;
        public DateTime? DeletedDate { get; private set; }
        public int? DeletedBy { get; private set; }
        public int? LanguageSupportId { get; set; } = 1; // opsiyonel dil desteği için // 1 : Türkçe

        protected BaseEntity()
        {
            CreatedDate = DateTime.UtcNow;
        }

        public void SetCreated(int? user)
        {
            CreatedBy = user;
        }

        public void SetUpdated(int? user)
        {
            UpdatedDate = DateTime.UtcNow;
            UpdatedBy = user;
        }

        public void SoftDelete(int? user)
        {
            IsDeleted = true;
            IsActive = false;
            DeletedDate = DateTime.UtcNow;
            DeletedBy = user;
        }
    }
}