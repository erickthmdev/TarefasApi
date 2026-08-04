using System.Security.Claims;
using TarefasApi.Data;
using TarefasApi.DTOs;
using TarefasApi.Extensions;
using TarefasApi.Models;
using Microsoft.EntityFrameworkCore;
using TarefasApi.Enums;
using static TarefasApi.DTOs.TaskDtos;

namespace TarefasApi.Endpoints;

public static class TaskEndpoints
{
    public static void MapTaskEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/tasks").WithTags("Tasks").RequireAuthorization();

        group.MapGet("/", ListarTarefas);
        group.MapGet("/{id:int}", ObterTarefa);
        group.MapPost("/", CriarTarefa);
        group.MapPut("/{id:int}", AtualizarTarefa);
        group.MapPatch("/{id:int}/complete", ConcluirTarefa);
        group.MapDelete("/{id:int}", DeletarTarefa);
    }

    private static async Task<IResult> ListarTarefas(AppDbContext db, ClaimsPrincipal user, TaskType? type)
    {
        var userId = user.GetUserId();
        var query = db.Tasks.Where(t => t.UserId == userId);

        if (type is not null)
            query = query.Where(t => t.Type == type);

        var tarefas = await query.OrderBy(t => t.DueDate).ToListAsync();
        return Results.Ok(tarefas.Select(ToResponse));
    }

    private static async Task<IResult> ObterTarefa(int id, AppDbContext db, ClaimsPrincipal user)
    {
        var tarefa = await db.Tasks.FindAsync(id);
        if (tarefa is null || tarefa.UserId != user.GetUserId()) return Results.NotFound();
        return Results.Ok(ToResponse(tarefa));
    }

    private static async Task<IResult> CriarTarefa(CreateTaskRequest request, AppDbContext db, ClaimsPrincipal user)
    {
        var tarefa = new TaskItem
        {
            Title = request.Title,
            Description = request.Description,
            DueDate = request.DueDate,
            Type = request.Type,
            UserId = user.GetUserId()
        };

        db.Tasks.Add(tarefa);
        await db.SaveChangesAsync();
        return Results.Created($"/tasks/{tarefa.Id}", ToResponse(tarefa));
    }

    private static async Task<IResult> AtualizarTarefa(int id, UpdateTaskRequest request, AppDbContext db, ClaimsPrincipal user)
    {
        var tarefa = await db.Tasks.FindAsync(id);
        if (tarefa is null || tarefa.UserId != user.GetUserId()) return Results.NotFound();

        tarefa.Title = request.Title;
        tarefa.Description = request.Description;
        tarefa.DueDate = request.DueDate;
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    private static async Task<IResult> ConcluirTarefa(int id, AppDbContext db, ClaimsPrincipal user)
    {
        var tarefa = await db.Tasks.FindAsync(id);
        if (tarefa is null || tarefa.UserId != user.GetUserId()) return Results.NotFound();

        tarefa.IsCompleted = true;
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    private static async Task<IResult> DeletarTarefa(int id, AppDbContext db, ClaimsPrincipal user)
    {
        var tarefa = await db.Tasks.FindAsync(id);
        if (tarefa is null || tarefa.UserId != user.GetUserId()) return Results.NotFound();

        db.Tasks.Remove(tarefa);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    private static bool PodeAcessar(TaskItem tarefa, ClaimsPrincipal user) => tarefa.UserId == user.GetUserId();

    private static TaskResponse ToResponse(TaskItem t) =>
        new(t.Id, t.Title, t.Description, t.IsCompleted, t.DueDate, t.Type);
}