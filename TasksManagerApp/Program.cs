using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using TasksManagerApp;
using TasksManagerApp.DTOS;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add this line to register your DbContext
//builder.Services.AddDbContext<AppDbContext>(options =>
//	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("TodoList"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/tareas", async (AppDbContext db) => {
	return Results.Ok(new {
		ok = true,
		data = await db.Tasks.ToListAsync()
	});
});

app.MapGet("/tareas/{id}", async (int id, AppDbContext db) =>
	await db.Tasks.FindAsync(id)
		is TaskModel task
			? Results.Ok(new {
				data = task,
				ok = true
			})
			: Results.NotFound());



app.MapPost("/tareas", async (TaskDto<int> taskDto, AppDbContext db) => {


	try {

		if (taskDto == null) {
			throw new NullReferenceException("Trate de enviar correctamente los campos requeridos.");

		}

		if (string.IsNullOrEmpty(taskDto?.Description) || taskDto?.DueDate == null || taskDto?.AdditionalData == null) {
			throw new NullReferenceException("Trate de enviar correctamente los campos requeridos.");
		}

		var task = new TaskModel {
			Description = taskDto.Description,
			DueDate = taskDto.DueDate,
			Status = "New",
			AdditionalData = (taskDto.AdditionalData is System.Collections.IEnumerable)
				? string.Join(',', taskDto.AdditionalData)
				: taskDto.AdditionalData.ToString()
		};

		db.Tasks.Add(task);
		await db.SaveChangesAsync();

		return Results.Created($"/tareas/{task.Id}", task);

	} catch (NullReferenceException ex) {
		return Results.BadRequest(new {
			ok = false,
			message = ex.Message
		});
	} catch (Exception ex) {

		return Results.BadRequest(new {
			ok = false,
			message = ex.Message
		});
	}

});


app.MapPut("/tareas/{id}", async (int id, TaskDto<int> inputTask, AppDbContext db) => {
	try {
		var task = await db.Tasks.FindAsync(id);
		if (task is null) return Results.NotFound();

		// 1. Manual Validation Check
		if (inputTask.Description == null) return Results.BadRequest("Description is required.");

		// Update properties
		task.Description = inputTask.Description;
		task.Description = inputTask.Description;

		task.AdditionalData = task.AdditionalData = (inputTask.AdditionalData is System.Collections.IEnumerable)
			? string.Join(',', inputTask.AdditionalData)
			: inputTask.AdditionalData.ToString();

		await db.SaveChangesAsync();
		return Results.NoContent();
	} catch (DbUpdateConcurrencyException) {
		
		return Results.Conflict(new {
			ok = false,
			message = "Error de concurrencia: El registro ha sido modificado por otro proceso o usuario."
		});
	} catch (DbUpdateException) {
		return Results.BadRequest(new {
			ok = false,
			message = "Error al actualizar la base de datos. Verifique que los datos cumplan con los requisitos."
		});
	} catch (Exception) {
		
		return Results.Problem(detail: "Ocurrió un error inesperado al procesar su solicitud.");
	}
});

app.MapDelete("/tareas/{id}", async (int id, AppDbContext db) => {
	try {
		var task = await db.Tasks.FindAsync(id);

		if (task is null) {
			return Results.NotFound(new { ok = false, message = $"No se puede eliminar: No existe la tarea con ID {id}." });
		}

		db.Tasks.Remove(task);
		await db.SaveChangesAsync();

		return Results.NoContent();
	} catch (DbUpdateConcurrencyException) {
		// Ocurre si la tarea fue eliminada por otro usuario justo antes de procesar esta solicitud
		return Results.Conflict(new {
			ok = false,
			message = "El registro ya fue modificado o eliminado por otro usuario."
		});
	} catch (DbUpdateException) {
		// Común cuando hay llaves foráneas: la tarea tiene elementos hijos que impiden su borrado
		return Results.BadRequest(new {
			ok = false,
			message = "No se puede eliminar la tarea porque tiene registros relacionados vinculados."
		});
	} catch (Exception) {
		return Results.Problem(detail: "Ocurrió un error inesperado al intentar eliminar la tarea.");
	}
});


app.Run();