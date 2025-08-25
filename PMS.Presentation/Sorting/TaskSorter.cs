using PMS.Presentation.ViewModels;

namespace PMS.Presentation.Sorting
{
    public static class TaskSorter
    {
        public static IEnumerable<TaskViewModel> Sort(
            IEnumerable<TaskViewModel> tasks,
            SortOption option)
        {
            switch (option)
            {
                case SortOption.TitleAsc:
                    return tasks.OrderBy(t => t.Title);
                case SortOption.TitleDesc:
                    return tasks.OrderByDescending(t => t.Title);
                case SortOption.PriorityAsc:
                    return tasks.OrderBy(t => t.Priority);
                case SortOption.PriorityDesc:
                    return tasks.OrderByDescending(t => t.Priority);
                case SortOption.DeadlineAsc:
                    return tasks.OrderBy(t => t.Deadline);
                case SortOption.DeadlineDesc:
                    return tasks.OrderByDescending(t => t.Deadline);
                default:
                    return tasks;
            }
        }
    }
}