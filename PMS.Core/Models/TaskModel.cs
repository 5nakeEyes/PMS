namespace PMS.Core.Models
{
    public class TaskModel
    {
        public string Title { get; }
        public string Description { get; }
        public DateTime DueDate { get; }
        public TaskState State { get; set; }
        public TaskPriority Priority { get; }

        public TaskModel(string title, string description, DateTime dueDate, TaskState state, TaskPriority priority)
        {
            Title = title;
            Description = description;
            DueDate = dueDate;
            State = state;
            Priority = priority;
        }
    }
}