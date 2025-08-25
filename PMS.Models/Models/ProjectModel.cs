namespace PMS.Models
{
    public class ProjectModel
    {
        public string Name { get; set; }

        public DateTime Deadline { get; set; }

        public List<TaskModel> Tasks { get; set; }

        public ProjectModel(
            string name, 
            DateTime deadline)
        {
            Name = name;
            Deadline = deadline;
            Tasks = new List<TaskModel>();
        }
    }
}
