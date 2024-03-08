using Core.Models;
using Core.SpecificationModel;

namespace API.Services
{
    public class EmployeeSpecification : BaseSpecification<Employee>
    {
        public EmployeeSpecification(int id) : base(x => x.Id == id)
        {
            
        }
        public EmployeeSpecification() : base()
        {
            
        }

        public void FiltersByCreatedBy(string userId)
        {
            Filter(x => x.CreatedBy == userId);
        }
    }
}
