using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIDemo.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string? Name{ get; set; }
        public byte Age { get; set; }
        public float Salary { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }

        //nsvigation property 
        public virtual Department? Department { get; set; }
        [ForeignKey(nameof(Department))]
        public int DepartmentId { get; set; }
    }
}
