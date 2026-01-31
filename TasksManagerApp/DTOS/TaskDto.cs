namespace TasksManagerApp.DTOS {
	public class TaskDto<T> {
		public int Id { get; set; }
		public string Description { get; set; }
		public DateTime DueDate { get; set; }
		public string Status { get; set; }
		public T AdditionalData  { get; set; }
	}
}
