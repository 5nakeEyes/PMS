namespace PMS.Domain.Models
{
    public class TaskModel
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskState State { get; set; }
        public TaskPriority Priority { get; set; }

        public TaskModel() { }

        public TaskModel(
            string title,
            string description,
            DateTime dueDate,
            TaskState state,
            TaskPriority priority)
        {
            Title = title;
            Description = description;
            DueDate = dueDate;
            State = state;
            Priority = priority;
        }
    }
}