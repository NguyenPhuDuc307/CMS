namespace CMS.Data.Entities
{
    public interface IDateTracking
    {
        DateTime CreateDate { get; set; }

        DateTime? LastModifiedDate { get; set; }
    }
}