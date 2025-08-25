using System.ComponentModel;

namespace PMS.Presentation.Sorting
{
    public enum SortOption
    {
        [Description("Tytuł A-Z")]
        TitleAsc,

        [Description("Tytuł Z-A")]
        TitleDesc,

        [Description("Priorytet ↑")]
        PriorityAsc,

        [Description("Priorytet ↓")]
        PriorityDesc,

        [Description("Deadline ↑")]
        DeadlineAsc,

        [Description("Deadline ↓")]
        DeadlineDesc
    }
}
