using Microsoft.EntityFrameworkCore;
using TasksManagerApp.Models;

namespace TasksManagerApp {
	public class AppDbContext : DbContext{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){
			
		}

		public DbSet<TaskModel> Tasks { get; set; }
	}
}