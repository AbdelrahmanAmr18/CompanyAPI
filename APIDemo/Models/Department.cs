using Microsoft.AspNetCore.Connections;
using System.ComponentModel.DataAnnotations;

namespace APIDemo.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? DepartmentName{ get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        = new HashSet<Employee>();
    }
}
