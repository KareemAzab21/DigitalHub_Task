using Microsoft.EntityFrameworkCore;
using DigitalHub_Task.Models;

namespace DigitalHub_Task.Data
{
    public class TaskContext : DbContext
    {
        public TaskContext(DbContextOptions<TaskContext> options) : base(options) { }

        public DbSet<DigitalHub_Task.Models.Entity.Task> Tasks { get; set; }
    }

}
