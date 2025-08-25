namespace PMS.Models
{
    public class TaskModel
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime Deadline { get; set; }

        public TaskState State { get; set; }

        public TaskPriority Priority { get; set; }

        public TaskModel(
            string title,
            string description,
            DateTime deadline,
            TaskState state,
            TaskPriority priority)
        {
            Title = title;
            Description = description;
            Deadline = deadline;
            State = state;
            Priority = priority;
        }
    }
}