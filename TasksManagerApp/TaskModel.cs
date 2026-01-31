namespace TasksManagerApp {
	public class TaskModel {
		public int Id { get; set; }
		public string Description { get; set; }
		public DateTime DueDate { get; set; }
		public string Status { get; set; }
		public string AdditionalData { get; set; }
	}
}