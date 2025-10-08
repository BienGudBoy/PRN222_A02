using NguyenVanBien_SE18C.NET_A02.Models;

namespace NguyenVanBien_SE18C.NET_A02.Repositories
{
    public interface ITagRepository
    {
        List<Tag> GetAllTags();
        Tag? GetTagById(int tagId);
        void AddTag(Tag tag);
        void UpdateTag(Tag tag);
        void DeleteTag(int tagId);
        bool TagExists(int tagId);
    }
}
