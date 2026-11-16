using DMXDomain.Models;

namespace DMX.ViewModels
{


    public class CommentVM
    {
        public Guid RecordPublicId { get; set; }

        public int ModuleId { get; set; }

        public string NewComment { get; set; } = string.Empty;

        public List<CommentItemVM> Comments { get; set; } = new();

        public int CommentCount => Comments.Count;
    }

    public class CommentItemVM
    {
        public Guid PublicId { get; set; }

        public string Message { get; set; } = string.Empty;

        public string? Fullname { get; set; }

        public byte[]? Picture { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}